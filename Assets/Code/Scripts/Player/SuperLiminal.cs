using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SuperLiminal : MonoBehaviour
{
    [Header("Components")]
    public Transform target;            

    [Header("Parameters")]
    public LayerMask targetMask;        
    public LayerMask ignoreTargetMask;  
    public float offsetFactor;          

    float originalDistance;             
    float originalScale;                
    Vector3 targetScale;                

    public InputActionProperty showButton;

    void Update()
    {
        HandleInput();
        ResizeTarget();
    }

    void HandleInput()
    {
        if (showButton.action.WasPressedThisFrame())
        {
            if (target == null)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, targetMask))
                {
                    target = hit.transform;
                    target.GetComponent<Rigidbody>().isKinematic = true;
                    originalDistance = Vector3.Distance(transform.position, target.position);
                    originalScale = target.localScale.x;
                    targetScale = target.localScale;
                }
            }
            else
            {
                target.GetComponent<Rigidbody>().isKinematic = false;
                target = null;
            }
        }
    }

    void ResizeTarget()
    {
        if (target == null)
        {
            return;
        }
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, ignoreTargetMask))
        {
            target.position = hit.point - transform.forward * offsetFactor * targetScale.x;
            float currentDistance = Vector3.Distance(transform.position, target.position);
            float s = currentDistance / originalDistance;
            targetScale.x = targetScale.y = targetScale.z = s;
            target.localScale = targetScale * originalScale;
        }
    }
}
