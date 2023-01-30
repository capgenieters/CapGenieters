using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class SceneData : ScriptableObject
{
    public string SceneName;
    public GameObject PlayerPrefab;
    public Vector3 SpawnPosition;
    public Quaternion SpawnRotation;
}
