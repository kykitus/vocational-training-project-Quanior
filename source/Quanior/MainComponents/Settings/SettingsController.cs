using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using UnityEngine.Events;
using System.Linq;

public class SettingsController : MonoBehaviour
{

    public Button SetBase;
    public Button DelBase;
    public Button Returner;

    public Button Sampler;
    public Button Desampler;

    public TextMeshProUGUI[] Samplers = new TextMeshProUGUI[4];
    public SpriteRenderer[] SamplerPlaces = new SpriteRenderer[4];


    //Presets
    public Button Max25;
    public Button Max35;
    public Button Max45;

    public Slider OffsetterX;
    public TextMeshProUGUI OXVal;
    public Slider OffsetterY;
    public TextMeshProUGUI OYVal;
    public Slider ScaleX;
    public TextMeshProUGUI SXVal;
    public Slider ScaleY;
    public TextMeshProUGUI SYVal;

    public TextMeshProUGUI Output1;
    public TextMeshProUGUI Output2;
    public GameObject Ball;
    public GameObject BallOrigin;

    public BluetoothBridge Bridge;

    string MaxPosTemplate = "Maksymane wychylenie (X Y): ";
    string OutputPosTemplate = "Obecne wychylenie (X Y): ";

    long NextTwitch;

    UnityAction Samplerator = () => { };

    void Start()
    {
        GameObject bridg = GameObject.Find("BluetoothBridge");
        if (bridg == null) { print("Bridge not found"); }
        else { Bridge = bridg.GetComponent<BluetoothBridge>(); Bridge.Data.AddListener(move); }

        SetBase.onClick.AddListener(set_Base);
        DelBase.onClick.AddListener(del_Base);
        Returner.onClick.AddListener(returner);

        Sampler.onClick.AddListener(() => { Bridge.set_Smapler(true); Samplerator = get_Samples; });
        Desampler.onClick.AddListener(() => { Bridge.set_Smapler(true); Bridge.flush_Samples(); Samplerator = () => { }; });

        Max25.onClick.AddListener(() => { set_ScaleX(3.6f); set_ScaleY(3.6f); });
        Max35.onClick.AddListener(() => { set_ScaleX(2.5714f); set_ScaleY(2.5714f); });
        Max45.onClick.AddListener(() => { set_ScaleX(2f); set_ScaleY(2f); });

        OffsetterX.onValueChanged.AddListener(set_OffsetX);
        OffsetterY.onValueChanged.AddListener(set_OffsetY);
        ScaleX.onValueChanged.AddListener(Button_set_ScaleX);
        ScaleY.onValueChanged.AddListener(Button_set_ScaleY);

        OffsetterX.value = Bridge.Offsets.x;
        OffsetterY.value = Bridge.Offsets.y;
        ScaleX.value = Bridge.Scale.y;
        ScaleY.value = Bridge.Scale.y;
    }

    void Update()
    {
        if (Environment.TickCount > NextTwitch)
        {
            Samplerator();
            NextTwitch = Environment.TickCount + 1000;
        }
    }

    void set_Base() 
    { 
        Bridge.Offsets = Bridge.get_RawAngles();
        OXVal.text = Bridge.Offsets.x.ToString();
        OYVal.text = Bridge.Offsets.y.ToString();
        OffsetterX.value = Bridge.Offsets.x;
        OffsetterY.value = Bridge.Offsets.y;
    }
    void del_Base() 
    {
        Bridge.Offsets = Vector3.zero;
        OXVal.text = "0";
        OYVal.text = "0";
        OffsetterX.value = 0;
        OffsetterY.value = 0;
    }
    void returner() { Bridge.save_SaveData(); Bridge.come_Back(); }
    void set_OffsetX(float val) 
    {
        Bridge.Offsets.x = val;
        Bridge.Vault.Offsets.x = val;
        OXVal.text = val.ToString();
        update_BallOrigin();
        Output1.text = MaxPosTemplate + update_MaxAngle().ToString();
    }
    void set_OffsetY(float val) 
    {
        Bridge.Offsets.y = val;
        Bridge.Vault.Offsets.y = val;
        OYVal.text = val.ToString();
        update_BallOrigin();
        Output1.text = MaxPosTemplate + update_MaxAngle().ToString();
    }
    void Button_set_ScaleX(float val)
    {
        Bridge.Scale.x = val;
        Bridge.Vault.Scale.x = val;
        SXVal.text = val.ToString();
        Output1.text = MaxPosTemplate + update_MaxAngle().ToString();
    }
    void Button_set_ScaleY(float val)
    {
        Bridge.Scale.y = val;
        Bridge.Vault.Scale.y = val;
        SYVal.text = val.ToString();
        Output1.text = MaxPosTemplate + update_MaxAngle().ToString();
    }
    void set_ScaleX(float val) { Button_set_ScaleX(val); ScaleX.value = val; }
    void set_ScaleY(float val) { Button_set_ScaleY(val); ScaleY.value = val; }
    void move()
    {
        Vector3 angles = Bridge.get_Angles();
        Output2.text = OutputPosTemplate + angles.ToString();
        Ball.transform.localPosition = new Vector3((float)Math.Sin(angles.y * 0.01745329) * 1.5f, -1 * (float)Math.Sin(angles.x * 0.01745329) * 1.5f);
    }

    Vector2 update_MaxAngle()
    {
        Vector2 result = new Vector2(90f / Bridge.Scale.x + Bridge.Offsets.x, 90f / Bridge.Scale.y + Bridge.Offsets.y);
        return result;
    }

    void update_BallOrigin()
    {
        BallOrigin.transform.localPosition = new Vector3((float)Math.Sin(-1 * Bridge.Offsets.x * 0.01745329) * 1.5f, -1 * (float)Math.Sin( -1 * Bridge.Offsets.y * 0.01745329) * 1.5f);
    }

    void get_Samples() 
    {
        float[] nums = Bridge.get_BetterSamples();
        foreach (int n in Enumerable.Range(0,4)) 
        {
            Samplers[n].text = (nums[n] * 100f).ToString();
            Color color = SamplerPlaces[n].color;
            color.a = nums[n];
            SamplerPlaces[n].color = color;
        }
    }

    void flush_Samples()
    {
        foreach (TextMeshProUGUI n in Samplers) { n.text = ""; };
        foreach (SpriteRenderer n in SamplerPlaces) { Color color = n.color; color.a = 0; n.color = color; };
    }

}
