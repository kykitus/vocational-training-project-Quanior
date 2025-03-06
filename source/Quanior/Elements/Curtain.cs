using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Curtain : MonoBehaviour
{ 
    Animation Anim;
    SpriteRenderer Fiber;
    string TargetScene = "Setting";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (GameObject.FindWithTag("Only") != null && GameObject.FindWithTag("Only") != gameObject) { Destroy(gameObject); return; }
        gameObject.tag = "Only";
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        Anim = GetComponent<Animation>();
        Fiber = GetComponent<SpriteRenderer>();
        //SceneManager.activeSceneChanged += switch_Activity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void switch_Activity(Color color, string scene_path)
    {
        Fiber.color = color;
        TargetScene = scene_path;
        Anim.Play("CurtainSwitch");
    }

    public void change() { SceneManager.LoadScene(TargetScene); }
}
