using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class SettingsController : MonoBehaviour
{

    public Button SetBase;
    public Button DelBase;
    public Button Returner;

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
    Vector2 MaxAngle = new Vector2(90f, 90f);

    void Start()
    {
        Bridge = GameObject.Find("BluetoothBridge").GetComponent<BluetoothBridge>();


        SetBase.onClick.AddListener(set_Base);
        DelBase.onClick.AddListener(del_Base);
        Returner.onClick.AddListener(returner);

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

        Bridge.Data.AddListener(move);
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
    void returner() { SceneManager.LoadScene("MainMenu"); }

    void set_OffsetX(float val) { Bridge.Offsets.x = val; OXVal.text = val.ToString(); update_BallOrigin(); Output1.text = MaxPosTemplate + update_MaxAngle().ToString(); }
    void set_OffsetY(float val) { Bridge.Offsets.y = val; OYVal.text = val.ToString(); update_BallOrigin(); Output1.text = MaxPosTemplate + update_MaxAngle().ToString(); }
    void Button_set_ScaleX(float val)
    {
        Bridge.Scale.x = val;
        SXVal.text = val.ToString();
        Output1.text = MaxPosTemplate + update_MaxAngle().ToString();
    }
    void Button_set_ScaleY(float val)
    {
        Bridge.Scale.y = val;
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
}
