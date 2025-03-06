using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using System;
using Unity.VisualScripting;
using TMPro;

public class AirPlane : MonoBehaviour
{

    public TextMeshProUGUI CoinCounter;

    public Vector2 MaxSpeed = new Vector3(30f, 15f);
    public Vector2 PositionRange = new Vector2(30.0f, 15.0f);

    public float Brake = 50.0f;
    public float MaxRotation = 45f;

    Vector2 Velocity = Vector2.zero;
    int Coins = 0;

    public UnityEvent Died;
    public UnityEvent DiedEvenMore;
    public UnityEvent CoinUP;
    UnityAction Steerer = () => {};

    Animation Anim;


    public BluetoothBridge Bridge;

    void Start()
    {
        GameObject bridg = GameObject.Find("BluetoothBridge");
        if (bridg == null) { print("Bridge not found"); }
        else { Bridge = bridg.GetComponent<BluetoothBridge>(); Bridge.Data.AddListener(steer); }
        Anim = GetComponent<Animation>();
    }

    void Update() { Steerer(); }

    public void start_Flying() { Steerer = flight; } 

    void steer() 
    {
        Vector2 angle = Bridge.get_Angles();
        angle = new Vector2((float)Math.Sin(angle.y * 0.01745329), -1 * (float)Math.Sin(angle.x * 0.01745329));

        Velocity += new Vector2(MaxSpeed.x * angle.x, -1 * MaxSpeed.y * angle.y);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, -1 * Mathf.Lerp(transform.rotation.z, MaxRotation * Mathf.Sign(angle.x), Math.Abs(angle.x))));
    }

    void flight() 
    {
        move();
        hit_Wall();
        damp_Velocity();
    }

    void post_mortem_flight() 
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(30f, transform.rotation.y, transform.rotation.z), 0.02f);
        Velocity.y -= MaxSpeed.y * 2.5f;
        move();
    }

    void move() { transform.position += new Vector3(Velocity.x, Velocity.y) * Time.deltaTime; }

    void hit_Wall() 
    {
        transform.position = new Vector3
            (
            Mathf.Clamp(transform.position.x, -PositionRange.x, PositionRange.x),
            Mathf.Clamp(transform.position.y, -PositionRange.y, PositionRange.y)
            );
    }

    void damp_Velocity() 
    {
        Vector2 velocity_value = new Vector2(Math.Abs(Velocity.x), Math.Abs(Velocity.y));
        Velocity.x = Math.Clamp(velocity_value.x - Brake * Time.deltaTime, 0.0f, velocity_value.x) * Math.Sign(Velocity.x);
        Velocity.y = Math.Clamp(velocity_value.y - Brake * Time.deltaTime, 0.0f, velocity_value.y) * Math.Sign(Velocity.y);
    }

    void die() 
    {
        Steerer = post_mortem_flight;
        //Died.Invoke();
    }

    void fucking_die() 
    {
        Anim.Play("Death");
    }

    void die_die() 
    {
        Steerer = flight;
        Died.Invoke();
        Coins = 0;
        Velocity = Vector2.zero;
        transform.rotation = Quaternion.Euler(0,0,0);
    }

    void coin_Up() 
    {
        Coins += 1;
        CoinUP.Invoke();
        CoinCounter.text = "Monety: " + Coins.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag) 
        {
            case "BadEntity":
                die();
                break;
            case "GoodEntity":
                coin_Up();
                break;
            case "BadDestroyer":
                fucking_die();
                break;
        }
    }

}
