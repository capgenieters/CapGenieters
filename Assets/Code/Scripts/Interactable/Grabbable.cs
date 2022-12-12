using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit;

public class Grabbable : MonoBehaviour
{
    private bool updateInteractible;
    protected bool functionFired;
    //All hand functions
    /*private XRGrabInteractable grabInteractable => GetComponent<XRGrabInteractable>();
    private XRBaseInteractor interactor;
    [SerializeField] private XRDirectInteractor directInteractor;
    private Outline outline;
    //
    [SerializeField] private GameObject RightHandModel;
    [SerializeField] private GameObject LeftHandModel;*/

    protected virtual void Start()
    {
        /*updateInteractible = false;
        functionFired = false;
        
        outline = GetComponent<Outline>();
        outline.enabled = false;
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(HoverEnter);
            grabInteractable.selectExited.AddListener(HoverExit);
        }  */
    }

    private void Update() 
    {
        if (updateInteractible)
        {
            UpdateInteractible();
        }
    }
    protected virtual void UpdateInteractible() {}

    /*private void HoverEnter(SelectEnterEventArgs arg0)
    {
        interactor = GetComponent<XRGrabInteractable>().selectingInteractor;
        directInteractor = interactor.GetComponent<XRDirectInteractor>();
        directInteractor.hideControllerOnSelect = true;
        outline.enabled = true;
        updateInteractible = true;
        HandleVisibilityState(true);
        throw new NotImplementedException();
    }
    private void HoverExit(SelectExitEventArgs arg0)
    {
        if (directInteractor != null)
        {
            directInteractor.hideControllerOnSelect = false;
        }
        outline.enabled = false;
        updateInteractible = false;
        HandleVisibilityState(false);
        throw new NotImplementedException();
    }
    private void HandleVisibilityState(bool visibilityState)
    {
        Debug.Log(directInteractor);
        if (interactor.name == "Left Hand")
        {
            LeftHandModel.SetActive(visibilityState);    
        }
        else
        {
            RightHandModel.SetActive(visibilityState);
        }
    }*/
}
