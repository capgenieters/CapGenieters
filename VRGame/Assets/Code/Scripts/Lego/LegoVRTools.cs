using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegoVRTools
{
    // Prevent adding more garbage every frame
    List<UnityEngine.XR.InputDevice> rightHandDevices, leftHandDevices;
    UnityEngine.XR.InputDevice rightController, leftController;
    bool rightGripPrevious, leftGripPrevious;

    public bool[] GetGripStates(bool detectPressOnly = true)
    {
        bool rightGripPressed = false, leftGripPressed = false;

        // Get the right and left hand devices
        rightHandDevices = new List<UnityEngine.XR.InputDevice>();
        leftHandDevices = new List<UnityEngine.XR.InputDevice>();

        UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.RightHand, rightHandDevices);
        UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.LeftHand, leftHandDevices);

        // If a device is found, get it's gripButton state and set it
        if(rightHandDevices.Count > 0)
        {
            rightController = rightHandDevices[0];
            rightController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton, out rightGripPressed);
        }

        if(leftHandDevices.Count > 0)
        {
            leftController = leftHandDevices[0];
            leftController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton, out leftGripPressed);
        }

        bool[] result;

        if (detectPressOnly)
            result = new bool[2] { leftGripPressed && !leftGripPrevious, rightGripPressed && !rightGripPrevious };
        else
            result = new bool[2] { leftGripPressed, rightGripPressed };

        rightGripPrevious = rightGripPressed;
        leftGripPrevious = leftGripPressed;

        return result;
    }
}