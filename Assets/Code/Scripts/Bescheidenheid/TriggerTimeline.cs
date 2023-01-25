using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TriggerTimeline : MonoBehaviour
{
    [SerializeField] PlayableDirector director;
    [SerializeField] bool activeAfterPlay = true;
    private PlayableDirector director2;

    void Awake()
    {
        director2 = GameObject.Find("Red").GetComponent<PlayableDirector>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            director.Play();
            this.transform.parent.gameObject.SetActive(activeAfterPlay);
            Debug.Log(director2);
        }
    }
}
