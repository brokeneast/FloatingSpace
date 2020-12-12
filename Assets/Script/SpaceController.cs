/*
 * 使用者操作列表。
 */
using UnityEngine;
using System.Collections;
public class SpaceController : MonoBehaviour
{
    [SerializeField] SpaceDirector spaceDirector = null;
 
    void OnGUI()
    {

        #region All
        //全部角色聽口令
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            //基本
            if (Input.GetKeyDown(KeyCode.F))//重設漂浮
            {
                spaceDirector.Floating();
            }
            else if (Input.GetKeyDown(KeyCode.P))//全部暫停
            {
                spaceDirector.Stop();
            }
            else if (Input.GetKeyDown(KeyCode.R))//全部恢復
            {
                spaceDirector.Resume();
            }

            //特殊
            else if (Input.GetKeyDown(KeyCode.T))//回歸集合
            {
                spaceDirector.BackToSeat();
            }
            else if (Input.GetKeyDown(KeyCode.H))//握手
            {
  
            }
            else if (Input.GetKeyDown(KeyCode.U))//舉手
            {
   
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
   
            }
        }
        #endregion

        #region Assign
        //當前指定角色退回
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Debug.Log("Back");
            spaceDirector.Back();
        }

        //指定角色出場
        if (Input.GetKey(KeyCode.DownArrow))
        {
            foreach (char c in Input.inputString)
            {
                int index = SeatIndexHelper.SeatKeyCodeToIndex(c);
                if (index != -1)
                {
                    spaceDirector.PopOut(index);
                }
                else
                {
                    Debug.Log("超出範圍");
                }
            }
        }


        #endregion
    }
}
