using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class BluetoothDump : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    TextMeshProUGUI Dump;
    void Start()
    {
        Dump = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void print_Log( string log ) 
    {
        Dump.text += log + "\n";
    }
}
