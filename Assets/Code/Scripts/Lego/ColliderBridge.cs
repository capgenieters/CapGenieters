// Note by devs:
// This class has been based on the solution by rsodre on the Unity forms.
// You can view the original question and solution here:
// http://answers.unity.com/answers/1321124/view.html

using UnityEngine;

public class ColliderBridge : MonoBehaviour
{
    // TODO: Add collider to prefab
    public delegate void CollisionFunction(Collider collider, Transform transform);
    public CollisionFunction triggerEnterFunction;

    private void OnTriggerEnter(Collider collider)
    {
        triggerEnterFunction(collider, transform);
    }
}