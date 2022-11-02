using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR;
using UnityEngine;

public class HMDInfoManager : MonoBehaviour
{

    void Start()
    {
        Debug.Log("Is Device Active " + XRSettings.isDeviceActive);
        Debug.Log("Device Name is : " + XRSettings.loadedDeviceName);

        if(!XRSettings.isDeviceActive)
        {
            Debug.Log("No Headset Plugged");
        }
        else if(XRSettings.isDeviceActive && (XRSettings.loadedDeviceName == "Mock HMD" || XRSettings.loadedDeviceName == "MockHMDDisplay"))
        {
            Debug.Log("Using Mock HMD");
        }
        else
        {
            Debug.Log("We Have a Headset " + XRSettings.loadedDeviceName);
        }
    }


    void Update()
    {
        
    }
}
