﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Character : RigidMovement
{
    public RoleInfo roleInfo { get; private set; }

    public enum MovingMode {FLOATING, REST, IN_PLACE};//移動狀態
    public enum ActionMode {SWIMING, HOLD_HANDS, RAISE_HAND};//動畫狀態

    /// <summary>
    /// 角色初始化。
    /// </summary>
    /// <param name="info"></param>
    public void Init(RoleInfo info)
    {
        roleInfo = info;
    }
}