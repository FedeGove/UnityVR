using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

public class MQTTTest : MonoBehaviour
{

    private MqttClient client;
    private string led1Topic = "attuatori/casa/rgb1";
    private string led2Topic = "attuatori/casa/rgb2";
    private string led3Topic = "attuatori/casa/rgb3";
    private string led4Topic = "attuatori/casa/rgb4";

    public string brokerAddress = "192.168.0.144";
    private bool[] ledStates = new bool[4];

    // Start is called before the first frame update
    void Start()
    {
        client = new MqttClient(brokerAddress);
        client.MqttMsgPublishReceived += OnMessageReceived;
        client.Connect(System.Guid.NewGuid().ToString());

        client.Subscribe(new string[] { led1Topic }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
        client.Subscribe(new string[] { led2Topic }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
        client.Subscribe(new string[] { led3Topic }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
        client.Subscribe(new string[] { led4Topic }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });

        Debug.Log("MQTT connesso");
    }

    private void OnMessageReceived(object sender, MqttMsgPublishEventArgs e)
    {

    }

    public void SetLedStateButton(int index)
    {
        string command = ledStates[index] ? "off" : "on";
        ledStates[index] = !ledStates[index];

        string topic = index switch
        {
            0 => led1Topic,
            1 => led2Topic,
            2 => led3Topic,
            3 => led4Topic,
            _ => ""
        };

        client.Publish(topic, Encoding.UTF8.GetBytes(command));
        Debug.Log("Pubblicato: Topic: " + topic + ", Messaggio: " + command);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
