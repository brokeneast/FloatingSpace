using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Character : RigidMovement
{
    public Seat seat;//席位及個人資訊

    public enum MovingMode {FLOATING, REST, IN_SEAT};//移動狀態
    public enum ActionMode {SWIMING_FRONT_CRAWL, SWIMING_BREASTSTROKE, HOLD_HANDS, RAISE_HAND};//動畫狀態

    protected override void Start()
    {
        transform.position = seat.seatPos;
        transform.rotation = Quaternion.Euler(seat.rotation);
        base.Start();
    }

    /// <summary>
    /// 角色初始化。
    /// </summary>
    public void Init(Seat seat)
    {
        this.seat = seat;
    }

}
