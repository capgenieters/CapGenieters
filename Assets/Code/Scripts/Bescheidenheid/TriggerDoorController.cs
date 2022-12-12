using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDoorController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [SerializeField] private string doorClose = "DoorsClose";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.Play(doorClose, 0, 0.0f);
            this.transform.parent.gameObject.SetActive(false);
        }
    }

}
