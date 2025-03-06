using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AppIconInstance : MonoBehaviour
{
    AppIcon Preset;

    Image Icon;
    TextMeshPro Name;
    string ScenePath = "MainMenu";
    Color TransitionColor = Color.green;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Icon = transform.Find("Icon").gameObject.GetComponent<Image>();
        Name = transform.Find("Name").gameObject.GetComponent<TextMeshPro>();

        update_Layout();
    }

    public void set_Preset(AppIcon preset)
    {
        Preset = preset;
        
    }

    public void update_Layout()
    {
        Icon.sprite = Sprite.Create(Preset.Icon, new Rect(0.0f, 0.0f, Preset.Icon.width, Preset.Icon.height), new Vector2(0.5f, 0.5f), 100.0f);
        Name.text = Preset.AppName;
        ScenePath = Preset.SceneName;
        TransitionColor = Preset.TransitionColor;
    }

    public void on_Click() 
    {
        GameObject.Find("Curtain").gameObject.GetComponent<Curtain>().switch_Activity(TransitionColor, ScenePath);
    }


}
