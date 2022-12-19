using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreedomLeverHandler : MonoBehaviour
{
    private int LeversToPull = 2;
    private List<GameObject> LeversPulled = new List<GameObject>();

    public void HandleLeverPull(GameObject lever)
    {   
        Debug.Log("LeversPulled: " + LeversPulled);
        Debug.Log("Lever" + lever.name + " pulled");

        if (!LeversPulled.Contains(lever))
        {
            LeversPulled.Add(lever);
        }

        if (LeversPulled.Count == LeversToPull)
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
