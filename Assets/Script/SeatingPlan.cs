/*
 * 集合位置安排，每位角色都有其指定座位，先有位子，方能產生角色。
 */
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SeatingPlan
{
    public List<Seat> seats { get; private set; }//就定位座位
    public delegate void ChangeSeatHandler(List<Seat> seats);
    public static event ChangeSeatHandler OnSeatChanged;//座位產生了變動

    //Auto
    private Vector3 autoSeatRefPos = Vector3.zero;//角色自動排列時就定位參考位置，以此點為參考位置(參考y,z為主)
    private float seatSpace = 1.5f;//座位距離
    private float characterWidth = 1f;//角色寬度
    private float characterHeight = 1f;//角色寬度

    public SeatingPlan()
    {
        seats = new List<Seat>();
        SetAllSeatPos(AutoCreateSeat(1));//預產生一個空位
    }

    /// <summary>
    /// 開啟位置計畫，並指定開啟座位。
    /// </summary>
    public SeatingPlan(int amount)
    {
        seats = new List<Seat>();
        SetAllSeatPos(AutoCreateSeat(amount));
    }

    /// <summary>
    /// 重置座位。
    /// </summary>
    public void Clear()
    {
        seats.Clear();
        seats = new List<Seat>();
        SetAllSeatPos(AutoCreateSeat(1));//預產生一個空位
    }


    #region Seat Modify

    /// <summary>
    /// 創建座位。
    /// </summary>
    public void CreateSeat()
    {

    }

    /// <summary>
    /// 設座位位置至列表座位資訊。
    /// </summary>
    public void SetAllSeatPos(List<Vector3> seatPos)
    {
        for(int i=0; i<seatPos.Count; i++)
        {
            if(i <= seats.Count - 1)//規劃的座位位置，尚未超出目前座位數量
            {
                seats[i].seatPos = seatPos[i];
            }
            else
            {
                //增加座位
                seats.Add(new Seat(seatPos[i]));
            }
        }
        OnSeatChanged?.Invoke(seats);
    }

    /// <summary>
    /// 取得目前所有座位位置。
    /// </summary>
    public List<Vector3> GetAllSeatPos()
    {
        List<Vector3> s = new List<Vector3>();
        s = seats.Select(p => p.seatPos).ToList();
        return s;
    }

    /// <summary>
    /// 寫入座位位置根據索引值。
    /// </summary>
    public void SetSeatPos(Vector3 pos, int seatIndex)
    {
        if (seatIndex < seats.Count)
        {
            seats[seatIndex].seatPos = pos;
            OnSeatChanged?.Invoke(seats);
        }
    }

    /// <summary>
    /// 檢查座位是否已被使用。
    /// </summary>
    public bool CheckSeatBeUsed(int seatIndex)
    {
        return string.IsNullOrEmpty(seats[seatIndex].characterId)? false : true;
    }

    /// <summary>
    /// 設定座位角色。
    /// </summary>
    public void SetCharacter(string characterId, int seatIndex)
    {
        if (seatIndex < seats.Count)
        {
            seats[seatIndex].characterId = characterId;
            OnSeatChanged?.Invoke(seats);
        }
    }
    #endregion

    #region Auto Create Seats
    /// <summary>
    /// 自動產生就定位位置
    /// </summary>
    public List<Vector3> AutoCreateSeat(int memberAmount)
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
    private void CreatSeatPos(int amountOfRow,int amountOfColumn, ref int amount, ref List<Vector3> seats)
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
                seats.Add(new Vector3(leftX + (i * (characterWidth + seatSpace))+ autoSeatRefPos.x, currentColumnOffset + autoSeatRefPos.y, autoSeatRefPos.z));
            }

            currentColumnOffset = currentColumnOffset -( seatSpace + characterHeight);
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
}
