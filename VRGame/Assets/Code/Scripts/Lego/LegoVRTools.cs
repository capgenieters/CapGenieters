using System.Collections.Generic;
using UnityEngine.XR;

public class LegoVRTools
{
    // Prevent adding more garbage every frame
    List<InputDevice> rightHandDevices, leftHandDevices;
    InputDevice rightController, leftController;

    bool rightButtonPrevious, leftButtonPrevious;

    public bool[] GetButtonStates(InputFeatureUsage<bool> inputFeature, bool detectPressOnly = true)
    {
        bool rightButtonPressed = false, leftButtonPressed = false;

        // Get the right and left hand devices
        rightHandDevices = new List<InputDevice>();
        leftHandDevices = new List<InputDevice>();

        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, rightHandDevices);
        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, leftHandDevices);

        // If a device is found, get it's gripButton state and set it
        if(rightHandDevices.Count > 0)
        {
            rightController = rightHandDevices[0];
            rightController.TryGetFeatureValue(inputFeature, out rightButtonPressed);
        }

        if(leftHandDevices.Count > 0)
        {
            leftController = leftHandDevices[0];
            leftController.TryGetFeatureValue(inputFeature, out leftButtonPressed);
        }

        bool[] result;

        if (detectPressOnly)
            result = new bool[2] { leftButtonPressed && !leftButtonPrevious, rightButtonPressed && !rightButtonPrevious };
        else
            result = new bool[2] { leftButtonPressed, rightButtonPressed };

        rightButtonPrevious = rightButtonPressed;
        leftButtonPrevious = leftButtonPressed;

        return result;
    }
}