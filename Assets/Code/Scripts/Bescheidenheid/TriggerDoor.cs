using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TriggerDoor : MonoBehaviour
{
    private PlayableDirector director;

    void Awake()
    {
        director = GameObject.Find("Red").GetComponent<PlayableDirector>();
        Debug.Log(director);

    }

}
