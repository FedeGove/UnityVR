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

    private bool[] ledStates = new bool[4];
    public GameObject[] windows;

    public string brokerAddress = "192.168.0.144";

    public TextMeshPro text;
    public AudioSource audioSourceDoorbell;
    public AudioSource audioSourceWindows;
    private float? newTempValue = null;
    private bool playAudio = false;
    private bool playWindows = false;
    private bool shouldCloseWindows = false;
    private bool shouldOpenWindows = false;
    private bool hasPlayedRainAudio = false;
    private bool hasPlayedDoorbellAudio = false;

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

        client.Subscribe(new string[] { rainTopic }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
        client.Subscribe(new string[] { pirTopic }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
        client.Subscribe(new string[] { tempTopic }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });

        Debug.Log("MQTT connesso");

    }

    private void OnMessageReceived(object sender, MqttMsgPublishEventArgs e)
    {
        string mess = Encoding.UTF8.GetString(e.Message);

        if (e.Topic == rainTopic)
        {
            if (float.TryParse(mess, out float messFloat))
            {
                if (messFloat == 0.00f)
                {
                    //Debug.Log("Sta piovendo");
                    shouldCloseWindows = true;
                    shouldOpenWindows = false;

                    if (hasPlayedRainAudio == false)
                    {
                        playWindows = true;
                        hasPlayedRainAudio = true;
                    }
                }
                else if (messFloat == 100f)
                {
                    //Debug.Log("Non sta piovendo");
                    shouldCloseWindows = false;
                    shouldOpenWindows = true;
                    hasPlayedRainAudio = false;
                }
            }
        }
        else if (e.Topic == tempTopic)
        {
            if (float.TryParse(mess, out float messFloat))
            {
                newTempValue = messFloat / 100;
            }
        }
        else if (e.Topic == pirTopic)
        {
            if (mess == "1.00")
            {
                if (!hasPlayedDoorbellAudio)
                {
                    playAudio = true;
                    hasPlayedDoorbellAudio = true;
                }
            }else
            {
                hasPlayedDoorbellAudio = false;
            }
        }
    }

    public void SetLedState(int index, bool isOn)
    {
        ledStates[index] = isOn;
        string message = (isOn ? "on" : "off");

        string topic = index switch
        {
            0 => led1Topic,
            1 => led2Topic,
            2 => led3Topic,
            3 => led4Topic,
            _ => ""
        };

        if (!string.IsNullOrEmpty(topic))
        {
            client.Publish(topic, Encoding.UTF8.GetBytes(message));
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Temperatura
        if (newTempValue.HasValue)
        {
            text.text = newTempValue.Value.ToString("F1") + "°C";
            newTempValue = null;
        }

        //Audio Citofono
        if (playAudio)
        {
            if (audioSourceDoorbell != null)
            {
                audioSourceDoorbell.Play();
            }

            playAudio = false;
        }

        //Audio Finestre Chiusura
        if (playWindows)
        {
            audioSourceWindows.Play();
            playWindows = false;
        }

        //Chiusura Finestre
        if (shouldCloseWindows)
        {
            foreach (GameObject window in windows)
            {
                Vector3 pos = window.transform.position;
                pos.y = 1.3f;
                window.transform.position = pos;
            }
            shouldCloseWindows = false;
        }

        //Apertura Finestre
        if (shouldOpenWindows)
        {
            foreach (GameObject window in windows)
            {
                Vector3 pos = window.transform.position;
                pos.y = 2f;
                window.transform.position = pos;
            }
            shouldOpenWindows = false;
        }
    }
}
