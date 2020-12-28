using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : RigidMovement
{
    [SerializeField] TextMesh nameText = null;
    public Seat seat;//席位及個人資訊

    public enum MovingMode {FLOATING, REST, IN_SEAT};//移動狀態

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
        nameText.text = seat.role.displayName;
    }

}
