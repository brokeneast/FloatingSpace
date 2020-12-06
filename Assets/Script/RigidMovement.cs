/*
 * 浮動空間中剛體的移動模式
 */
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RigidMovement : Movement
{
    public Rigidbody body;
    private bool stopRigi;//靜止Rigi

    void Start()
    {
        Floating();
    }


    /// <summary>
    /// 停止不動。
    /// </summary>
    public override void Stop()
    {
        //Rigid
        stopRigi = true;

        //一般
        base.Stop();
    }

    /// <summary>
    /// 還原動作。
    /// </summary>
    public override void Resume()
    {
        //Rigid
        stopRigi = false;

        //一般
        base.Resume();
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
            if (hasLookRotation) //有轉向指定
            {
                body.freezeRotation = true;
            }
            else
            {
                body.freezeRotation = false;
            }
        }

        base.FixedUpdate();

        if (stopRigi)
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
    

    private void OnTriggerExit(Collider others)
    {
        if (others.gameObject.tag == "FloatingSpace")//角色正離開空間
        {
            transform.position = new Vector3( -transform.position.x, -transform.position.y,0);
        }
    }

}
