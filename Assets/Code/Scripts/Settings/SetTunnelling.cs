using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTunnelling : MonoBehaviour
{
    public GameObject tunnelling;

    public void SetTypeFromIndex(int index)
    {
        if (index == 0)
        {
            tunnelling.SetActive(false);
        }
        else if (index == 1)
        {
            tunnelling.SetActive(true);
        }
    }
}


