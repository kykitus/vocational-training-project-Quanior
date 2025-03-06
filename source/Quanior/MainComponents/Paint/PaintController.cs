using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PaintController : MonoBehaviour
{
    UnityAction<Vector3> Drawer;

    public Button StartDrawing;
    public Button Eraser;
    public Button StopDrawing;
    public Button Clean;

    public GameObject DrawBase;
    public GameObject DrawLinePrefab;
    public ColorPicker DrawColor;
    public ColorPicker WallColor;
    public Slider DrawWidth;
    public Slider EraseWidth;

    float DraWidth = 0.1f;
    float ErasWidth = 2f;
    float Width = 0.1f;

    bool is_Erasing = false;
    List<GameObject> Erasers = new List<GameObject>();

    public Button Returner;

    public TextMeshProUGUI Output;

    public SpriteRenderer BallBaseColor;
    public GameObject Ball;
    public SpriteRenderer BallColor;

    public BluetoothBridge Bridge;

    GameObject CurrentDrawing;
    LineRenderer CurrentLine;

    void Start()
    {
        GameObject bridg = GameObject.Find("BluetoothBridge");
        if (bridg == null) { print("Bridge not found"); }
        else { Bridge = bridg.GetComponent<BluetoothBridge>(); Bridge.Data.AddListener(move); }

        StartDrawing.onClick.AddListener(start_Draw);
        Eraser.onClick.AddListener(start_Erase);
        StopDrawing.onClick.AddListener(stop_Draw);
        Clean.onClick.AddListener(clean_Canva);

        DrawColor.ColorUpdate.AddListener(set_DrawColor);
        WallColor.ColorUpdate.AddListener(set_WallColor);
        DrawWidth.onValueChanged.AddListener(set_DrawWidth);
        EraseWidth.onValueChanged.AddListener(set_EraseWidth);
        DrawWidth.value = DraWidth;
        EraseWidth.value = ErasWidth;

        Returner.onClick.AddListener(returner);
    }

    void start_Draw() 
    {
        Drawer = draw;
        add_Line(DrawColor.OriginalColor, false);
        is_Erasing = false;
        Width = DraWidth;
    }
    void start_Erase() 
    {
        Drawer = draw;
        add_Line(WallColor.OriginalColor, true);
        is_Erasing = true;
        Width = ErasWidth;
    }
    void stop_Draw() { Drawer = (Vector3 n) => { }; CurrentDrawing = null; CurrentLine = null; is_Erasing = false; }
    void clean_Canva()
    {
        foreach (Transform child in DrawBase.transform) { Destroy(child.gameObject); }
        Erasers.Clear();
        Drawer = (Vector3 n) => { };
        CurrentDrawing = null;
        CurrentLine = null;
        is_Erasing = false;
    }

    void returner() { Bridge.come_Back(); }


    void set_DrawColor(Color color) 
    {
        if (CurrentLine != null) { set_Color(color); }
    }
    void set_WallColor(Color color)
    {
        BallBaseColor.color = color;
        BallColor.color = get_Inverted(color);

        foreach (GameObject eraser in Erasers)
        {
            LineRenderer erase_line = eraser.GetComponent<LineRenderer>();
            erase_line.startColor = color;
            erase_line.endColor = color;
        }
    }
    void set_Color(Color color) 
    {
        CurrentLine.startColor = color;
        CurrentLine.endColor = color;
    }
    void set_DrawWidth(float val) 
    {
        DraWidth = val;
        if (!is_Erasing) { Width = val; }
    }
    void set_EraseWidth(float val) 
    {
        ErasWidth = val;
        if (is_Erasing) { Width = val; }
    }

    private void move()
    {
        Vector3 angles = Bridge.get_Angles();
        Output.text = "K¹t: " + angles.ToString();
        Ball.transform.localPosition = new Vector3((float)Math.Sin(angles.y * 0.01745329) * 1.5f, -1 * (float)Math.Sin(angles.x * 0.01745329) * 1.5f, 0);
        Drawer(Ball.transform.position);
    }

    void draw(Vector3 new_pos) { draw_Line(new_pos); }

    void add_Line(Color color, bool is_eraser)
    {
        CurrentDrawing = GameObject.Instantiate(DrawLinePrefab, DrawBase.transform);
        CurrentLine = CurrentDrawing.GetComponent<LineRenderer>();
        CurrentLine.SetPosition(0, Ball.transform.position);
        set_Color(color);
        if (is_eraser) { Erasers.Add(CurrentDrawing); }
    }
    void draw_Line(Vector3 point)
    {
        CurrentLine.startWidth = Width;
        CurrentLine.endWidth = Width;
        CurrentLine.positionCount++;
        CurrentLine.SetPosition(CurrentLine.positionCount - 1, point);
    }

    Color get_Inverted(Color color) 
    {
        return new Color(1.0f - color.r, 1.0f - color.g, 1.0f - color.b);
    }
}
