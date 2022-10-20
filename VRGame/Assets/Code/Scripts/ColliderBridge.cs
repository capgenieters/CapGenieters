// Note by devs:
// This class has been based on the solution by rsodre on the Unity forms.
// You can view the original question and solution here:
// http://answers.unity.com/answers/1321124/view.html

using UnityEngine;

public class ColliderBridge : MonoBehaviour
{
    public delegate void CollisionFunction(Collision collision);
    public CollisionFunction collisionFunction;

    private void OnCollisionEnter(Collision collision)
    {
        collisionFunction(collision);
    }

}