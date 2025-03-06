using System;
using UnityEngine;
using UnityEngine.UIElements;

public class FlyingBlock : MonoBehaviour
{

    public float MaxSpeed = 30;

    float Velocity = 0;

    float SpeedFactor = 1;

    void Start()
    {
        
    }

    void Update()
    {
        Velocity = Mathf.Lerp(Velocity, MaxSpeed, 0.05f);
        transform.position += new Vector3(0, 0, -Velocity) * Time.deltaTime;
    }

    public void init(PlaneGame parent) { parent.Paused.AddListener(pause); parent.Resumed.AddListener(resume); }

    void pause() { MaxSpeed = 0; }
    void resume() { MaxSpeed = 3000; }

    public void die() { GetComponent<Animation>().Play("BlockDestroy"); }
    public void fucking_die() { Destroy(gameObject); }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Destroyer") || other.gameObject.CompareTag("Player"))
        {
            MaxSpeed = 0;
            Velocity = 0;
            die();
        }
    }
}
