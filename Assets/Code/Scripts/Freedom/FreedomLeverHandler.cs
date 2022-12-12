using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreedomLeverHandler : MonoBehaviour
{

    private int LeversPulled = 0;
    private int LeversToPull = 2;

    public void LeverPullDown()
    {
        Debug.Log("LeverHandler down");
        LeversPulled++;
        HandleLeverPull();
    }

    public void LeverPullUp()
    {
        Debug.Log("LeverHandler up");
        LeversPulled--;
        HandleLeverPull();
    }

    private void HandleLeverPull()
    {   
        Debug.Log("LeversPulled: " + LeversPulled);
        if (LeversPulled == LeversToPull)
        {
            Debug.Log("All levers pulled");
            gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("Not all levers pulled");
            gameObject.SetActive(false);
        }
    }
}
