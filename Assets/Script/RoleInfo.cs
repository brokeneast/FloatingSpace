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

    public RoleInfo()
    {
        //id = Guid.NewGuid().ToString("N");
        id = "0";
    }
}
