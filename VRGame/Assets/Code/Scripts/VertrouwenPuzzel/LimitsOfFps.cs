using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class LimitsOfFps : MonoBehaviour
{
    private int _frameRate = 60;
    private int _refreshRate = 60;
    private void Start() => SetFps(_frameRate);
    private void SetFps(int frameRate)
    {
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = frameRate;
        Screen.SetResolution(Screen.width, Screen.height, true, _refreshRate);
        XRSettings.renderViewportScale = 1f;
        XRSettings.eyeTextureResolutionScale = .75f;
        XRSettings.gameViewRenderMode = GameViewRenderMode.RightEye;
        Debug.Log(XRSettings.eyeTextureResolutionScale);
    }
}
