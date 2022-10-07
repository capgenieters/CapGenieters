using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertrouwenButtonAnimationEvent : MonoBehaviour
{
    [SerializeField]
    private List<Collider> _colliders;

    public void EnableBoxes()
    {
        foreach (Collider collider in _colliders)
        {
            collider.enabled = true;
        }
    }

    public void DisableBoxes()
    {
        foreach (Collider collider in _colliders)
        {
            collider.enabled = false;
        }
    }
}
