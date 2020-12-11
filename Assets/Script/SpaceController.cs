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
            if (Input.GetKeyDown(KeyCode.F))
            {
                Debug.Log("All floating");
                spaceDirector.Floating();
            }
            else if (Input.GetKeyDown(KeyCode.H))
            {
                Debug.Log("All hold hands");
  
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                Debug.Log("All raise hand");
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                Debug.Log("Active");
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

                    spaceDirector.SayHello();
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
