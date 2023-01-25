using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyed : MonoBehaviour
{
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "Hammer")
        {
            foreach (Rigidbody glassPart in transform.GetComponentsInChildren<Rigidbody>())
            {
                glassPart.isKinematic = false;
            }
        }
    }
}
