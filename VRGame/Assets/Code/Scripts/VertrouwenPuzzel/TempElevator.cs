using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
public class TempElevator : MonoBehaviour
{
    private static Animator _animator;
    [SerializeField]
    private BoxCollider _triggerCollider;
    private Light _elevatorLight;
    [SerializeField]
    private bool _liftOpened;
    [SerializeField]
    private LayerMask _playerLayer;
    private bool _allPlayersEntered;

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _elevatorLight = GetComponentInChildren<Light>();
        ChangeLights(false);
        _liftOpened = false;
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
            StartElevator();
        }
    }

    private bool CheckForPlayers()
    {
        Collider[] targets = Physics.OverlapBox(_triggerCollider.transform.position, _triggerCollider.size, _triggerCollider.transform.rotation, _playerLayer);
        if (targets.Length >= CheckPlayerCount.PlayerCount)
        {
            return true;
        }
        return false;
    }

    private void StartElevator()
    {
        _animator.Play("Close");
    }

    public void OpenElevator()
    {
        _animator.Play("Open");
        ChangeLights(true);
        _liftOpened = true;
    }

    private void ChangeLights(bool isActive)
    {
        _elevatorLight.enabled = isActive;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(_triggerCollider.transform.position, _triggerCollider.size * 2f);
    }
}
