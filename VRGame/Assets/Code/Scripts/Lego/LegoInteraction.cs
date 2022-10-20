using System.Collections.Generic;
using UnityEngine;

public class LegoInteraction
{
    // Prevent adding more garbage every frame
    public List<GameObject> breakableObjects = new List<GameObject>();
    public Vector3[] position = new Vector3[5];
    LegoTools tools;

    public LegoInteraction(LegoTools _tools)
    {
        this.tools = _tools;
    }
    
    /// <summary>
    /// This function will update the controller positions from the last 5 times it ran in an array. This array is used to check wether a 'swinging' motion was made to break objects.
    /// </summary>
    public void UpdateController(GameObject controller)
    {
        
    }

    public void AddObject(GameObject obj)
    {
        ColliderBridge bridge = obj.AddComponent<ColliderBridge>();
        bridge.collisionFunction = OnCollisionEnter;

        breakableObjects.Add(obj);
    }

    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.transform.name);
    }
}