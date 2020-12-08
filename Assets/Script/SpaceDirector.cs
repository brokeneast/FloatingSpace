/*
 * 空間支配者。
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceDirector : MonoBehaviour
{
    public GameObject characterPrefab;//成員物件
    //characterCrator
    private CharacterCommander characterCommander;//角色指令官

    private void Start()
    {
        //替角色們指派一位指揮官
        characterCommander = new CharacterCommander();
        //初始化
        //等待角色生成完成
        //characterCrator
    }

    void Arrival()
    {
        Debug.Log("Arrive");
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
    /// 依照指定席位打招呼。
    /// </summary>
    public void SayHello()
    {
        characterCommander.LookForward("0");
        characterCommander.GoTo("0", new Vector3(0, 0, -7), 10f, true, Arrival);
    }

    /// <summary>
    /// 退回當前打招呼之角色。
    /// </summary>
    public void Back()
    {
        characterCommander.BackFromDestination("0");
    }

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
