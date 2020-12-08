using UnityEngine;

[System.Serializable]
public class Seat
{
    public Vector3 seatPos = Vector3.zero;//座位位置
    public string characterId;//角色id
    public Vector3 rotation = Vector3.zero;//旋轉

    public Seat(Vector3 seatPos)
    {
        this.seatPos = seatPos;
        characterId = "";
    }
}
