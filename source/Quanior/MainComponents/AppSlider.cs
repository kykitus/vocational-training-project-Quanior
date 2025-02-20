using System.IO;
using UnityEngine;
using System.Linq;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class AppSlider : MonoBehaviour
{
    public string AppDataPath;
    public string RelativeDataPath;

    public GameObject AppIconPrefab;
    GameObject AppBox;

    int size = 0;
    public float step = 6.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AppBox = transform.Find("Viewport/Content").gameObject;
#if UNITY_EDITOR_WIN
        string[] files = Directory.GetFiles(AppDataPath, "*.asset", SearchOption.AllDirectories).Select(Path.GetFileNameWithoutExtension).ToArray();
        foreach (string file in files) 
        {
            var new_app = add_EmptyApp();

            AppIcon data = Resources.Load<AppIcon>(RelativeDataPath + file);
            if (data == null) { print("AppIconData not found"); }
            else { new_app.GetComponent<AppIconInstance>().set_Preset(data); }
         }
#elif UNITY_ANDROID
        var assets = Resources.LoadAll("Apps", typeof(AppIcon));
        foreach (AppIcon asset in assets)
        {
            var new_app = add_EmptyApp();
            new_app.GetComponent<AppIconInstance>().set_Preset(asset);
        }
#endif
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    GameObject add_EmptyApp() 
    {
        GameObject new_app = Object.Instantiate(AppIconPrefab, AppBox.transform);
        new_app.transform.position += new Vector3(size * step + step, 0, 0);
        size++;
        AppBox.GetComponent<RectTransform>().offsetMax += new Vector2(step - 0.4f, 0);
        return new_app;
    }
}
