/*
 * 浮動空間中的移動模式
 */
using System;
using UnityEngine;

public abstract class Movement : MonoBehaviour
{
    //移動
    [Range(0.5f, 3f)]
    public float movingSpeed = 1;
    [Range(0.001f, 0.01f)]
    public float rotatingSpeed = 0.004f;

    private float _movingSpeed;
    private float _rotatingSpeed;

    protected bool isRotating;
    protected bool isMoving;

    //自動移動(自由浮動)
    protected Vector3 rotatingEuler = Vector3.zero;//自動移動時的旋轉角
    protected Vector3 movingDirection = Vector3.zero;//自動移動時的移動方向

    //指定移動(有特定目的地)
    private Vector3 destination; //要移動到的位置
    private Vector3 rotation;//指定旋轉的角度
    private Action OnArrivalDestination;//抵達特定位置後動作
    private bool isArrival;
    private Vector3 comeBackPos;//當從指定點回來時的原本位置

    protected bool hasAimingTarget;//已設定要瞄準的方向，因此將忽略自轉等情形
    protected bool hasAssignedRotation;//已設定要旋轉至的方向，因此將忽略自轉等情形
    protected bool hasMovingDestination;//已設定要移動的地點，因此將忽略自主移動情形

    protected virtual void Start()
    {
        //備份初始速度
        _movingSpeed = movingSpeed;
        _rotatingSpeed = rotatingSpeed;
    }

    /// <summary>
    /// 隨意浮動。
    /// </summary>
    public virtual void Floating()
    {
        SpeedReset(true,true);
        CreateMovingSettings();
        Resume();
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
    /// 恢復動作。
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
        comeBackPos = transform.position;
        MoveTo(targetPos);
    }

    public virtual void GoTo(Vector3 targetPos, float speed)
    {
        comeBackPos = transform.position;
        MoveTo(targetPos);
        movingSpeed = speed;
    }

    public virtual void GoTo(Vector3 targetPos, Action onArrival)
    {
        comeBackPos = transform.position;
        MoveTo(targetPos);
        OnArrivalDestination = onArrival;
    }


    public virtual void GoTo(Vector3 targetPos, float speed, Action onArrival)
    {
        comeBackPos = transform.position;
        MoveTo(targetPos);
        movingSpeed = speed;
        OnArrivalDestination = onArrival;
    }

    /// <summary>
    /// 從前往目的地的途中折返。
    /// </summary>
    public virtual void BackFromDestination()
    {
        MoveTo(comeBackPos);
        OnArrivalDestination = OnBackFromDestination;
    }

    /// <summary>
    /// 望向目標物。
    /// </summary>
    public virtual void LookTo(Vector3 targetPos)
    {
        TurnToward(targetPos);
    }

    /// <summary>
    /// 轉回為前方。
    /// </summary>
    public virtual void LookForward()
    {
        TurnToward(transform.forward);
    }

    /// <summary>
    /// 轉向指定角度。
    /// </summary>
    public virtual void RotateTo(Vector3 rotation)
    {
        isRotating = true;
        hasAimingTarget = false;
        hasAssignedRotation = true;
        this.rotation = rotation;
    }

    /// <summary>
    /// 速度回歸至初始值。
    /// </summary>
    public void SpeedReset(bool resetMovingSpeed, bool resetRotatingSpeed)
    {
        movingSpeed = resetMovingSpeed ? _movingSpeed : movingSpeed;
        rotatingSpeed = resetRotatingSpeed ? _rotatingSpeed : rotatingSpeed;
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
    /// 取消轉為特定方向，但可能還會持續自主旋轉。
    /// </summary>
    protected virtual void StopRotateToDirection()
    {
        hasAssignedRotation = false;
    }

    /// <summary>
    /// 開始自行旋轉。
    /// </summary>
    protected virtual void StartRotating(Vector3 euler)
    {
        isRotating = true;
        hasAimingTarget = false;
        hasAssignedRotation = false;
        rotatingEuler = euler;
    }

    /// <summary>
    /// 停止自行旋轉。
    /// </summary>
    protected virtual void StopRotating()
    {
        isRotating = false;
    }


    /// <summary>
    /// 朝向目標位置方向(望向)。
    /// </summary>
    protected virtual void TurnToward(Vector3 targetPos)
    {
        isRotating = true;
        hasAimingTarget = true;
        hasAssignedRotation = false;
        destination = targetPos;
    }

    /// <summary>
    /// 取消望向特定方向，但可能還會持續自主旋轉。
    /// </summary>
    protected virtual void StopTurnToward()
    {
        hasAimingTarget = false;
    }

    /// <summary>
    /// 從目的地回歸後。
    /// </summary>
    protected virtual void OnBackFromDestination()
    {
        SpeedReset(true, true);//速度回復
        Floating();//繼續浮動
    }

    /// <summary>
    /// 創建新移動設定。
    /// </summary>
    protected virtual void CreateMovingSettings()
    {
        rotatingEuler = new Vector3(UnityEngine.Random.Range(-180, 180), UnityEngine.Random.Range(-180, 180), 0);
        StartRotating(rotatingEuler);
        movingDirection = new Vector3(Mathf.RoundToInt(UnityEngine.Random.Range(-1, 1)), 1, 0);
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
            if (hasAimingTarget) //有轉向指定
            {
                rotation = (destination - transform.position).normalized;
                if (rotation == Vector3.zero)
                    rotation = Vector3.up;//此處防止rotation為0
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, Quaternion.LookRotation(rotation).z), Time.deltaTime * rotatingSpeed * 200);
            }
            else if (hasAssignedRotation)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(rotation.x, rotation.y, rotation.z), Time.deltaTime * rotatingSpeed * 200);
            }
            else
            {
                transform.Rotate(rotatingEuler * rotatingSpeed);//自轉
            }
        }

        if (isMoving)
        {
            if (hasMovingDestination)
            {
                transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * movingSpeed);


                if (transform.position == destination && !isArrival)//抵達指定位置
                {
                    isArrival = true;
                    OnArrivalDestination?.Invoke();//觸發
                }
                else if (transform.position != destination)
                {
                    isArrival = false;
                }

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
