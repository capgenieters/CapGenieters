using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftButton : MonoBehaviour
{
    [SerializeField] private GameObject buttonBase;
    [SerializeField] private Material baseMat;
    [SerializeField] private Material lightMat;
    [SerializeField] private Light liftLight;
    
    private MeshRenderer ligthRenderer;
    [SerializeField] private LiftController liftController;
    [SerializeField] private LiftButton liftButton;
    private void Awake()
    {
        ligthRenderer = buttonBase.GetComponent<MeshRenderer>();
        ligthRenderer.material = baseMat;
    }

    public void PressButton()
    {
        liftController.SwitchButton(liftButton);
    }

    public void ActivateButton()
    {
        ligthRenderer.material = lightMat;
        liftLight.enabled = true;
    }

    public void DeactiveButton()
    {
        ligthRenderer.material = baseMat;
        liftLight.enabled = false;
    }
}
