using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public enum LiftState
{
    open,
    traveling,
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
    public string Destination;
    [SerializeField] private List<LiftButton> liftButtons;
    [SerializeField] private BoxCollider triggerCollider;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private List<GameObject> trackedPlayers;

    private Animator animator;
    [SerializeField] private LiftState liftState;
    private LiftButton activeLiftButton;
    private bool allPlayersEntered;
    private Vector3 spawnPosition;
    private Quaternion spawnRotation;
    [SerializeField] private float spawnTime;
    private float maxSpawnTime = 5f;
    //Older code
    [SerializeField] private VideoPlayer videoPlayer;
    //

    private void Awake()
    {
        animator = GetComponent<Animator>();
        liftState = LiftState.closed;
        DontDestroyOnLoad(this);
    }
    private void Update()
    {
        switch (liftState)
        {
            case LiftState.open:
                CheckForPlayers();
                if (Destination != string.Empty && allPlayersEntered)
                {
                    SwitchElevation();
                }
                break;
            case LiftState.traveling:
                SetDestinationData();
                //
                break;
            case LiftState.closed:
                break;
            default:
                break;
        }
        
    }
    //Scene switching
    private async void SwitchElevation()
    {
        animator.Play("Close");
        SceneManager.LoadScene(Destination);
        await Task.Yield();
        liftState = LiftState.traveling;
        spawnTime = maxSpawnTime;
        Destination = string.Empty;
    }
    public void OpenLift()
    {
        animator.Play("Open");
        liftState = LiftState.open;
    }
    private void SetDestinationData()
    {
        transform.position = spawnPosition;
        transform.rotation = spawnRotation;
        if (spawnTime > 0)
        {
            spawnTime -= Time.deltaTime;
        }
        else
        {
            animator.Play("Open");
            foreach (GameObject player in trackedPlayers)
            {
                player.transform.parent = null;
            }
            if (trackedPlayers.Count != 0)
            {
                trackedPlayers.Clear();
            }           
            CheckIfPlayersLeft();
        }       
    }
    private void CheckIfPlayersLeft()
    {
        Vector3 overlapPosition = transform.position + triggerCollider.center;
        Collider[] targets = Physics.OverlapBox(overlapPosition, triggerCollider.size / 2f, transform.rotation, playerLayer);
        if (targets.Length <= 0)
        {
            animator.Play("Close");
            liftState = LiftState.closed;
        }
    }
    //

    //Button functions
    public void SwitchButton(LiftButton lifButton)
    {
        if (activeLiftButton != null)
        {
            activeLiftButton.DeactiveButton();
        }
        if (liftButtons.Contains(lifButton))
        {
            activeLiftButton = lifButton;
            activeLiftButton.ActivateButton();
        }
    }
    public void SetNextElevation(SceneData sceneData)
    {
        Destination = sceneData.SceneName;
        spawnPosition = sceneData.SpawnPosition;
        spawnRotation = sceneData.SpawnRotation;
    }
    //

    //Checks if player is inside the elevator
    private void CheckForPlayers()
    {
        Vector3 overlapPosition = transform.position + triggerCollider.center;
        Collider[] targets = Physics.OverlapBox(overlapPosition, triggerCollider.size / 2f, transform.rotation, playerLayer);
        allPlayersEntered = (targets.Length == LiftManager.PlayerCount) ? true : false;

        if (targets.Length != 0)
        {
            for (int i = 0; i < targets.Length; i++)
            {
                TrackPlayer(targets[i].transform.gameObject);
            }
        }
    }
    private void TrackPlayer(GameObject targetPlayer) 
    {
        if (!trackedPlayers.Contains(targetPlayer))
        {
            trackedPlayers.Add(targetPlayer);
            targetPlayer.transform.parent = this.transform;
        }
    }
    //

    //Screen Functions
    public void PlayVideo()
    {
        videoPlayer.Play();
    }
    public void RestartVideo()
    {
        videoPlayer.Stop();
        videoPlayer.Play();
    }
    //
    

    private void OnDrawGizmos()
    {
        Vector3 overlapPosition = transform.position + triggerCollider.center;
        Gizmos.DrawWireCube(overlapPosition, triggerCollider.size);
    }
}
