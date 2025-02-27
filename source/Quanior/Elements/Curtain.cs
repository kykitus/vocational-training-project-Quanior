using UnityEngine;
using UnityEngine.SceneManagement;

public class Curtain : MonoBehaviour
{
    Animator Anims;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        Anims = GetComponent<Animator>();
        //SceneManager.activeSceneChanged += switch_Activity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void switch_Activity() { Anims.Play("CurtainDOWN"); Anims.Play("CurtainUP"); print("Curtaining"); }
}
