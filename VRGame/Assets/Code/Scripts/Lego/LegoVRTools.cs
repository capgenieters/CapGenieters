using System.Collections.Generic;
using UnityEngine.XR;

public class LegoVRTools
{
    // Prevent adding more garbage every frame
    List<InputDevice> rightHandDevices, leftHandDevices;
    Dictionary<InputFeatureUsage<bool>, bool[]> previous = new Dictionary<InputFeatureUsage<bool>, bool[]>();
    InputDevice rightController, leftController;

    /// <summary>
    /// Use this function at the beginning of the game to get the controllers, or run it every once in a while to make sure the player's controllers are still found. 
    /// </summary>
    public void UpdateControllers()
    {
        // Get the right and left hand devices
        rightHandDevices = new List<InputDevice>();
        leftHandDevices = new List<InputDevice>();

        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, rightHandDevices);
        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, leftHandDevices);
    }

    /// <summary>
    /// Use this function to check wether a certain controller button is pressed or held. The first parameter is the input feature (the button) and the second parameter is wether to detect a press only, or if the button is held.
    /// </summary>
    public bool[] GetButtonStates(InputFeatureUsage<bool> inputFeature, bool detectPressOnly = true)
    {
        bool rightButtonPressed = false, leftButtonPressed = false;

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

        bool[] result, cPrevious;
        if (previous.ContainsKey(inputFeature))
            cPrevious = previous[inputFeature];
        else
            cPrevious = new bool[2];

        if (detectPressOnly)
            result = new bool[2] { leftButtonPressed && !cPrevious[0], rightButtonPressed && !cPrevious[1] };
        else
            result = new bool[2] { leftButtonPressed, rightButtonPressed };

        previous[inputFeature] = new bool[2] { leftButtonPressed, rightButtonPressed };

        return result;
    }
}