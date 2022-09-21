using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsLimit : MonoBehaviour
{
    [SerializeField] private int _frameRate = 60;
    private void Awake() => SetFps(_frameRate);
    private void SetFps(int frameRate) => Application.targetFrameRate = frameRate;
}
