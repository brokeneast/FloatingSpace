/*
 * 物件移動指揮官。
 */

using System.Collections.Generic;

public abstract class Commander<TMovement>
    where TMovement : Movement
{
    protected List<TMovement> members = new List<TMovement>();

    /// <summary>
    /// 加入成員。
    /// </summary>
    public abstract void AddMember(TMovement movement);

    /// <summary>
    /// 移除成員。
    /// </summary>
    public abstract void RemoveMember(TMovement movement);

    /// <summary>
    /// 全部隨意浮動。
    /// </summary>
    public virtual void Floating()
    {
        for (int i = 0; i < members.Count; i++)
        {
            members[i].Floating();
        }
    }

    /// <summary>
    /// 全部停止。
    /// </summary>
    public virtual void Stop()
    {
        for(int i = 0; i < members.Count; i++)
        {
            members[i].Stop();
        }
    }

    /// <summary>
    /// 全部恢復動作。
    /// </summary>
    public virtual void Resume()
    {
        for (int i = 0; i < members.Count; i++)
        {
            members[i].Resume();
        }
    }

    /// <summary>
    /// 向前看起。
    /// </summary>
    public virtual void LookForward()
    {
        for (int i = 0; i < members.Count; i++)
        {
            members[i].LookForward();
        }
    }

    ~Commander()
    {
        members.Clear();
        members = null;
    }
}
