/*
 * 物件活動區域。
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class FloatingSpace : MonoBehaviour
{
    [Header("Space Settings")]
    public float spaceZSize = 5;//空間深度
    public static float spaceWidth { get; private set; }
    public static float spaceHeight { get; private set; }
    private float _spaceWidth = 30f;
    private float _spaceHeight = 30f;

    private void Start()
    {
        CreateFloatingSpace();//產生活動空間
    }

    /// <summary>
    /// 創建漂浮空間。空間將與螢幕大小同長寬。
    /// </summary>
    private void CreateFloatingSpace()
    {
        spaceWidth = Camera.main.orthographicSize * 2.0f * Screen.width / Screen.height;
        spaceHeight = Camera.main.orthographicSize * 2.0f;
        //縮放長寬至與相機同大小
        StartCoroutine(FloatingSpaceAdjust());
    }

    /// <summary>
    /// 空間調整，逐漸內縮。防止因席位超出指定空間導致無法完全包覆。
    /// </summary>
    /// <returns></returns>
    IEnumerator FloatingSpaceAdjust()
    {
        yield return new WaitForSeconds(0.05f);

        if (spaceWidth == _spaceWidth && spaceHeight == _spaceHeight)//達到指定大小，停止內縮
        {
            StopCoroutine(FloatingSpaceAdjust());
        }
        else
        {
            //Width
            if (_spaceWidth > spaceWidth)
            {
                _spaceWidth--;
            }
            else
            {
                _spaceWidth = spaceWidth;
            }

            //Height
            if (_spaceHeight > spaceHeight)
            {
                _spaceHeight--;
            }
            else
            {
                _spaceHeight = spaceHeight;
            }
        }

        transform.localScale = new Vector3(_spaceWidth, _spaceHeight, spaceZSize);
        yield return StartCoroutine(FloatingSpaceAdjust());
    }
}
