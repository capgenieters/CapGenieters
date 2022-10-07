using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitsOfFps : MonoBehaviour
{
    [SerializeField] private int _frameRate = 60;
    private void Awake() => SetFps(_frameRate);
    private void SetFps(int frameRate)
    {
        QualitySettings.vSyncCount = 0;  // VSync must be disabled
        Application.targetFrameRate = frameRate;
    }
}
