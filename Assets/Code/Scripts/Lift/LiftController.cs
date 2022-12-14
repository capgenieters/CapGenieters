using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public enum LiftState
{
    open,
    closed
}

public class LiftController : MonoBehaviour
{
    public LiftButton activeButton;
    [SerializeField] private VideoPlayer _videoPlayer;
    private Animator _animator;
    private LiftState _liftState;

    [SerializeField]
    private BoxCollider _triggerCollider;
    private Light _elevatorLight;
    [SerializeField]
    private bool _liftOpened;
    [SerializeField]
    private LayerMask _playerLayer;
    private bool _allPlayersEntered;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _liftState = LiftState.closed;
    }
    private void Update()
    {
        if (_liftOpened)
        {
            _allPlayersEntered = CheckForPlayers();
        }
        else
        {
            _allPlayersEntered = false;
        }
        if (_liftOpened && _allPlayersEntered)
        {
            CloseLift();
        }
    }

    private bool CheckForPlayers()
    {
        Collider[] targets = Physics.OverlapBox(_triggerCollider.transform.position, _triggerCollider.size, _triggerCollider.transform.rotation, _playerLayer);
        /*
        if (targets.Length >= CheckPlayerCount.PlayerCount)
        {
            return true;
        }
        */
        return false;
    }

    public void PlayVideo()
    {
        _videoPlayer.Play();
    }
    public void RestartVideo()
    {
        _videoPlayer.Stop();
        _videoPlayer.Play();
    }
    public void OpenLift()
    {
        if (_liftState == LiftState.open)
            return;
        _animator.Play("Open");
        _liftState = LiftState.open;
    }
    public void CloseLift()
    {
        if (_liftState == LiftState.closed)
            return;
        _animator.Play("Close");
        _liftState = LiftState.closed;
    }

    public void SwitchButton(LiftButton Button)
    {
        activeButton.DeactiveButton();
        activeButton = Button;
        activeButton.ActivateButton();
    }
}
