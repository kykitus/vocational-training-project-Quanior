using UnityEngine.Android;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using System.Linq;

public class BluggerController : MonoBehaviour
{
    public Button GetPaired;
    public Button Connector;
    public Button Returner;
    public TextMeshProUGUI Output;
    public BluetoothDump Dump;
    public GameObject Slider;
    public DebugBall Ball;


    public BluetoothBridge Bridge;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject bridg = GameObject.Find("BluetoothBridge");
        if (bridg == null) { print("Bridge not found"); }
        else { Bridge = bridg.GetComponent<BluetoothBridge>(); Bridge.Data.AddListener(fetch_Data); }

        GetPaired.onClick.AddListener(get_paired);
        Connector.onClick.AddListener(try_Connect);
        Returner.onClick.AddListener(returner);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void returner() { Bridge.come_Back(); }

    public void try_Connect()
    {
        Bridge.try_Connect();
    }

    public void fetch_Data()
    {
        Vector3 angle = Bridge.get_RawAngles();
        Output.text = "K¹t: " + angle.ToString();
        angle = new Vector3((float)Math.Sin(angle.y * 0.01745329) * 7.5f, -1 * (float)Math.Sin(angle.x * 0.01745329) * 4, 0.0f);
        Ball.Ball.transform.localPosition = angle;
        Output.text += "\nPozycja: " + angle.ToString();
    }

    public void get_paired() 
    {
        Bridge.Bridge.Slider = Slider;
        Bridge.Bridge.GetPairedDevices();
    }

}
