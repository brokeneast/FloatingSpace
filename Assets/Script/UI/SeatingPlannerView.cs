using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SeatingPlanner))]
public class SeatingPlannerView : MonoBehaviour
{
    [SerializeField] SeatingPlanner seatingPlanner = null;
    [SerializeField] GameObject seatingPanelPrefab = null;//單一座位安排介面
    [SerializeField] GameObject seatingPanelContainer = null;//座位安排介面放置區域
    [SerializeField] Text seatAmountText = null;
    public List<SeatPanel> seatPanels = new List<SeatPanel>();

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
            SeatPanel sp = Instantiate(seatingPanelPrefab, seatingPanelContainer.transform).GetComponent<SeatPanel>();
            sp.gameObject.transform.SetSiblingIndex(i);
            sp.Init(i, seats[i]);
            seatPanels.Add(sp);
        }

        SeatAmountRefresh();
    }

    /// <summary>
    /// 增加座位。
    /// </summary>
    private void AddSeat(Seat seat)
    {
        AddSeatPanelAndInit(seatPanels.Count, seat);
        SeatAmountRefresh();
    }

    /// <summary>
    /// 刪除該項。
    /// </summary>
    private void DeleteSeat(int index)
    {
        Destroy(seatPanels[index].gameObject);
        seatPanels.RemoveAt(index);
        RefreshSeatIndex();
        SeatAmountRefresh();
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
        SeatPanel sp = Instantiate(seatingPanelPrefab, seatingPanelContainer.transform).GetComponent<SeatPanel>();
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

    private void SeatAmountRefresh()
    {
        seatAmountText.text = string.Format("{0}/{1}", seatPanels.Count, SeatingPlanner.maxSeatAmount);
    }
}
