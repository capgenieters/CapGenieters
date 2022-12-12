using System.Collections.Generic;
using UnityEngine;

public class LegoInteraction
{
    // Prevent adding more garbage every frame
    public List<GameObject> breakableObjects = new List<GameObject>();
    public Vector3[] positionHistory = new Vector3[3];
    private Material droppedMat;
    private LegoTools tools;

    public LegoInteraction(LegoTools _tools)
    {
        this.tools = _tools;
        droppedMat = tools.CreateMaterial();
        droppedMat.color = new Color(0, 0.57f, 0.28f);
    }
    
    /// <summary>
    /// This function will update the controller positions from the last 3 times it ran in an array. This array is used to check wether a 'swinging' motion was made to break objects. All this will actually do is push back the entity array by 1 and then add the new position to the front of it.
    /// </summary>
    public void UpdateController(Vector3 newPosition)
    {
        // Create a temporary list of position to apply later on
        Vector3[] newHistory = new Vector3[3];

        for (int i = 0; i < positionHistory.Length; i++)
        {
            if (positionHistory[i] == null)
                break;

            if (i != positionHistory.Length - 1)
                newHistory[i + 1] = positionHistory[i];
            else
                continue;
        }   
        
        newHistory[0] = newPosition;
        positionHistory = newHistory;
    }

    public void AddObject(GameObject obj)
    {
        ColliderBridge bridge = obj.AddComponent<ColliderBridge>();
        bridge.triggerEnterFunction = OnTriggerEnter;
        breakableObjects.Add(obj);
    }

    public void OnTriggerEnter(Collider collider, Transform transform)
    {
        // Make it so this will run in the collide script only once
        if (positionHistory.Length == 3)
        {
            float distance = Vector3.Distance(positionHistory[2],  positionHistory[1]);
            distance += Vector3.Distance(positionHistory[1],  positionHistory[0]);

            bool didSlash = distance > 0.1f;

            if (didSlash)
            {
                BreakObject(transform);
            }
        }
    }

    private void BreakObject(Transform transform)
    {
        ref List<Brick> pool = ref tools.droppedBricksPool;
        Vector3 scale = transform.localScale;
        Vector3 dropPoint = transform.position;
        
        float dropAmount = (scale.x / tools.worldScale + scale.y / tools.worldScale + scale.z / tools.worldScale) / 30;
        int poolIndex = 0;

        for (int i = 0; i < (int)dropAmount; i++)
        {
            if (poolIndex < pool.Count)
            {
                // Move the exising cloud block to the new position
                Brick b = pool[poolIndex];
                b.SetAbsolutePosition(dropPoint + new Vector3(0, i * 0.1f, 0));
                b.SetActive(true);
                poolIndex++;
            }
            else
            {
                // Create a new cloud block, set the position and move it into the cloud pool
                float height = Random.Range(0, 3) == 0 ? 0.2f : 0.6f;
                Brick b = new Brick(tools, 1, 1, droppedMat, height, false, true);
                b.SetAbsolutePosition(dropPoint);
                pool.Add(b);
            }
        }

        Object.Destroy(transform.gameObject);
    }
}