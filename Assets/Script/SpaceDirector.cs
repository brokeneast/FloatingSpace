/*
 * 空間支配者。
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceDirector : MonoBehaviour
{
    public GameObject characterPrefab;//成員物件
    public List<Vector3> seats;//就定位座標
    private CharacterCommander characterCommander;//角色指令官
    private SeatingPlan seatingPlan;//座位安排

    private void Start()
    {
        //替角色們指派一位指揮官
        characterCommander = new CharacterCommander();
        seatingPlan = new SeatingPlan();
        seats = seatingPlan.AutoCreateSeat(18);
    }

    /// <summary>
    /// 增加新成員到空間中(預設)。
    /// </summary>
    public void AddNewMember()
    {
        var c = Instantiate(characterPrefab, Vector3.zero, Quaternion.identity);
        characterCommander.AddMember(c.GetComponent<Character>());//將其依附在指揮官下
    }

    /// <summary>
    /// 增加新成員到空間中。
    /// </summary>
    public void AddNewMember(RoleInfo info)
    {
        var c = Instantiate(characterPrefab, Vector3.zero, Quaternion.identity);
        Character character = c.GetComponent<Character>();
        character.Init(info);
        characterCommander.AddMember(character);//將其依附在指揮官下
    }

    #region Space control
    /// <summary>
    /// 閒置狀態。
    /// </summary>
    public void Floating()
    {
        characterCommander.Floating();
    }

    /// <summary>
    /// 時間停止。
    /// </summary>
    public void Stop()
    {
        characterCommander.Stop();
    }

    /// <summary>
    /// 復原。
    /// </summary>
    public void Resume()
    {
        characterCommander.Resume();
    }

    /// <summary>
    /// 向前看起。
    /// </summary>
    public void LookForward()
    {
        characterCommander.LookForward();
    }
    #endregion
}
