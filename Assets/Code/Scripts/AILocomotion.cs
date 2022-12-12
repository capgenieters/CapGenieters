using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AILocomotion : MonoBehaviour
{
    public Destinations[] destinations;

    [System.Serializable]
    public class Destinations
    {
        public Transform destination;
    }

    NavMeshAgent agent;
    Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        CreatePath();
    }

    public void CreatePath()
    {
        animator.SetFloat("Speed", agent.velocity.magnitude);

        if (!agent.hasPath)
        {
            int r = Random.Range(0, destinations.Length);
            agent.destination = destinations[r].destination.position;
        }
    }
}
