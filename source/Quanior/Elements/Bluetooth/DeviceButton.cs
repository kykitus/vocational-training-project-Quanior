using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeviceButton : MonoBehaviour
{
    public TextMeshProUGUI Name;
    Button Clicker;

    public BluggerController Selector;
    public BluetoothDump Dump;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Clicker = GetComponent<Button>();
        Selector = GameObject.Find("Controller").GetComponent<BluggerController>();
        Dump = GameObject.Find("Dump").GetComponent<BluetoothDump>();

        Clicker.onClick.AddListener(Clicked);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Clicked() 
    {
        Selector.Bridge.set_LastMAC(Name.text);
        Dump.print_Log("Wybrano: " + Name.text);
    }
}
