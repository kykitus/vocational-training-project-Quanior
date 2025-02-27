using System;
using System.Globalization;
using System.Linq;
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

    Vector3 Angles = Vector3.zero;

    public Vector3 Offsets = Vector3.zero;
    public Vector3 Scale = Vector3.one;

    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        Bridge.InitBluetooth();
        Bridge.Fetcher.AddListener(get_Data);
        //Bridge.StartScanDevices();

    }

    public Vector3 get_RawAngles() { return Angles; }

    public Vector3 get_Angles() { return new Vector3((Angles.x - Offsets.y) * Scale.y, (Angles.y - Offsets.x) * Scale.x, (Angles.z - Offsets.z) * Scale.z); }

    public void extract_Data_Classic()
    {
        string data = Bridge.Found;

        int separator = data.IndexOf(" ", StringComparison.Ordinal);

        if (separator == 0) { return; }
        print(data);
        Angles.x = float.Parse(data.Substring(0, separator), CultureInfo.InvariantCulture);

        data = data.Substring(separator+1);
        print(data);
        separator = data.IndexOf(" ", StringComparison.Ordinal);

        Angles.y = float.Parse(data.Substring(0, separator), CultureInfo.InvariantCulture);

        data = data.Substring(separator+1);
        print(data);
        Angles.z = float.Parse(data, CultureInfo.InvariantCulture);

    }

    public void get_Data() 
    {
        Message = Bridge.Found;
        extract_Data_Classic();
        Data.Invoke();
    }

}
