using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ActivateGrabRay : MonoBehaviour
{
    public GameObject leftGrabRay;
    public GameObject rightGrabRay;

    public XRDirectInteractor leftDirectGrab;
    public XRDirectInteractor rightDirectGrab;

    void Update()
    {
        leftGrabRay.SetActive(leftDirectGrab.interactablesSelected.Count == 0);
        rightGrabRay.SetActive(rightDirectGrab.interactablesSelected.Count == 0);
    }

    public void SetTypeFromIndex(int index)
    {
        if (index == 0)
        {
            leftGrabRay.SetActive(false);
            rightGrabRay.SetActive(false);
        }
        else if (index == 1)
        {
            leftGrabRay.SetActive(true);
            rightGrabRay.SetActive(true);
        }
    }
}
