using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    public int switchIndex;
    public Light linkedLight;

    MQTT mqtt;
    
    void Start()
    {
        mqtt = FindObjectOfType<MQTT>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (linkedLight != null && mqtt != null)
        {
            bool newState = !linkedLight.enabled;
            linkedLight.enabled = newState;

            mqtt.SetLedState(switchIndex, newState);
        }
    }
}
