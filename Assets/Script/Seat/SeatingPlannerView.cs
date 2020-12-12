using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SeatingPlanner))]
public class SeatingPlannerView : MonoBehaviour
{
    #region Settings
    [Header("Mode Switch")]
    [SerializeField] GameObject seatingPlanerWindow = null;//席位設定視窗
    [SerializeField] GameObject seatingPosAnchors = null;//席位位置擺放編輯器
    [SerializeField] GameObject seatingPosChangedComfirm = null;//席位位置擺放確認鍵
    [SerializeField] GameObject floatingSpace = null;//編輯位置時須關閉空間
    [SerializeField] SpaceDirector spaceDirector = null;//編輯時，須停止其他動作。
    [SerializeField] GameObject objectCreatedArea = null;//編輯時，須隱藏遊戲物件。

    [Header("Seat")]
    [SerializeField] SeatingPlanner seatingPlanner = null;
    [SerializeField] GameObject seatingPanelPrefab = null;//單一座位安排介面
    [SerializeField] GameObject seatingPanelContainer = null;//座位安排介面清單
    [SerializeField] SeatingPosSettingsView seatingPosSettingsView = null;//座位位置控制茅點放置
    [SerializeField] Text seatAmountText = null;
    #endregion

    public List<SeatingPanel> seatPanels = new List<SeatingPanel>();

    private void OnEnable()
    {
        seatingPlanner.OnSeatsRefreshed += Refresh;
        seatingPlanner.OnSeatAdded += AddSeat;
        seatingPlanner.OnSeatDeleted += DeleteSeat;
        seatingPlanner.OnSeatModified += ModifySeat;
        seatingPlanner.OnCharacterSwapped += SwapCharacter;
    }

    private void OnDisable()
    {
        seatingPlanner.OnSeatsRefreshed -= Refresh;
        seatingPlanner.OnSeatAdded -= AddSeat;
        seatingPlanner.OnSeatDeleted -= DeleteSeat;
        seatingPlanner.OnSeatModified -= ModifySeat;
        seatingPlanner.OnCharacterSwapped -= SwapCharacter;
    }

    #region Settings Window
    /// <summary>
    /// 刷新所有座位編輯畫面。
    /// </summary>
    /// <param name="seats"></param>
    private void Refresh(List<Seat> seats)
    {
        for (int i = 0; i < seatPanels.Count; i++)
        {
            Destroy(seatPanels[i].gameObject);
        }

        seatPanels.Clear();

        for (int i = 0; i < seats.Count; i++)
        {
            SeatingPanel sp = Instantiate(seatingPanelPrefab, seatingPanelContainer.transform).GetComponent<SeatingPanel>();
            sp.gameObject.transform.SetSiblingIndex(i);
            sp.Init(i, seats[i]);
            seatPanels.Add(sp);
        }

        SeatPosViewAndAmountRefresh();
    }

    /// <summary>
    /// 增加座位。
    /// </summary>
    private void AddSeat(Seat seat)
    {
        AddSeatPanelAndInit(seatPanels.Count, seat);
        SeatPosViewAndAmountRefresh();
        
    }

    /// <summary>
    /// 刪除該項。
    /// </summary>
    private void DeleteSeat(int index)
    {
        Destroy(seatPanels[index].gameObject);
        seatPanels.RemoveAt(index);
        RefreshSeatIndex();
        SeatPosViewAndAmountRefresh();
    }

    /// <summary>
    /// 座位內容變動。
    /// </summary>
    private void ModifySeat(int index, Seat s)
    {
        seatPanels[index].Init(index,s);
    }

    /// <summary>
    /// 角色座位互換。
    /// </summary>
    private void SwapCharacter(RoleInfo a, RoleInfo b)
    {
        seatPanels[a.seatIndex].SetCharacter(a);
        seatPanels[b.seatIndex].SetCharacter(b);
    }

    /// <summary>
    /// 加入席位介面並初始化。
    /// </summary>
    private void AddSeatPanelAndInit(int i, Seat s)
    {
        SeatingPanel sp = Instantiate(seatingPanelPrefab, seatingPanelContainer.transform).GetComponent<SeatingPanel>();
        sp.gameObject.transform.SetSiblingIndex(i);
        sp.Init(i, s);
        seatPanels.Add(sp);
    }

    /// <summary>
    /// 刷新座位序號。
    /// </summary>
    private void RefreshSeatIndex()
    {
        for (int i = 0; i < seatPanels.Count; i++)
        {
            seatPanels[i].SetSeatIndex(i);
        }
    }

    private void SeatPosViewAndAmountRefresh()
    {
        seatAmountText.text = string.Format("{0}/{1}", seatPanels.Count, SeatingPlanner.maxSeatAmount);
        seatingPosSettingsView.Refresh(seatingPlanner.seatingPlan.seats);
    }

    #endregion

    #region Menu
    public void OpenSeatingPlannerWindow()
    {
        seatingPlanerWindow.SetActive(true);
        floatingSpace.SetActive(true);
        seatingPosAnchors.SetActive(true);
        seatingPosChangedComfirm.SetActive(false);
        spaceDirector.Stop();
        objectCreatedArea.SetActive(false);
    }

    public void CloseSeatingPlannerWindow()
    {
        seatingPlanerWindow.SetActive(false);
        floatingSpace.SetActive(true);
        seatingPosAnchors.SetActive(false);
        seatingPosChangedComfirm.SetActive(false);
        objectCreatedArea.SetActive(true);
        spaceDirector.Init();
    }

    public void OpenSeatingPosSettingsMode()
    {
        seatingPosAnchors.SetActive(true);
        seatingPosChangedComfirm.SetActive(true);
        seatingPlanerWindow.SetActive(false);
        floatingSpace.SetActive(false);
    }
    #endregion
}
