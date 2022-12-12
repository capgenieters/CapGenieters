using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    public GameObject destroyedVariant;

    void OnCollisionEnter(Collision col)
    {
        Instantiate(destroyedVariant, transform.position, transform.rotation);
        Destroy(gameObject);
        Debug.Log("awiudn");
    }
}
