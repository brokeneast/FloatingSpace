using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class SeatPanel : MonoBehaviour
{
    [Header("Seat")]
    [SerializeField] Text seatIndexText = null;
    [Header("Character")]
    [SerializeField] GameObject roleInfoSettingsWindow = null;
    [SerializeField] GameObject characterPanel = null;
    [SerializeField] GameObject settingPanel = null;
    [SerializeField] Text nameText = null;
    [SerializeField] Text tagText = null;

    Canvas canvas;
    public int seatIndex { get; private set; }
    public Seat seat { get; private set; }
    private SeatingPlanner planner;


    private void Awake()
    {
        planner = FindObjectOfType<SeatingPlanner>();
        canvas = FindObjectOfType<Canvas>();
    }

    /// <summary>
    /// 初始化座位安排介面。
    /// </summary>
    public void Init(int seatIndex, Seat seat)
    {
        this.seat = seat;
        SetSeatIndex(seatIndex);

        SetCharacterInfoPanel();
    }

    /// <summary>
    /// 設定指令索引值
    /// </summary>
    /// <param name="seatIndex"></param>
    public void SetSeatIndex(int seatIndex)
    {
        this.seatIndex = seatIndex;

        //座位索引
        seatIndexText.text = ((char)(seatIndex + 65)).ToString();
        gameObject.name = seatIndexText.text;
    }

    public void SetCharacter(RoleInfo character)
    {
        seat.character = character;
        SetCharacterInfoPanel();
    }

    /// <summary>
    /// 刪除席位。
    /// </summary>
    public void DeleteSeat()
    {
        planner.DeleteSeat(seatIndex);
        Debug.Log("刪除" + gameObject.name);
    }

    /// <summary>
    /// 在該席位上安排人員。
    /// </summary>
    public void AddCharacter()
    {
        RoleInfoSettingsWindow window = Instantiate(roleInfoSettingsWindow, canvas.transform).GetComponent<RoleInfoSettingsWindow>();
        if(seat.character == null || string.IsNullOrEmpty(seat.character.id))
        {
            seat.character = new RoleInfo(seatIndex);
        }
        window.Init(seat.character, RoleInfoModified);
    }

    private void RoleInfoModified(RoleInfo info)
    {
        planner.SetCharacter(info, seatIndex);
        SetCharacterInfoPanel();
    }

    /// <summary>
    /// 移除該席位上已安排的人員。
    /// </summary>
    public void DeleteCharacter()
    {
        planner.SetCharacter(null, seatIndex);
    }

    /// <summary>
    /// 該位置之角色上移。
    /// </summary>
    public void CharacterMoveUp()
    {
        if (seatIndex > 0)
            planner.SwapCharacter(seatIndex, seatIndex - 1);
    }

    /// <summary>
    /// 該位置之角色下移。
    /// </summary>
    public void CharacterMoveDown()
    {
        if (seatIndex < planner.seatingPlan.seats.Count - 1)
            planner.SwapCharacter(seatIndex, seatIndex + 1);
    }

    /// <summary>
    /// 角色資料介面。
    /// </summary>
    private void SetCharacterInfoPanel()
    {
        //人員確認
        if (seat.character == null || string.IsNullOrEmpty(seat.character.id))
        {
            settingPanel.SetActive(true);
            characterPanel.SetActive(false);
        }
        else
        {
            settingPanel.SetActive(false);
            characterPanel.SetActive(true);
            nameText.text = seat.character.displayName + " " + seat.character.description;
            tagText.text = seat.character.tag;
        }
    }
}
