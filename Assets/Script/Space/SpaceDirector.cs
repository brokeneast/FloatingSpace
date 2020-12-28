/*
 * 空間支配者。
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceDirector : MonoBehaviour
{
    [Header("Seat")]
    [SerializeField] SeatingPlanner seatingPlanner = null;
    [Header("Character")]
    [SerializeField] GameObject objectCreatedArea = null;//角色生所成的物件下
    [SerializeField] RoleCreator roleCreator = null;
    private CharacterCommander characterCommander;//角色指令官

    //Pop
    [Range(8,20)]
    public float popOutSpeed = 10;
    public Vector3 popOutPos = new Vector3(0, 0, -7);
    private string currentPopCharacterId;//當前跳出之成員

    //BackToSeat
    [Range(2,10)]
    public float backToSeatSpeed = 4;
    private void Start()
    {
        //初始化
        Init();
    }

    /// <summary>
    /// 初始化。
    /// </summary>
    public void Init()
    {
        StartCoroutine(InitProcess());
    }
    
    private IEnumerator InitProcess()
    {
        yield return StartCoroutine(CharacterInit());
        yield return null;
    }

    /// <summary>
    /// 角色初始。
    /// </summary>
    /// <returns></returns>
    private IEnumerator CharacterInit()
    {
        //替角色們指派一位指揮官
        characterCommander = new CharacterCommander();
        ClearAllObjects();
        AddNewMember(seatingPlanner.seatingPlan);
        yield return null;
    }

    /// <summary>
    /// 清除所有漂浮空間中的物件。
    /// </summary>
    private void ClearAllObjects()
    {
        foreach (Transform o in objectCreatedArea.transform)
        {
            Destroy(o.gameObject);
        }
    }

    /// <summary>
    /// 根據席位配製增加新成員到空間中。
    /// </summary>
    private void AddNewMember(SeatingPlan seatingPlan)
    {
        for(int i = 0; i < seatingPlan.seats.Count; i++)
        {
            RoleInfo roleInfo = seatingPlan.seats[i].role;
            if(roleInfo != null && !string.IsNullOrEmpty(roleInfo.id))
            {
                var c = roleCreator.CreateRole(roleInfo);
                c.transform.SetParent(objectCreatedArea.transform);
                Character character = c.GetComponent<Character>();
                character.Init(seatingPlan.seats[i]);
                characterCommander.AddMember(character);//將其依附在指揮官下
            }
        }
       
    }
    

    #region Space control
    /// <summary>
    /// 依照指定席位打招呼。
    /// </summary>
    public void PopOut(int index)
    {
        string id = seatingPlanner.GetCharacterId(index);
        if (!string.IsNullOrEmpty(id) && id != currentPopCharacterId)
        {
            Back();
            currentPopCharacterId = id;
            characterCommander.LookForward(id);
            characterCommander.GoTo(id, popOutPos, popOutSpeed, true, ArrivalPopPlace);
        }

    }

    /// <summary>
    /// 抵達打招呼位置後。
    /// </summary>
    private void ArrivalPopPlace()
    {

    }

    /// <summary>
    /// 退回當前打招呼之角色。
    /// </summary>
    public void Back()
    {
        if (!string.IsNullOrEmpty(currentPopCharacterId))
        {
            characterCommander.BackFromDestination(currentPopCharacterId);
            currentPopCharacterId = "";
        }

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
    /// 全部望前看。
    /// </summary>
    public void LookForward()
    {
        characterCommander.LookForward();
    }

    /// <summary>
    /// 回歸席位。
    /// </summary>
    public void BackToSeat()
    {
        characterCommander.GoToSeat(seatingPlanner.seatingPlan.seats, backToSeatSpeed, ArrivalSeat);
    }

    private void ArrivalSeat()
    {

    }

    #endregion
}
