using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
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
        Bridge = GameObject.Find("BluetoothBridge").GetComponent<BluetoothBridge>();
        StartDrawing.onClick.AddListener(start_Draw);
        Eraser.onClick.AddListener(start_Erase);
        StopDrawing.onClick.AddListener(stop_Draw);
        Clean.onClick.AddListener(clean_Canva);

        DrawColor.ColorUpdate.AddListener(set_DrawColor);
        WallColor.ColorUpdate.AddListener(set_WallColor);

        Returner.onClick.AddListener(returner);

        Bridge.Data.AddListener(move);
    }

    void start_Draw() 
    {
        Drawer = draw;
        add_Line(DrawColor.OriginalColor, false);
    }
    void start_Erase() 
    {
        Drawer = draw;
        add_Line(WallColor.OriginalColor, true);
    }
    void stop_Draw() { Drawer = (Vector3 n) => { }; CurrentDrawing = null; CurrentLine = null; }
    void clean_Canva()
    {
        foreach (Transform child in DrawBase.transform) { Destroy(child.gameObject); }
        Drawer = (Vector3 n) => { };
        CurrentDrawing = null;
        CurrentLine = null;
    }

    void returner() { SceneManager.LoadScene("MainMenu"); }


    void set_DrawColor(Color color) 
    {
        if (CurrentLine != null) { set_Color(color); }
    }
    void set_WallColor(Color color)
    {
        if (is_Erasing && CurrentLine != null) { set_Color(color); }
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

    private void move()
    {
        Vector3 angles = Bridge.get_Angles();
        Output.text = angles.ToString();
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
        CurrentLine.startWidth = 0.1f;
        CurrentLine.endWidth = 0.1f;
        CurrentLine.positionCount++;
        CurrentLine.SetPosition(CurrentLine.positionCount - 1, point);
    }

    Color get_Inverted(Color color) 
    {
        return new Color(1.0f - color.r, 1.0f - color.g, 1.0f - color.b);
    }
}
