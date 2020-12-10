/*
 * 座位安排總管。
 */
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class SeatingPlanner : MonoBehaviour
{
    public string seatingPlanName = "SeatingPlane";
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



    public const int maxSeatAmount = 30;//最大座位數

    //Auto
    private Vector3 autoSeatRefPos = Vector3.zero;//角色自動排列時就定位參考位置，以此點為參考位置(參考y,z為主)
    private float seatSpace = 1.5f;//座位距離
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
        //PlayerPrefs.DeleteAll();//全部資料清空
        //檢查並載入資料
        seatingPlan = new SeatingPlan(seatingPlanName);

        if (CheckData(seatingPlanName))
        {
            Load();
        }
        else
        {
            Save();
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
            Save();
        }
    }

    /// <summary>
    /// 刪除座位。
    /// </summary>
    public void DeleteSeat(int seatIndex)
    {
        seatingPlan.seats.RemoveAt(seatIndex);
        OnSeatDeleted?.Invoke(seatIndex);
        Save();
    }

    /// <summary>
    /// 設定座位角色。
    /// </summary>
    public void SetCharacter(RoleInfo roleInfo, int seatIndex)
    {
        if (roleInfo == null)
        {
            seatingPlan.seats[seatIndex].character = null;
        }
        else if (seatIndex < seatingPlan.seats.Count)
        {
            roleInfo.seatIndex = seatIndex;
            seatingPlan.seats[seatIndex].character = roleInfo;
        }

        OnSeatModified?.Invoke(seatIndex, seatingPlan.seats[seatIndex]);
    }

    /// <summary>
    /// 刪除該席位的角色。
    /// </summary>
    public void DeleteCharacter(int seatIndex)
    {
        seatingPlan.seats[seatIndex].character = null;
        seatingPlan.seats[seatIndex].character.seatIndex = -1;
        Save();
        OnSeatDeleted?.Invoke(seatIndex);
    }

    /// <summary>
    /// 角色互換。
    /// </summary>
    public void SwapCharacter(int indexA, int indexB)
    {
        RoleInfo c = seatingPlan.seats[indexA].character;
        seatingPlan.seats[indexA].character = seatingPlan.seats[indexB].character;
        seatingPlan.seats[indexB].character = c;
        seatingPlan.seats[indexA].character.seatIndex = indexA;
        seatingPlan.seats[indexB].character.seatIndex = indexB;
        OnCharacterSwapped?.Invoke(seatingPlan.seats[indexA].character, seatingPlan.seats[indexB].character);
    }

    /// <summary>
    /// 設座位位置至列表座位資訊。
    /// </summary>
    public void SetAllSeatPos(List<Vector3> seatPos)
    {
        for (int i = 0; i < seatPos.Count; i++)
        {
            if (i <= seatingPlan.seats.Count - 1)//規劃的座位位置，尚未超出目前座位數量
            {
                seatingPlan.seats[i].seatPos = seatPos[i];
            }
            else
            {
                //增加座位
                seatingPlan.seats.Add(new Seat(seatPos[i]));
            }
        }
        OnSeatsRefreshed?.Invoke(seatingPlan.seats);
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

    /// <summary>
    /// 寫入座位位置根據索引值。
    /// </summary>
    public void SetSeatPos(Vector3 pos, int seatIndex)
    {
        if (seatIndex < seatingPlan.seats.Count)
        {
            seatingPlan.seats[seatIndex].seatPos = pos;
            OnSeatModified?.Invoke(seatIndex, seatingPlan.seats[seatIndex]);
        }
    }

    /// <summary>
    /// 檢查座位是否已被使用。
    /// </summary>
    public bool CheckSeatBeUsed(int seatIndex)
    {
        return string.IsNullOrEmpty(seatingPlan.seats[seatIndex].character.id) ? false : true;
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
        Debug.Log(amountOfColumn);
        CreatSeatPos(amountOfRow, amountOfColumn, ref remainAmount, ref s);

        return s;
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
        Debug.Log("Load seating data: " + JsonUtility.ToJson(seatingPlan));
    }

    /// <summary>
    /// 資料讀出。
    /// </summary>
    public void Load()
    {
        Debug.Log("Load seating data: " + PlayerPrefs.GetString(seatingPlanName));
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
    #endregion
}
