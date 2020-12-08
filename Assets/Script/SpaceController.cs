/*
 * 使用者操作列表。
 */
using UnityEngine;

public class SpaceController : MonoBehaviour
{
    [SerializeField] SpaceDirector spaceDirector = null;
    void Update()
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

        //指定角色向前
        if (Input.GetKey(KeyCode.DownArrow))
        {

            if (Input.GetKeyDown(KeyCode.A))
            {
                Debug.Log("Seat 0");
                spaceDirector.SayHello();
            }
            else if (Input.GetKeyDown(KeyCode.B))
            {
                Debug.Log("Seat 1");
            }
            else if (Input.GetKeyDown(KeyCode.C))
            {
                Debug.Log("Seat 2");
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                Debug.Log("Seat 3");
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Seat 4");
            }
            else if (Input.GetKeyDown(KeyCode.F))
            {
                Debug.Log("Seat 5");
            }
            else if (Input.GetKeyDown(KeyCode.G))
            {
                Debug.Log("Seat 6");
            }
            else if (Input.GetKeyDown(KeyCode.H))
            {
                Debug.Log("Seat 7");
            }
            else if (Input.GetKeyDown(KeyCode.I))
            {
                Debug.Log("Seat 8");
            }
            else if (Input.GetKeyDown(KeyCode.J))
            {
                Debug.Log("Seat 9");
            }
            else if (Input.GetKeyDown(KeyCode.K))
            {
                Debug.Log("Seat 10");
            }
            else if (Input.GetKeyDown(KeyCode.L))
            {
                Debug.Log("Seat 11");
            }
            else if (Input.GetKeyDown(KeyCode.M))
            {
                Debug.Log("Seat 12");
            }
            else if (Input.GetKeyDown(KeyCode.N))
            {
                Debug.Log("Seat 13");
            }
            else if (Input.GetKeyDown(KeyCode.O))
            {
                Debug.Log("Seat 14");
            }
            else if (Input.GetKeyDown(KeyCode.P))
            {
                Debug.Log("Seat 15");
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                Debug.Log("Seat 16");
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                Debug.Log("Seat 17");
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                Debug.Log("Seat 18");
            }
            else if (Input.GetKeyDown(KeyCode.T))
            {
                Debug.Log("Seat 19");
            }
            else if (Input.GetKeyDown(KeyCode.U))
            {
                Debug.Log("Seat 20");
            }
            else if (Input.GetKeyDown(KeyCode.V))
            {
                Debug.Log("Seat 21");
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                Debug.Log("Seat 22");
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                Debug.Log("Seat 23");
            }
            else if (Input.GetKeyDown(KeyCode.Y))
            {
                Debug.Log("Seat 24");
            }
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                Debug.Log("Seat 25");
            }
        }


        #endregion
    }
}
