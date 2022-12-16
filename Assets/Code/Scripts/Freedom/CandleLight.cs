using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleLight : MonoBehaviour
{
    private Light LightSource;

    // Start is called before the first frame update
    void Start()
    {
        LightSource = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        int random = Random.Range(10, 20);
        //LightSource.intensity = random;
    }
}
