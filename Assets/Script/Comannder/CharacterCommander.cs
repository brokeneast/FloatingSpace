/*
 * 角色指揮官。
 */
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterCommander : Commander<Character>
{
    public CharacterCommander()
    {
        members = new List<Character>();
    }

    public CharacterCommander(List<Character> members)
    {
        this.members = members;
    }

    /// <summary>
    /// 加入成員。
    /// </summary>
    public override void AddMember(Character movement)
    {
        members.Add(movement);
    }

    /// <summary>
    /// 移除成員。
    /// </summary>
    public override void RemoveMember(Character movement)
    {
        members.Remove(movement);
    }

    /// <summary>
    /// 前往指定的位子，依照位置排序站好，每個位置都有對應的角色id，及他的位置、轉向等資訊。
    /// </summary>
    public void GoToSeat(List<Seat> seats, float speed, Action OnArrival)
    {
        for (int i = 0; i < seats.Count; i++)
        {
            Character c = GetCharacter(seats[i].role.id);
            if (c != null)
            {
                c.RotateTo(seats[i].rotation); //旋轉至指定角度
                c.GoTo(seats[i].seatPos, speed, true, OnArrival);//移動前往，忽略碰撞
            }
        }
    }

    #region Assign
    /// <summary>
    /// 全部隨意浮動。
    /// </summary>
    public void Floating(string characterId)
    {
        Character c = GetCharacter(characterId);
        if (c != null)
        {
            c.Resume();
        }
    }

    /// <summary>
    /// 指定特定角色停止。
    /// </summary>
    public void Stop(string characterId)
    {
        Character c = GetCharacter(characterId);
        if (c != null)
        {
            c.Stop();
        }
    }

    /// <summary>
    /// 指定特定角色恢復動作。
    /// </summary>
    public void Resume(string characterId)
    {
        Character c = GetCharacter(characterId);
        if (c != null)
        {
            c.Resume();
        }
    }

    /// <summary>
    /// 指定特定角色到特定位置。
    /// </summary>
    public void GoTo(string characterId, Vector3 pos)
    {
        Character c = GetCharacter(characterId);
        if (c != null)
        {
            c.GoTo(pos);
        }
    }

    /// <summary>
    /// 指定特定角色到特定位置，可不受空間碰撞限制。
    /// </summary>
    public void GoTo(string characterId, Vector3 pos, float speed, bool ignoreCollider)
    {
        Character c = GetCharacter(characterId);
        if (c != null)
        {
            c.GoTo(pos, speed, ignoreCollider);
        }
    }

    public void GoTo(string characterId, Vector3 pos, float speed, bool ignoreCollider, Action onArrival)
    {
        Character c = GetCharacter(characterId);
        if (c != null)
        {
            c.GoTo(pos, speed, ignoreCollider, onArrival);
        }
    }

    public void BackFromDestination(string characterId)
    {
        Character c = GetCharacter(characterId);
        if (c != null)
        {
            c.BackFromDestination();
        }
    }

    /// <summary>
    /// 指定特定角色向前看起。
    /// </summary>
    public void LookForward(string characterId)
    {
        Character c = GetCharacter(characterId);
        if (c != null)
        {
            c.LookForward();
        }
    }

    /// <summary>
    /// 指定特定角色轉向特定方向。
    /// </summary>
    public void LookTo(string characterId, Vector3 targetPos)
    {
        Character c = GetCharacter(characterId);
        if (c != null)
        {
            c.LookTo(targetPos);
        }
    }

    private Character GetCharacter(string roleId)
    {
        try
        {
            return members.First(p => roleId == p.seat.role.id);
        }
        catch
        {
            return null;
        }
    }

    #endregion

}
