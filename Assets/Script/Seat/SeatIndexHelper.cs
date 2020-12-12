using System;
using UnityEngine;

public static class SeatIndexHelper
{
    public static string SeatIndexToKeyCode(int index)
    {
        string keyCode = "";
        //index 0~25 -> 對應到A-Z || a-z
        //index 26~36 -> 對應到0~9
        if (index<=36)
        {
            if (index < 26 && index >= 0)
            {
                keyCode = ((char)(index + 65)).ToString();
            }
            else
            {
                keyCode = ((char)(index - 26 + 48)).ToString();
            }
        }
        return keyCode;
    }

    public static int SeatKeyCodeToIndex(char key)
    {
        int index = -1;
        int ascii =Convert.ToInt32(key);

        //index 0~25 -> 對應到A-Z || a-z
        //index 26~36 -> 對應到0~9
        if (ascii <= 90 && ascii >= 65)
        {
            index = ascii - 65;
        }
        else if (ascii <= 122 && ascii >= 97)
        {
            index = ascii - 97;
        }
        else if (ascii <= 57 && ascii >= 48)
        {
            index = ascii - 48 + 26;
        }

        return index;
    }
}
