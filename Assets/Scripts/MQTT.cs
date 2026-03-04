using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

public class MQTT : MonoBehaviour
{

    private MqttClient client;
    private string led1Topic = "attuatori/casa/rgb1";
    private string led2Topic = "attuatori/casa/rgb2";
    private string led3Topic = "attuatori/casa/rgb3";
    private string led4Topic = "attuatori/casa/rgb4";
    private string rainTopic = "attuatori/casa/finestra";
    private string tempTopic = "sensori/casa/temp";
    private string pirTopic = "sensori/casa/pir";

    public string brokerAddress = "192.168.0.144";

    // Start is called before the first frame update
    void Start()
    {
        client = new MqttClient(brokerAddress);
        client.Connect(System.Guid.NewGuid().ToString());

        client.Subscribe(new string[] { led1Topic }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
        client.Subscribe(new string[] { led2Topic }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
        client.Subscribe(new string[] { led3Topic }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
        client.Subscribe(new string[] { led4Topic }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });

        client.Subscribe(new string[] { rainTopic }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
        client.Subscribe(new string[] { pirTopic }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
        client.Subscribe(new string[] { tempTopic }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });

        Debug.Log("MQTT connesso");

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
