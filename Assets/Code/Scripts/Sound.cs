using UnityEngine;

[System.Serializable]
public class Sound : MonoBehaviour
{
    public string soundName;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;

    [Range(.1f, 1f)]
    public float pitch;

    [Range(500f, 2000f)]
    public float maxDistance;

    public bool spatialBlend;
    public bool loop;
    public bool fadeIn;
    public bool fadeOut;

    [HideInInspector]
    public AudioSource source;
}