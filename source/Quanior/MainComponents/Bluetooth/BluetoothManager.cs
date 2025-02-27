using System.Collections;
using System.Collections.Generic;
using UnityEngine.Android;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class BluetoothManager : MonoBehaviour
{
    public Text deviceAdd;
    public Text dataToSend;
    public Text receivedData;
    public GameObject devicesListContainer;
    public GameObject deviceMACText;
    private bool isConnected;


    // Mine
    public string DeviceToConnect;
    public string Found;

    public UnityEvent Fetcher;
    int quantity = 0;
    public GameObject Selectable;
    public GameObject Slider;
    //

    private static AndroidJavaClass unity3dbluetoothplugin;
    private static AndroidJavaObject BluetoothConnector;
    // Start is called before the first frame update

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        InitBluetooth();
        isConnected = false;
    }

    // creating an instance of the bluetooth class from the plugin 
    public void InitBluetooth()
    {
        if (Application.platform != RuntimePlatform.Android)
            return;

        // Check BT and location permissions
        if (!Permission.HasUserAuthorizedPermission(Permission.CoarseLocation)
            || !Permission.HasUserAuthorizedPermission(Permission.FineLocation)
            || !Permission.HasUserAuthorizedPermission("android.permission.BLUETOOTH_ADMIN")
            || !Permission.HasUserAuthorizedPermission("android.permission.BLUETOOTH")
            || !Permission.HasUserAuthorizedPermission("android.permission.BLUETOOTH_SCAN")
            || !Permission.HasUserAuthorizedPermission("android.permission.BLUETOOTH_ADVERTISE")
            || !Permission.HasUserAuthorizedPermission("android.permission.BLUETOOTH_CONNECT"))
        {

            Permission.RequestUserPermissions(new string[] {
                        Permission.CoarseLocation,
                            Permission.FineLocation,
                            "android.permission.BLUETOOTH_ADMIN",
                            "android.permission.BLUETOOTH",
                            "android.permission.BLUETOOTH_SCAN",
                            "android.permission.BLUETOOTH_ADVERTISE",
                             "android.permission.BLUETOOTH_CONNECT"
                    });

        }

        unity3dbluetoothplugin = new AndroidJavaClass("com.example.unity3dbluetoothplugin.BluetoothConnector");
        BluetoothConnector = unity3dbluetoothplugin.CallStatic<AndroidJavaObject>("getInstance");
    }

    // Start device scan
    public void StartScanDevices()
    {
        Toast("Start");
        print("Started");
        if (Application.platform != RuntimePlatform.Android)
            return;
        // Destroy devicesListContainer child objects for new scan display
        foreach (Transform child in devicesListContainer.transform)
        {
            Destroy(child.gameObject);
        }
        Toast("GottenUnderground");
        BluetoothConnector.CallStatic("StartScanDevices");
    }

    // Stop device scan
    public void StopScanDevices()
    {
        Toast("Stop");
        print("Stopped");
        if (Application.platform != RuntimePlatform.Android)
            return;
        BluetoothConnector.CallStatic("StopScanDevices");
    }

    // This function will be called by Java class to update the scan status,
    // DO NOT CHANGE ITS NAME OR IT WILL NOT BE FOUND BY THE JAVA CLASS
    public void ScanStatus(string status)
    {
        Toast("Scan Status: " + status);
    }

    // This function will be called by Java class whenever a new device is found,
    // and delivers the new devices as a string data="MAC+NAME"
    // DO NOT CHANGE ITS NAME OR IT WILL NOT BE FOUND BY THE JAVA CLASS
    public void NewDeviceFound(string data)
    {
        GameObject newDevice = deviceMACText;
        newDevice.GetComponent<Text>().text = data;
        Toast("Found: " + data);
        Instantiate(newDevice, devicesListContainer.transform);  
    }

    // Get paired devices from BT settings
    public void GetPairedDevices()
    {

        if (Application.platform != RuntimePlatform.Android)
            return;

        // This function when called returns an array of PairedDevices as "MAC+Name" for each device found
        string[] data = BluetoothConnector.CallStatic<string[]>("GetPairedDevices"); ;
        // Destroy devicesListContainer child objects for new Paired Devices display
        foreach (Transform child in Slider.transform)
        {
            Destroy(child.gameObject);
        }
        quantity = 0;

        foreach (var name in data) 
        {
            GameObject new_selectable = Object.Instantiate(Selectable, Slider.transform);
            new_selectable.GetComponent<DeviceButton>().Name.text = name;
            quantity += 1;
            new_selectable.transform.localPosition = new Vector3(400, -10 + quantity * -72f, 0);
        }
    }

    // Start BT connect using device MAC address "deviceAdd"
    public void StartConnection()
    {
        if (Application.platform != RuntimePlatform.Android)
            return;

        //BluetoothConnector.CallStatic("StartConnection", deviceAdd.text.ToString().ToUpper());
        BluetoothConnector.CallStatic("StartConnection", DeviceToConnect.ToString().ToUpper());
    }

    // Stop BT connetion
    public void StopConnection()
    {
        if (Application.platform != RuntimePlatform.Android)
            return;

        if (isConnected)
            BluetoothConnector.CallStatic("StopConnection");
    }

    // This function will be called by Java class to update BT connection status,
    // DO NOT CHANGE ITS NAME OR IT WILL NOT BE FOUND BY THE JAVA CLASS
    public void ConnectionStatus(string status)
    {
        Toast("Connection Status: " + status);
        isConnected = status == "connected";
    }

    // This function will be called by Java class whenever BT data is received,
    // DO NOT CHANGE ITS NAME OR IT WILL NOT BE FOUND BY THE JAVA CLASS
    public void ReadData(string data)
    {
        Debug.Log("BT Stream: " + data);
        Found = data;
        Fetcher.Invoke();
    }

    // Write data to the connected BT device
    public void WriteData()
    {
        if (Application.platform != RuntimePlatform.Android)
            return;

        if (isConnected)
            BluetoothConnector.CallStatic("WriteData", dataToSend.text.ToString());
    }

    // This function will be called by Java class to send Log messages,
    // DO NOT CHANGE ITS NAME OR IT WILL NOT BE FOUND BY THE JAVA CLASS
    public void ReadLog(string data)
    {
        Debug.Log(data);
    }


    // Function to display an Android Toast message
    public void Toast(string data)
    {
        if (Application.platform != RuntimePlatform.Android)
            return;

        BluetoothConnector.CallStatic("Toast", data);
    }
}
