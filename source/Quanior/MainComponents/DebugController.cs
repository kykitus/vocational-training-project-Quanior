using UnityEngine.Android;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using System.Linq;

public class DebugController : MonoBehaviour
{

    public Button start;
    public Button Stop;
    public Button None;
    public Button Connector;
    public Button StateChecker;
    public TextMeshProUGUI Output;
    public BluetoothDump Dump;
    public DebugBall Ball;
    public GameObject Slider;
    public BluetoothServiceManager Manager;


    public BluetoothBridge Bridge;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Bridge = GameObject.Find("BluetoothBridge").GetComponent<BluetoothBridge>();
        start.onClick.AddListener(Bridge.Bridge.StartScanDevices);
        Stop.onClick.AddListener(Bridge.Bridge.StopScanDevices);
        None.onClick.AddListener(get_paired);
        Connector.onClick.AddListener(try_Connect);
        //StateChecker.onClick.AddListener(check_Connection);

        Bridge.Data.AddListener(fetch_Data);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void try_Connect() 
    {
        Bridge.Bridge.DeviceToConnect = Manager.get_DeviceMAC();
        Bridge.Bridge.StartConnection();
    }

    public void fetch_Data()
    {
        /*Vector3 angle = Bridge.extract_Data_Classic();
        Output.text = angle.ToString();
        Ball.Ball.transform.localPosition = angle;*/
        Output.text = Bridge.Message;
    }

    

    public void get_paired() 
    {
        Bridge.Bridge.Slider = Slider;
        Bridge.Bridge.GetPairedDevices();
    }


}
