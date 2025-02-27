using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ColorPicker : MonoBehaviour
{

    public UnityEvent<Color> ColorUpdate;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Slider Hue;
    public Slider Saturation;
    public Slider Light;

    public SpriteRenderer Preview;

    public Color OriginalColor = Color.white;
    public Vector3 TargetColor = Vector3.zero;

    void Start()
    {
        Hue.onValueChanged.AddListener(set_Hue);
        Saturation.onValueChanged.AddListener(set_Saturation);
        Light.onValueChanged.AddListener(set_Light);
        update_Color();
    }

    void set_Hue(float val) { TargetColor.x = val; update_Color(); }
    void set_Saturation(float val) { TargetColor.y = val; update_Color(); }
    void set_Light(float val) { TargetColor.z = val; update_Color(); }
    void update_Color()
    { 
        OriginalColor = Color.HSVToRGB(TargetColor.x, TargetColor.y, TargetColor.z);
        Preview.color = OriginalColor;
        ColorUpdate.Invoke(OriginalColor);
    }
}
