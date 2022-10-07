using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCube : MonoBehaviour
{
  private Rigidbody rb;
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*
     
        */
    }

    
}
