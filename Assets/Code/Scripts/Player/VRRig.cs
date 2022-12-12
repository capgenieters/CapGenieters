using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VRMap
{
    public Transform vrTarget;
    public Transform rigTarget;
    public Vector3 trackingPositionOffsest;
    public Vector3 trackingRotationOffsest;

    public void Map()
    {
        rigTarget.position = vrTarget.TransformPoint(trackingPositionOffsest);
        rigTarget.rotation = vrTarget.rotation * Quaternion.Euler(trackingRotationOffsest);
    }
}

public class VRRig : MonoBehaviour
{
    public VRMap head;
    public VRMap leftHand;
    public VRMap rightHand;

    public Transform headConstraint;
    public Vector3 headBodyOffsest;
    public float turnSmoothness;

    void Start()
    {
        headBodyOffsest = transform.position - headConstraint.position;
    }

    void LateUpdate()
    {
        transform.position = headConstraint.position + headBodyOffsest;
        transform.forward = Vector3.Lerp(transform.forward, Vector3.ProjectOnPlane(headConstraint.up, Vector3.up).normalized, Time.deltaTime * turnSmoothness);

        head.Map();
        leftHand.Map();
        rightHand.Map();
    }
}
