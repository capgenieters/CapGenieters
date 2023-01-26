using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class Lever : XRGrabInteractable
{
    [Header("Audio clips")]
    [SerializeField] private AudioClip upAudio;
    [SerializeField] private AudioClip downAudio;
    private AudioSource audioSource;

    [Header("Events")]
    public UnityEvent OnPullUp = null;
    public UnityEvent OnPullDown = null;

    [SerializeField] private GameObject Handle;
    [SerializeField] private GameObject RightHandModel;
    [SerializeField] private GameObject LeftHandModel;

    private XRBaseInteractor baseInteractor = null;

    private HingeJoint joint;
    private float limitOffset = 5f;
    private bool functionFired;

    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
        joint = GetComponent<HingeJoint>();
        onSelectEntered.AddListener(StartGrab);
        onSelectExited.AddListener(EndGrab);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        onSelectEntered.RemoveListener(StartGrab);
        onSelectExited.RemoveListener(EndGrab);
    }

    private void Start()
    {
        functionFired = false;
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);
        if (baseInteractor)
        {
            bool limitReached = joint.angle >= (joint.limits.max - limitOffset);
            if (limitReached)
            {
                if (functionFired)
                {
                    return;
                }
                if (Handle.transform.rotation.x > 0 && !functionFired)
                {
                    OnPullUp.Invoke();
                    PlayAudio(upAudio);
                }
                if (Handle.transform.rotation.x < 0 && !functionFired)
                {
                    OnPullDown.Invoke();
                    PlayAudio(downAudio);
                }
                functionFired = true;
            }
            else
            {
                functionFired = false;
            }
        }
    }

    private void StartGrab(XRBaseInteractor interactor)
    {
        baseInteractor = interactor;
        HandleVisibilityState(true);
    }

    private void EndGrab(XRBaseInteractor interactor)
    {
        baseInteractor = interactor;
        HandleVisibilityState(false);
    }

    private void HandleVisibilityState(bool visibilityState)
    {
        GameObject currentHand = (baseInteractor.name == "Left Hand") ? LeftHandModel : RightHandModel;
        currentHand.SetActive(visibilityState);
        if (visibilityState)
        {
            HideHand(baseInteractor.transform.gameObject);
        }
        else
        {
            ShowHand(baseInteractor.transform.gameObject);
        }
    }

    private void ShowHand(GameObject hand)
    {
        SkinnedMeshRenderer meshRenderer = hand.GetComponentInChildren<SkinnedMeshRenderer>();
        meshRenderer.enabled = true;
    }
    private void HideHand(GameObject hand)
    {
        SkinnedMeshRenderer meshRenderer = hand.GetComponentInChildren<SkinnedMeshRenderer>();
        meshRenderer.enabled = false;
    }

    private void PlayAudio(AudioClip audio)
    {
        if (audio != null && audioSource != null)
        {
            audioSource.clip = audio;
            audioSource.Play();
        }
    }
}
