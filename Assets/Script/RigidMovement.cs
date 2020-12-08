/*
 * 浮動空間中剛體的移動模式
 */
using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class RigidMovement : Movement
{
    public Rigidbody body;
    public Collider theCollider;
    private bool stopRigid;//靜止Rigi，停止一切物理動作

    protected override void Start()
    {
        base.Start();
        Floating();
    }


    /// <summary>
    /// 停止不動。
    /// </summary>
    public override void Stop()
    {
        //Rigid
        stopRigid = true;

        //一般
        base.Stop();
    }

    /// <summary>
    /// 還原動作。
    /// </summary>
    public override void Resume()
    {
        //Rigid
        stopRigid = false;

        //一般
        base.Resume();
    }


    public void GoTo(Vector3 targetPos, float speed, bool ignoreCollider)
    {
        base.GoTo(targetPos, speed);
        theCollider.enabled = !ignoreCollider;
    }

    public void GoTo(Vector3 targetPos, float speed,bool ignoreCollider, Action onArrival)
    {
        base.GoTo(targetPos, speed, onArrival);
        theCollider.enabled = !ignoreCollider;
    }

    /// <summary>
    /// 從目的地回歸後。
    /// </summary>
    protected override void OnBackFromDestination()
    {
        SpeedReset(true, true);//速度回復
        Floating();//繼續浮動
        theCollider.enabled = true;//恢復碰撞
    }

    /// <summary>
    /// 自動不斷變換方向及漂浮。
    /// </summary>
    /// <returns></returns>
    IEnumerator DelayAndCreateMovingSettings()
    {
        yield return new WaitForSeconds(3);
        CreateMovingSettings();
        yield return StartCoroutine(DelayAndCreateMovingSettings());
    }

    protected override void FixedUpdate()
    {
        if (isRotating)
        {
            if (hasAimingTarget || hasAssignedRotation) //有轉向指定
            {
                body.freezeRotation = true;
            }
            else
            {
                body.freezeRotation = false;
            }
        }

        base.FixedUpdate();

        if (stopRigid)
        {
            body.velocity = Vector3.zero;
            body.freezeRotation = true;
        }


    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Blocker")//角色已撞到結界
        {
            CreateReflectedMovingSettings(other.contacts[0].normal);
        }
        else if (other.gameObject.tag == "Character")//角色已撞到結界
        {
            CreateReflectedMovingSettings(other.contacts[0].normal);
            StopRotating();//撞到同為角色的話，交給物理去執行。
        }
    }
    
    /*
    private void OnTriggerExit(Collider others)
    {
        if (others.gameObject.tag == "FloatingSpace")//角色正離開空間
        {
            transform.position = new Vector3( -transform.position.x, -transform.position.y,0);
        }
    }
    */
}
