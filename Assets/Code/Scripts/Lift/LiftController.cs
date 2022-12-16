using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public enum LiftState
{
    open,
    closed
}

public class TrackedPlayer 
{
    public string name;
    public Vector2 position;
    public Quaternion rotation;
}

public class LiftController : MonoBehaviour
{
    public string currentElevation;
    public string nextElevation;
    
    private Animator animator;
    private LiftState liftState;


    public LiftButton activeButton;
    [SerializeField] private VideoPlayer videoPlayer;
    
    [SerializeField]
    private BoxCollider triggerCollider;
    private Light elevatorLight;
    [SerializeField]
    private bool liftOpened;
    [SerializeField]
    private LayerMask playerLayer;
    private bool allPlayersEntered;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        liftState = LiftState.closed;
    }
    private void Update()
    {
        if (liftOpened)
        {
            CheckForPlayers();
        }
        else
        {
            allPlayersEntered = false;
        }
        if (liftOpened && allPlayersEntered)
        {
            CloseLift();
        }
    }

    public void SwitchElevation()
    { }

    public void CheckForPlayers()
    {
        Vector3 overlapPosition = transform.position + triggerCollider.center;
        Collider[] targets = Physics.OverlapBox(overlapPosition, triggerCollider.size / 2f, transform.rotation, playerLayer);
        //Debug.Log(targets.Length);
        /*if (targets.Length != 0)
        {
            for (int i = 0; i < targets.Length; i++)
            {
                TrackPlayers(targets[i].gameObject);
            }
        }*/
    }

    private void ParentPlayer()
    {

    }

    private void TrackPlayers(GameObject currentPlayer) 
    {
        Vector3 playerPosition = new Vector3(currentPlayer.transform.position.x, 0, currentPlayer.transform.position.z) + transform.position;
        Quaternion playerRotation = currentPlayer.transform.rotation;
        //Debug.Log(playerPosition + ", " + playerRotation);
        //currentPlayer.transform.position = playerPosition;
    }

    public void PlayVideo()
    {
        videoPlayer.Play();
    }
    public void RestartVideo()
    {
        videoPlayer.Stop();
        videoPlayer.Play();
    }
    public void OpenLift()
    {
        if (liftState == LiftState.open)
            return;
        animator.Play("Open");
        liftState = LiftState.open;
    }
    public void CloseLift()
    {
        if (liftState == LiftState.closed)
            return;
        animator.Play("Close");
        liftState = LiftState.closed;
    }

    public void SwitchButton(LiftButton Button)
    {
        activeButton.DeactiveButton();
        activeButton = Button;
        activeButton.ActivateButton();
    }

    private void OnDrawGizmos()
    {
        Vector3 overlapPosition = transform.position + triggerCollider.center;
        Gizmos.DrawWireCube(overlapPosition, triggerCollider.size);
    }
}
