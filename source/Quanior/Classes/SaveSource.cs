using UnityEngine;

public class SaveSource
{
    public Vector2 Offsets = Vector2.zero;
    public Vector2 Scale = Vector2.one;
    public int AllCoins = 0;
    public string LastDeviceName = "00-00-00-00-00-00+Template";

    public string get_DeviceMAC() { return LastDeviceName.Substring(0, 17); }
    public string get_DeviceName() { return LastDeviceName.Substring(18); }
}
