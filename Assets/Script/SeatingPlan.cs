/*
 * 集合位置安排，每位角色都有其指定座位，先有位子，方能置入角色。
 */
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SeatingPlan
{
    public string planName;//計畫名稱
    public List<Seat> seats;//就定位座位
  
    public SeatingPlan(string planName)
    {
        this.planName = planName;
        seats = new List<Seat>();
    }
}
