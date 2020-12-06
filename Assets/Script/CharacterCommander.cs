/*
 * 角色指揮官。
 */
using System.Collections.Generic;
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
    /// 依照位置排序站好，從左上開始為起始點(index:0)。將依照角色中之sortingIndex屬性為參考。
    /// </summary>
    public void SortingBySeat(List<Vector3> seats)
    {
        
    }



}
