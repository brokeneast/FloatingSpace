using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeatingPlan
{
    public  List<Vector3> seats { get; private set; }//就定位座標
    //Auto
    private Vector3 autoSeatRefPos = Vector3.zero;//角色自動排列時就定位參考位置，以此點為參考位置(參考y,z為主)
    private float seatSpace = 1.5f;//座位距離
    private float characterWidth = 1f;//角色寬度
    private float characterHeight = 1f;//角色寬度

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
}
