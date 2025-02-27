using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeviceButton : MonoBehaviour
{
    public TextMeshProUGUI Name;
    Button Clicker;

    public BluetoothServiceManager Selector;
    public BluetoothDump Dump;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Clicker = GetComponent<Button>();
        Selector = GameObject.Find("Manager").GetComponent<BluetoothServiceManager>();
        Dump = GameObject.Find("Dump").GetComponent<BluetoothDump>();

        Clicker.onClick.AddListener(Clicked);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Clicked() 
    {
        print(Name);
        Selector.SelectedDevice = Name.text;
        Dump.print_Log("Selected: " + Name.text);
    }
}
