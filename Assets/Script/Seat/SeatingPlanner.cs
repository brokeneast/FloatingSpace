﻿/*
 * 座位安排總管。
 */
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class SeatingPlanner : MonoBehaviour
{
    public string seatingPlanName = "SeatingPlan";
    public SeatingPlan seatingPlan;
    private static SeatingPlanner instance;

    public delegate void RefreshSeatHandler(List<Seat> seats);
    public event RefreshSeatHandler OnSeatsRefreshed;//全部刷新
    public delegate void AddSeatHandler(Seat seat);
    public event AddSeatHandler OnSeatAdded;//增加座位
    public delegate void DeleteSeatHandler(int removeIndex);
    public event DeleteSeatHandler OnSeatDeleted;//座位被刪
    public delegate void SwapCharacterHandler(RoleInfo a, RoleInfo b);
    public event SwapCharacterHandler OnCharacterSwapped;//角色座位互換
    public delegate void ModifySeatHandler(int index, Seat seat);
    public event ModifySeatHandler OnSeatModified;//席位資訊更動

    public const int maxSeatAmount = 36;//最大座位數

    //Auto
    private Vector3 autoSeatRefPos = Vector3.zero;//角色自動排列時就定位參考位置，以此點為參考位置(參考y,z為主)
    private float seatSpace = 1.25f;//座位距離
    private float characterWidth = 1f;//角色寬度
    private float characterHeight = 1f;//角色寬度

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if (this != instance)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //檢查並載入資料
        seatingPlan = new SeatingPlan(seatingPlanName);

        if (CheckData(seatingPlanName))
        {
            Load();
        }
        else
        {
            LoadTemplate(0);//載入預設檔案
        }
    }


    #region Seat Modify
    /// <summary>
    /// 創建空座位。
    /// </summary>
    public void AddSeat()
    {
        if (CheckSeatBeAdded())
        {
            Seat s = new Seat(Vector3.zero);
            seatingPlan.seats.Add(s);
            OnSeatAdded?.Invoke(s);
            AutoSeatPosAndSave();
        }
    }

    /// <summary>
    /// 刪除座位。
    /// </summary>
    public void DeleteSeat(int seatIndex)
    {
        seatingPlan.seats.RemoveAt(seatIndex);
        OnSeatDeleted?.Invoke(seatIndex);
        AutoSeatPosAndSave();
    }

    /// <summary>
    /// 重製座位。
    /// </summary>
    public void ResetSeat()
    {
        SetAllSeatPos(AutoCreateSeatPos(seatingPlan.seats.Count), true);
    }

    /// <summary>
    /// 設定座位角色。
    /// </summary>
    public void SetCharacter(RoleInfo roleInfo, int seatIndex)
    {
        if (roleInfo == null)
        {
            seatingPlan.seats[seatIndex].role = null;
        }
        else if (seatIndex < seatingPlan.seats.Count)
        {
            roleInfo.seatIndex = seatIndex;
            seatingPlan.seats[seatIndex].role = roleInfo;
        }

        OnSeatModified?.Invoke(seatIndex, seatingPlan.seats[seatIndex]);
        Save();
    }

    public string GetCharacterId(int seatIndex)
    {
        string id = "";
        if(seatIndex< seatingPlan.seats.Count)
        {
            id = seatingPlan.seats[seatIndex].role.id;
        }
       
        return id;
    }

    /// <summary>
    /// 刪除該席位的角色。
    /// </summary>
    public void DeleteCharacter(int seatIndex)
    {
        seatingPlan.seats[seatIndex].role = null;
        seatingPlan.seats[seatIndex].role.seatIndex = -1;
        Save();
        OnSeatDeleted?.Invoke(seatIndex);
    }

    /// <summary>
    /// 角色互換。
    /// </summary>
    public void SwapCharacter(int indexA, int indexB)
    {
        RoleInfo c = seatingPlan.seats[indexA].role;
        seatingPlan.seats[indexA].role = seatingPlan.seats[indexB].role;
        seatingPlan.seats[indexB].role = c;
        seatingPlan.seats[indexA].role.seatIndex = indexA;
        seatingPlan.seats[indexB].role.seatIndex = indexB;
        OnCharacterSwapped?.Invoke(seatingPlan.seats[indexA].role, seatingPlan.seats[indexB].role);
        Save();
    }

    /// <summary>
    /// 設座位位置至列表座位資訊。
    /// </summary>
    public void SetAllSeatPos(List<Vector3> seatPos, bool ignoreCustomPos)
    {
        for (int i = 0; i < seatPos.Count; i++)
        {
            if (i <= seatingPlan.seats.Count - 1)//規劃的座位位置，尚未超出目前座位數量
            {
                if (!ignoreCustomPos)
                {
                    if (!seatingPlan.seats[i].isCustomPos)
                    {
                        seatingPlan.seats[i].seatPos = seatPos[i];
                    }
                }
                else
                {
                    seatingPlan.seats[i].seatPos = seatPos[i];
                }
                
            }
            else
            {
                //增加座位
                seatingPlan.seats.Add(new Seat(seatPos[i]));
            }
        }
        OnSeatsRefreshed?.Invoke(seatingPlan.seats);
        Save();
    }

    /// <summary>
    /// 取得目前所有座位位置。
    /// </summary>
    public List<Vector3> GetAllSeatPos()
    {
        List<Vector3> s = new List<Vector3>();
        s = seatingPlan.seats.Select(p => p.seatPos).ToList();
        return s;
    }

    public Vector3 GetSeatPos(int seatIndex)
    {
        return seatingPlan.seats[seatIndex].seatPos;
    }

    /// <summary>
    /// 寫入座位位置根據索引值。
    /// </summary>
    public void SetSeatPos(int seatIndex, Vector3 pos, bool isCustom)
    {
        if (seatIndex < seatingPlan.seats.Count)
        {
            seatingPlan.seats[seatIndex].seatPos = pos;
            seatingPlan.seats[seatIndex].isCustomPos = isCustom;
            OnSeatModified?.Invoke(seatIndex, seatingPlan.seats[seatIndex]);
            Save();
        }
    }

    /// <summary>
    /// 檢查座位是否已被使用。
    /// </summary>
    public bool CheckSeatBeUsed(int seatIndex)
    {
        return string.IsNullOrEmpty(seatingPlan.seats[seatIndex].role.id) ? false : true;
    }

    /// <summary>
    /// 確認是否可以在增加座位。
    /// </summary>
    /// <returns></returns>
    public bool CheckSeatBeAdded()
    {
        return seatingPlan.seats.Count >= maxSeatAmount ? false : true;
    }

    #endregion
    #region Auto Create Seat Pos
    /// <summary>
    /// 自動產生就定位位置
    /// </summary>
    public List<Vector3> AutoCreateSeatPos(int memberAmount)
    {
        List<Vector3> s = new List<Vector3>();

        int remainAmount = memberAmount;//尚未排上之數量
        int amountOfRow = (int)((FloatingSpace.spaceWidth - seatSpace) / (seatSpace + characterWidth));//一排放幾個
        int amountOfColumn = Mathf.CeilToInt((float)memberAmount / (float)amountOfRow);
        CreatSeatPos(amountOfRow, amountOfColumn, ref remainAmount, ref s);

        return s;
    }

    /// <summary>
    /// 根據位置序位取得自動產生的預設位置。
    /// </summary>
    public Vector3 GetAutoPos(int index)
    {
        return AutoCreateSeatPos(seatingPlan.seats.Count)[index];
    }

    /// <summary>
    /// 依照剩餘數量排位置。
    /// </summary>
    private void CreatSeatPos(int amountOfRow, int amountOfColumn, ref int amount, ref List<Vector3> seats)
    {
        float currentColumnOffset = -GetInitPos(amountOfColumn, characterHeight, seatSpace);//目前行的位移

        while (amount > 0)
        {
            int currentAmountOfRow = amountOfRow;
            if (amount < amountOfRow)//最後一排
            {
                currentAmountOfRow = amount;
                amount = 0;
            }
            else//尚未最後一排
            {
                amount = amount - currentAmountOfRow;
            }

            float leftX = GetInitPos(currentAmountOfRow, characterWidth, seatSpace);//最左

            //開始排
            for (int i = 0; i < currentAmountOfRow; i++)
            {
                seats.Add(new Vector3(leftX + (i * (characterWidth + seatSpace)) + autoSeatRefPos.x, currentColumnOffset + autoSeatRefPos.y, autoSeatRefPos.z));
            }

            currentColumnOffset = currentColumnOffset - (seatSpace + characterHeight);
        }
    }

    /// <summary>
    /// 找座位起始值(最左或最上)。
    /// </summary>
    private float GetInitPos(int amount, float characterLength, float seatSpace)
    {
        return -(((int)(amount / 2) * characterWidth) - ((amount % 2 == 0) ? characterWidth / 2f : 0) + (((amount - 1f) / 2f) * seatSpace));
    }
    #endregion

    #region Data Saver
    /// <summary>
    /// 資料確認。
    /// </summary>
    /// <returns></returns>
    public bool CheckData(string name)
    {
        if (PlayerPrefs.GetString(name) == "")
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    /// <summary>
    /// 儲存資料。
    /// </summary>
    public void Save()
    {
        PlayerPrefs.SetString(seatingPlanName, JsonUtility.ToJson(seatingPlan));
    }


    /// <summary>
    /// 位置重整並儲存
    /// </summary>
    public void AutoSeatPosAndSave()
    {
        SetAllSeatPos(AutoCreateSeatPos(seatingPlan.seats.Count),false);
    }


    /// <summary>
    /// 資料讀出。
    /// </summary>
    public void Load()
    {
        JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(seatingPlanName), seatingPlan);
        OnSeatsRefreshed?.Invoke(seatingPlan.seats);
    }

    /// <summary>
    /// 刪除。
    /// </summary>
    public void Delete()
    {
        //刪除
        PlayerPrefs.DeleteKey(seatingPlanName);
    }

    /// <summary>
    /// 僅讀取席位資訊。
    /// </summary>
    public void LoadTemplate(int index)
    {
        SeatingPlan sp = new SeatingPlan(seatingPlanName);
        JsonUtility.FromJsonOverwrite(LoadResourceTextfile("Template/" + index.ToString()), sp);
        seatingPlan = sp;
        Save();
        Load();
    }


    private string LoadResourceTextfile(string path)
    {
        string filePath = path.Replace(".json", "");
        TextAsset targetFile = Resources.Load<TextAsset>(filePath);
        return targetFile.text;
    }
    #endregion
}
