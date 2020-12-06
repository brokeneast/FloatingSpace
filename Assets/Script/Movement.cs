/*
 * 浮動空間中的移動模式
 */
using UnityEngine;

public abstract class Movement : MonoBehaviour
{
    //移動
    [Range(0.5f,3f)]
    public float movingSpeed = 1;
    [Range(0.001f, 0.01f)]
    public float rotateSpeed = 0.004f;

    //自動
    protected Vector3 rotatingEuler = Vector3.zero;
    protected Vector3 movingDirection = Vector3.zero;

    //指定
    private Vector3 destination; //要移動到的位置

    protected bool isRotating;
    protected bool isMoving;

    protected bool hasLookRotation;//已設定要旋轉至的方向，因此將忽略自轉等情形
    protected bool hasMovingDestination;//已設定要移動的地點，因此將忽略自主移動情形

    /// <summary>
    /// 隨意浮動。
    /// </summary>
    public virtual void Floating()
    {
        CreateMovingSettings();
    }

    /// <summary>
    /// 停止不動。
    /// </summary>
    public virtual void Stop()
    {
        //一般
        StopRotating();
        StopMoving();
    }

    /// <summary>
    /// 還原動作。
    /// </summary>
    public virtual void Resume()
    {
        //一般
        StartRotating(rotatingEuler);
        StartMoving(movingDirection);
    }

    /// <summary>
    /// 移向目標物。
    /// </summary>
    public virtual void GoTo(Vector3 targetPos)
    {
        RotateTo(targetPos);
    }

    /// <summary>
    /// 望向目標物。
    /// </summary>
    public virtual void LookTo(Vector3 targetPos)
    {
        RotateTo(targetPos);
    }

    /// <summary>
    /// 轉回為前方。
    /// </summary>
    public virtual void LookForward()
    {
        RotateTo(transform.forward);
    }


    /// <summary>
    /// 旋轉。
    /// </summary>
    protected virtual void StartRotating(Vector3 euler)
    {
        isRotating = true;
        hasLookRotation = false;
        rotatingEuler = euler;
    }

    /// <summary>
    /// 停止旋轉。
    /// </summary>
    protected virtual void StopRotating()
    {
        isRotating = false;
    }


    /// <summary>
    /// 旋轉至目標位置方向(望向)。
    /// </summary>
    protected virtual void RotateTo(Vector3 targetPos)
    {
        isRotating = true;
        hasLookRotation = true;
        destination = targetPos;
    }

    /// <summary>
    /// 取消望向特定方向，但可能還會持續自主旋轉。
    /// </summary>
    protected virtual void StopRotateToTarget()
    {
        hasLookRotation = false;
    }

    /// <summary>
    /// 沿特定方向移動。
    /// </summary>
    protected virtual void StartMoving(Vector3 direction)
    {
        isMoving = true;
        hasMovingDestination = false;
        movingDirection = direction;
    }

    /// <summary>
    /// 停止移動。
    /// </summary>
    protected virtual void StopMoving()
    {
        isMoving = false;
    }

    /// <summary>
    /// 移動至特定位置。
    /// </summary>
    protected virtual void MoveTo(Vector3 targetPos)
    {
        isMoving = true;
        hasMovingDestination = true;
        destination = targetPos;
    }

    /// <summary>
    /// 取消前往特定位置，但可能還會持續自主移動。
    /// </summary>
    protected virtual void StopMoveToTarget()
    {
        hasMovingDestination = false;
    }

    /// <summary>
    /// 創建新移動設定。
    /// </summary>
    protected virtual void CreateMovingSettings()
    {
        rotatingEuler = new Vector3(Random.Range(-180, 180), Random.Range(-180, 180), 0);
        StartRotating(rotatingEuler);
        movingDirection = new Vector3(Mathf.RoundToInt(Random.Range(-1, 1)), 1, 0);
        StartMoving(movingDirection);
    }


    /// <summary>
    /// 創建反射效果之新移動設定。
    /// </summary>
    protected virtual void CreateReflectedMovingSettings(Vector3 normal)
    {
        movingDirection = Vector3.Reflect(movingDirection, normal);
        StartMoving(movingDirection);
    }

    /// <summary>
    /// 一般浮動，不使用物理引擎時的方法(isKinematic)。
    /// </summary>
    protected void NormalFloating()
    {
        if (isRotating)
        {
            if (hasLookRotation) //有轉向指定
            {
                Vector3 direction = (destination - transform.position).normalized;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, Quaternion.LookRotation(direction).z), Time.deltaTime*rotateSpeed*200);
            }
            else
            {
                transform.Rotate(rotatingEuler * rotateSpeed);//自轉
            }
        }

        if (isMoving)
        {
            if (hasMovingDestination)
            {
                transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * movingSpeed);
            }
            else
            {
                transform.Translate(movingDirection * movingSpeed * Time.deltaTime, Space.World);//沿方向移動
            }
        }
    }

    protected virtual void FixedUpdate()
    {
        NormalFloating();
    }
}
