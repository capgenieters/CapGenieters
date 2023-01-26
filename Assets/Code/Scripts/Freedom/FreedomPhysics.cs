using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreedomPhysics : MonoBehaviour
{
    [SerializeField] float Gravity = 2.0f;
    [Range(0.01f, 0.2f)] [SerializeField] float SurfaceAlignSpeed = 0.05f;  
    private GameObject Player;

    void Start()
    {
        Player = this.gameObject;
    }

    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, -transform.up, out hit))
        {
            Player.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation, SurfaceAlignSpeed);

            // Gravity 
            if (hit.distance > 1.1f)
            {
                Player.transform.position -= transform.up * Gravity * Time.deltaTime;
            }
        }
    }
}
