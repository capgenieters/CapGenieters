using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempButtonRayCast : MonoBehaviour
{
    [SerializeField]
    private LayerMask _interactableMask;
    [SerializeField]
    private float _rayRange;
    [SerializeField]
    private GameObject _highlightedTarget;

    // Update is called once per frame
    void Update()
    {
        ShootRay();
    }

    private void ShootRay()
    {
        Debug.DrawRay(transform.position, transform.forward, Color.green);
        RaycastHit hit;
        bool hitFound = Physics.Raycast(transform.position, transform.forward, out hit, _rayRange, _interactableMask);
        _highlightedTarget = (hitFound) ? ActivateOutline(hit.transform.gameObject) : DeactiveOutline();
    }
    private GameObject ActivateOutline(GameObject target)
    {
        target.GetComponent<Outline>().enabled = true;
        return target;
    }
    private GameObject DeactiveOutline()
    {
        if (_highlightedTarget != null)
        {
            _highlightedTarget.GetComponent<Outline>().enabled = false;
        }       
        return null;
    }
}
