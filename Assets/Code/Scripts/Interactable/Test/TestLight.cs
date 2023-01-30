using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLight : MonoBehaviour
{
    [SerializeField] private Material materialOn;
    [SerializeField] private Material materialOff;
    [SerializeField] private Light light;
    public MeshRenderer meshRenderer;

    private void Start()
    {
        LightOff();
    }

    public void LightOn()
    {
        meshRenderer.material = materialOn;
        light.enabled = true;
    }

    public void LightOff()
    {
        meshRenderer.material = materialOff;
        light.enabled = false;
    }
}
