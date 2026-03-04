using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    MQTT mqtt;
    
    void Start()
    {
        mqtt = FindObjectOfType<MQTT>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
