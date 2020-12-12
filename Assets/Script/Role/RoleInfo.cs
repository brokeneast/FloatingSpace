/*
 * 角色資訊。
 */
using System;

[Serializable]
public class RoleInfo
{
    public string id;
    public int seatIndex;//位置索引值
    public string displayName; //名稱
    public string description;
    public string tag;
    public int skin;//選擇的外觀

    public RoleInfo(int seatIndex)
    {
        id = Guid.NewGuid().ToString("N");
        this.seatIndex = seatIndex;
    }

    public RoleInfo(string name, string description, string tag ,int seatIndex)
    {
        id = Guid.NewGuid().ToString("N");
        displayName = name;
        this.description = description;
        this.seatIndex = seatIndex;
        this.tag = tag;
    }

}
