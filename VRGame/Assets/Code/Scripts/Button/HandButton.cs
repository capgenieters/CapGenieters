using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class HandButton : XRBaseInteractable
{
    public UnityEvent OnPress = null;

    private float _yMin = 0f;
    private float _yMax = 0f;
    private bool _previousPress = false;

    private float _previousHandHeight = 0f;
    private XRBaseInteractor _hoverInteractor = null;
    private Light _light;

    private void Start()
    {
        SetMinMax();
        _light = GetComponentInChildren<Light>();
        _light.enabled = false;
    }

    protected override void Awake()
    {
        base.Awake();
        onHoverEntered.AddListener(StartPress);
        onHoverExited.AddListener(EndPress);
    }

    private void OnDestroy()
    {
        onHoverEntered.RemoveListener(StartPress);
        onHoverExited.RemoveListener(EndPress);
    }

    private void StartPress(XRBaseInteractor interactor)
    {
        _hoverInteractor = interactor;
        _previousHandHeight = GetLocalYPosition(_hoverInteractor.transform.position);
        _light.enabled = true;
    }

    private void EndPress(XRBaseInteractor interactor)
    {
        _hoverInteractor = null;
        _previousHandHeight = 0f;

        _previousPress = false;
        SetYPosition(_yMax);
        _light.enabled = false;
    }
    //Button motion physics
    private void SetMinMax()
    {
        Collider collider = GetComponent<Collider>();
        _yMin = transform.localPosition.y - (collider.bounds.size.y / 4f);
        _yMax = transform.localPosition.y;
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        if(_hoverInteractor)
        {
            float newHandHeight = GetLocalYPosition(_hoverInteractor.transform.position);
            float handDifference = _previousHandHeight - newHandHeight;
            _previousHandHeight = newHandHeight;

            float newPosition = transform.localPosition.y - handDifference;
            SetYPosition(newPosition);

            CheckPress();
        }
    }

    private float GetLocalYPosition(Vector3 position)
    {
        Vector3 localPosition = transform.root.InverseTransformPoint(position);
        return localPosition.y;
    }
    private void SetYPosition(float position)
    {
        Vector3 newPosition = transform.localPosition;
        newPosition.y = Mathf.Clamp(position, _yMin, _yMax);
        transform.localPosition = newPosition;
    }
    private void CheckPress()
    {
        bool inPosition = InPosition();
        if (inPosition && inPosition != _previousPress)
        {
            OnPress.Invoke();
        }
        _previousPress = inPosition;
    }
    private bool InPosition()
    {
        float inRange = Mathf.Clamp(transform.localPosition.y, _yMin, _yMin + 0.01f);
        return transform.localPosition.y == inRange;
    }
    //
}
