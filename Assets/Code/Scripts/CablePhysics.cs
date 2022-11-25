using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CablePhysics : MonoBehaviour
{
    private void Start()
    {
        Transform currentTransform = transform;
        Rigidbody previousBody = null;

        while (currentTransform.childCount > 0)
        {
            currentTransform = currentTransform.GetChild(0);
            currentTransform.gameObject.AddComponent<Rigidbody>();
            currentTransform.gameObject.AddComponent<CapsuleCollider>();
            currentTransform.gameObject.AddComponent<HingeJoint>();
            HingeJoint joint = currentTransform.GetComponent<HingeJoint>();
            CapsuleCollider collider = currentTransform.GetComponent<CapsuleCollider>();
            collider.radius = 0.001f;
            collider.height = currentTransform.localPosition.y / 2;

            if (previousBody != null)
                joint.connectedBody = previousBody;

            previousBody = currentTransform.GetComponent<Rigidbody>();
        }
    }
}
