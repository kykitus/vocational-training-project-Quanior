using UnityEngine;

public class BluetoothServiceManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public string SelectedDevice = "00-00-00-00-00-00+Template";

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string get_DeviceMAC() { return SelectedDevice.Substring(0, 17); }
    public string get_DeviceName() { return SelectedDevice.Substring(18); }
}
