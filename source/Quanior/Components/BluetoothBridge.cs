using System;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BluetoothBridge : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public BluetoothManager Bridge;

    public string Message;

    public UnityEvent Data;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        Bridge.InitBluetooth();
        Bridge.Fetcher.AddListener(get_Data);
        //Bridge.StartScanDevices();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 extract_Data_Classic()
    {
        string data = Bridge.Found;
        Vector3 result = new Vector3();
        int separator = data.IndexOf(" ", StringComparison.Ordinal);

        if (separator == 0) { return Vector3.zero; }

        result.x = Int32.Parse(data.Substring(0, separator));
        data = data.Substring(0, separator);
        result.z = Int32.Parse(data.Substring(0, separator));
        data = data.Substring(0, separator);
        result.y = Int32.Parse(data.Substring(0, separator));

        return result;
    }

    public void get_Data() 
    {
        Message = Bridge.Found;
        Data.Invoke();
    }

    void setup_to_Scene(Scene current, Scene next) 
    {

    }
}
