using UnityEngine;

public class ConvexCollider : MonoBehaviour
{
    public static void AddConvexCollider(ref GameObject obj, string meshName = null)
    {
        MeshCollider collider = obj.AddComponent<MeshCollider>();

        if (meshName != null)
        {
            foreach (Transform child in obj.transform)
                if (child.name == meshName)
                    collider.sharedMesh = child.GetComponent<Mesh>();
        }
        else
            collider.sharedMesh = obj.GetComponentInChildren<Mesh>();
    }
}