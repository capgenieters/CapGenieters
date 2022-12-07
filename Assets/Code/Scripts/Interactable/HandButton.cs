using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class HandButton : XRBaseInteractable
{
    [Header("Audio clips")]
    [SerializeField] private AudioClip pressedAudio;
    [SerializeField] private AudioClip releasedAudio;
    private AudioSource audioSource;

    [Header("Events")]
    public UnityEvent OnActivate = null;
    public UnityEvent OnDeactive = null;

    [SerializeField] private GameObject button;

    private XRBaseInteractor baseInteractor = null;

    private bool buttonActivated;
    private float yMin = 0.0f;
    private float yMax = 0.0f;
    private bool previousPress = false;
    private float previousHeight = 0.0f;
    
    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
        onHoverEntered.AddListener(StartPress);
        onHoverExited.AddListener(EndPress);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        onHoverEntered.RemoveListener(StartPress);
        onHoverExited.RemoveListener(EndPress);
    }

    private void Start()
    {
        SetMinMax();
        buttonActivated = false;
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);
        if (baseInteractor)
        {
            float newHandHeight = GetLocalYPosition(baseInteractor.transform.position);
            float handDifference = previousHeight - newHandHeight;
            previousHeight = newHandHeight;

            float newPosition = button.transform.localPosition.y - handDifference;
            SetYPosition(newPosition);

            CheckPress();
        }
    }

    private void StartPress(XRBaseInteractor interactor)
    {
        baseInteractor = interactor;
        previousHeight = GetLocalYPosition(baseInteractor.transform.position);
    }

    private void EndPress(XRBaseInteractor interactor)
    {
        baseInteractor = null;
        previousHeight = 0.0f;

        previousPress = false;
        SetYPosition(yMax);
        PlayAudio(releasedAudio);
    }

    private void SetMinMax()
    {
        Collider collider = button.GetComponent<Collider>();
        yMin = button.transform.localPosition.y - (collider.bounds.size.y / 4f);
        yMax = button.transform.localPosition.y;
    }

    private float GetLocalYPosition(Vector3 position)
    {
        Vector3 localPosition = button.transform.root.InverseTransformPoint(position);

        return localPosition.y;
    }

    private void SetYPosition(float position)
    {
        Vector3 newPosition = button.transform.localPosition;
        newPosition.y = Mathf.Clamp(position, yMin, yMax);
        button.transform.localPosition = newPosition;
    }

    private void CheckPress()
    {
        bool inPosition = InPosition();

        if (inPosition && inPosition != previousPress)
        {
            UseButton();
            PlayAudio(pressedAudio);
        }         
        previousPress = inPosition;
    }

    private void UseButton()
    {
        if (buttonActivated)
        {
            OnActivate.Invoke();
        }
        else
        {
            OnDeactive.Invoke();
        }
        buttonActivated = !buttonActivated;
    }

    private bool InPosition()
    {
        float inRange = Mathf.Clamp(button.transform.localPosition.y, yMin, yMin + 0.01f);
        return button.transform.localPosition.y == inRange;
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
