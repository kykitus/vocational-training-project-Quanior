using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using System.Dynamic;
using UnityEngine.UI;
using TMPro;

public class BluetoothBridge : MonoBehaviour
{

    public BluetoothManager Bridge;
    public Animation ConnectionMessager;

    public TextMeshProUGUI ConnectionMessage;
    public Button AcceptButton;
    public Button DeclineButton;

    public string Message;
    public UnityEvent Data;

    public Vector3 Offsets = Vector3.zero;
    public Vector3 Scale = Vector3.one;

    UnityAction Checker = () => { };
    UnityAction Sampler = () => { };

    long NextTwitch;

    Vector3 Angles = Vector3.zero;
    int[][] SampleCounter = new int[2][];
    float AllCounts = 0;

    public string FileName = "Save.json";
    string FilePath;

    public SaveSource Vault;

    int RefreshCounter = 5;
    string[] NotifyMessages = new string[2]{ "Roz³¹czono urz¹dzenie, chcesz siê po³¹czyæ z nim ponownie?\n", "Czy chcesz po³¹czyæ siê z ostatnio zapamiêtanym urz¹dzeniem?\n" }; 

    void Awake()
    {
        if (GameObject.FindWithTag("Only") != null && GameObject.FindWithTag("Only") != gameObject) { Destroy(gameObject); return; }
        gameObject.tag = "Only";
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        SampleCounter[0] = new int[2];
        SampleCounter[1] = new int[2];
        Application.targetFrameRate = 60; // Limits to 60 FPS

        FilePath = Application.persistentDataPath + FileName;
        
        Bridge.InitBluetooth();
        Bridge.Fetcher.AddListener(get_Data);
        Bridge.Connect.AddListener(set_State);

        AcceptButton.onClick.AddListener(accept_Connection);
        DeclineButton.onClick.AddListener(decline_Connection);
        


        if (File.Exists(FilePath)) { Vault = get_SaveData(); Offsets = Vault.Offsets; Scale = Vault.Scale;  }
        else { Vault = new SaveSource(); save_SaveData(); }
        if (Vault.LastDeviceName != "00-00-00-00-00-00+Template") { remind_Connection(1); }

    }

    void Update()
    {
        if (Environment.TickCount > NextTwitch)
        {
            Sampler();
            Checker();
            NextTwitch = Environment.TickCount + 500;
        }
    }

    public void set_LastMAC(string mac) 
    {
        Vault.LastDeviceName = mac;
        save_SaveData();
    }

    public void come_Back() { GameObject.Find("Curtain").gameObject.GetComponent<Curtain>().switch_Activity(new Color(0.443f, 0.659f, 0.58f), "MainMenu"); }

    public Vector3 get_RawAngles() { return Angles; }

    public Vector3 get_Angles() { return new Vector3((Angles.x - Offsets.y) * Scale.y, (Angles.y - Offsets.x) * Scale.x, (Angles.z - Offsets.z) * Scale.z); }

    public void extract_Data_Classic()
    {
        RefreshCounter = 5;
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

    public void save_SaveData() 
    {
        File.WriteAllText(FilePath, JsonUtility.ToJson(Vault));
    }

    public SaveSource get_SaveData() 
    {
        if (File.Exists(FilePath))
        {
            return JsonUtility.FromJson<SaveSource>(File.ReadAllText(FilePath));
        }
        else { return new SaveSource(); }
    }

    void set_State(bool state) 
    {
        if (state) { Checker = check_Connection; }
    }

    public void remind_Connection(int message_type) 
    {
        ConnectionMessage.text = NotifyMessages[message_type] + Vault.LastDeviceName;
        set_AnimDirection(false);
        ConnectionMessager.Play("Halt");
    }

    public void accept_Connection() 
    {
        try_Connect();
        Checker = check_Connection;
        RefreshCounter = 5;
        set_AnimDirection(true);
        ConnectionMessager.Play("Halt");
    }
    public void decline_Connection()
    {
        Checker = check_Connection;
        RefreshCounter = 5;
        set_AnimDirection(true);
        ConnectionMessager.Play("Halt");
    }

    public void try_Connect() 
    {
        if (Vault.get_DeviceMAC() == "00-00-00-00-00-00" || Vault.get_DeviceMAC() == "") { print("no device address given"); return; }
        Bridge.DeviceToConnect = Vault.get_DeviceMAC();
        Bridge.StartConnection();
    }

    void set_AnimDirection(bool back)
    {
        if (back)
        {
            ConnectionMessager["Halt"].speed = -1;
            ConnectionMessager["Halt"].time = ConnectionMessager["Halt"].length;
        }
        else
        {
            ConnectionMessager["Halt"].speed = 1;
            ConnectionMessager["Halt"].time = 0;
        }
    }

    void check_Connection() 
    {
        if (RefreshCounter <= 0) { remind_Connection(0); Checker = () => { }; }
        else { RefreshCounter--; }
    }

    public void set_Smapler(bool really) 
    {
        if (really) { Sampler = Sample; }
        else { Sampler = () => { }; }
    }

    void Sample() 
    {
        int x = Angles.x > 0 ? 1 : 0;
        int y = Angles.y > 0 ? 1 : 0;

        SampleCounter[y][x]++;
        AllCounts++;
    }

    public float get_Sample(int x, int y) 
    {
        return (float)SampleCounter[x][y] / AllCounts;
    }

    public float[] get_BetterRawSamples() { return new float[] { SampleCounter[0][0], SampleCounter[1][0], SampleCounter[0][1], SampleCounter[1][1] }; }

    public float[] get_BetterSamples() { return  new float[] { get_Sample(0, 0), get_Sample(1, 0), get_Sample(0, 1), get_Sample(1, 1) }; }

    public void flush_Samples() 
    {
        AllCounts = 0;
        SampleCounter[0] = new int[2];
        SampleCounter[1] = new int[2];
    }
}
