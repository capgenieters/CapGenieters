using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCrosshair : MonoBehaviour
{
    public GameObject crosshair;

    public void SetTypeFromIndex(int index)
    {
        if (index == 0)
        {
            crosshair.SetActive(false);
        }
        else if (index == 1)
        {
            crosshair.SetActive(true);
        }
    }
}
