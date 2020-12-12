/*
 * 席位編輯茅點。
 */
using UnityEngine;

public class SeatingPosAnchor : MonoBehaviour
{
    [SerializeField] TextMesh seatIndexText = null;
    SeatingPlanner seatingPlanner;
    private int index;
    private Seat seat;

    //Edit
    Vector3 dist;
    Vector3 startPos;
    float posX;
    float posZ;
    float posY;


    private void Start()
    {
        seatingPlanner = FindObjectOfType<SeatingPlanner>();
        gameObject.name = SeatIndexHelper.SeatIndexToKeyCode(index);
    }

    public void Init(int index, Seat s)
    {
        this.index = index;
        seat = s;
        string i = SeatIndexHelper.SeatIndexToKeyCode(index);
        seatIndexText.text = i;
        transform.position = seat.seatPos;
        transform.rotation = Quaternion.Euler(seat.rotation);
    }

    void OnMouseDown()
    {
        Debug.Log("Drag");
        startPos = transform.position;//抓物件起始位置，世界座標
        dist = Camera.main.WorldToScreenPoint(startPos);//轉換物件為螢幕座標
        posX = Input.mousePosition.x - dist.x;//算物件與點擊間的差距
        posY = Input.mousePosition.y - dist.y;
        posZ = Input.mousePosition.z - dist.z;

    }

    void OnMouseDrag()
    {
        float disX = Input.mousePosition.x - posX;//移動，並扣掉原本的差距
        float disY = Input.mousePosition.y - posY;
        float disZ = Input.mousePosition.z - posZ;

        Vector3 lastPos = Camera.main.ScreenToWorldPoint(new Vector3(disX, disY, disZ));//轉換物件為世界座標，新的座標
        transform.position = new Vector3(lastPos.x, lastPos.y, lastPos.z);//移動物件
    }

    void OnMouseUp()
    {
        //回寫
        seatingPlanner.SetSeatPos(index, transform.position, true);
    }
}
