/*
 * 物件活動區域。
 */
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(BoxCollider))]
public class FloatingSpace : MonoBehaviour
{
    [Header("Space Settings")]
    public float spaceZSize = 5;//空間深度
    public static float spaceWidth { get; private set; }
    public static float spaceHeight { get; private set; }


    private void Awake()
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
        transform.localScale = new Vector3(spaceWidth, spaceHeight, spaceZSize);
    }


}
