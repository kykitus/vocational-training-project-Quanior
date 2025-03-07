using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class PlaneController : MonoBehaviour
{

    public Button Starter;
    public Button Returner;
    public Button Pauser;
    public Toggle Mode;

    public Animation MenuAnimator;

    public Slider Difficulty;
    public TextMeshProUGUI DiffVal;

    public AirPlane Plane;
    public PlaneGame Game;

    public TextMeshProUGUI LeaderBoard;
    string[] LeaderBoardSamples = new string[4] { "Dzisiejsze punkty: ", "\n\nCa³kowita iloœæ punktów: ", "", ""};

    int AllCoins = 0;
    int AllAllCoins = 0;

    public BluetoothBridge Bridge;

    void Start()
    {
        GameObject bridg = GameObject.Find("BluetoothBridge");
        if (bridg == null) { print("Bridge not found"); }
        else { Bridge = bridg.GetComponent<BluetoothBridge>(); }

        update_Coin();

        Starter.onClick.AddListener(starter);
        Returner.onClick.AddListener(returner);
        Pauser.onClick.AddListener(pauser);
        Mode.onValueChanged.AddListener(set_Mode);

        Plane.CoinUP.AddListener(coin_Add);
        Plane.Died.AddListener(pauser);

        Difficulty.onValueChanged.AddListener(update_Difficulty);
        
    }

    void Update()
    {
        Bridge.transform.position = Plane.transform.position + new Vector3(0,0, 8.06f);
        Bridge.transform.rotation = Plane.transform.rotation;
    }

    void starter() { Game.start_Game(); set_AnimDirection(false); MenuAnimator.Play("PlaneMenuAppear"); }
    void returner() { Bridge.come_Back(); }
    void pauser() { Game.pause_Game(); set_AnimDirection(true); MenuAnimator.Play("PlaneMenuAppear"); }

    void set_Mode(bool which) 
    {
        Game.set_Mode(which);
        Plane.set_Mode(which);
    }

    void set_AnimDirection(bool back) 
    {
        if (back)
        {
            MenuAnimator["PlaneMenuAppear"].speed = -1;
            MenuAnimator["PlaneMenuAppear"].time = MenuAnimator["PlaneMenuAppear"].length;
        }
        else 
        {
            MenuAnimator["PlaneMenuAppear"].speed = 1;
            MenuAnimator["PlaneMenuAppear"].time = 0;
        }
    }
    void update_Difficulty(float val) { Game.Difficulty = (int)val; DiffVal.text = "Trudnoœæ:\n" + val.ToString(); }

    void update_Coin() 
    {
        AllAllCoins = Bridge.Vault.AllCoins;
        update_LeaderBoard();
    }

    void coin_Add() 
    {
        AllCoins += 1;
        AllAllCoins += 1;
        update_LeaderBoard();
        Bridge.Vault.AllCoins = AllAllCoins;
        Bridge.save_SaveData();
    }

    void update_LeaderBoard() 
    {
        LeaderBoard.text = LeaderBoardSamples[0] + AllCoins + LeaderBoardSamples[1] + AllAllCoins;
    }
}
