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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Icon = transform.Find("Icon").gameObject.GetComponent<Image>();
        Name = transform.Find("Name").gameObject.GetComponent<TextMeshPro>();

        update_Layout();
    }

    // Update is called once per frame
    void Update()
    {

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
    }

    public void on_Click() 
    {
        SceneManager.LoadScene(ScenePath);
    }


}
