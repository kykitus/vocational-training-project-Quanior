using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Unity.VisualScripting;

public class PlaneGame : MonoBehaviour
{

    public GameObject BlockArray;
    public GameObject Block;
    public GameObject Coin;
    public GameObject Tree;
    public AirPlane Plane;

    public Animation Mountain;
    public Animation Mountain2;
    public Animation Bush;
    public Animation Bush2;
    public Animation Trees;
    public Animation Trees2;
    public Animation Ground;

    public int SpawnDistance = 10000;
    public int Difficulty = 1;
    public int BlockWidth = 83;

    public Vector2 SpawnRange = new Vector3(900.0f, 470.0f);


    public UnityEvent Paused;
    public UnityEvent Resumed;

    UnityAction Ticker;
    UnityAction NeedDraw;
    UnityAction Speedster;

    Queue<Vector2> BlockDrawQueue = new Queue<Vector2>();

    long NextTwitch;
    long TreeTwitch;
    public int TimeInterval = 800;
    int Counter = 1;

    int TargetSpeed = 1;

    void Start()
    {
        Ticker = () => { };
        NeedDraw = () => { };
        Speedster = () => { };
        Plane.Died.AddListener(stop_Game);
    }

    void Update() { Ticker(); lower_Speed(); }

    public void start_Game() { Ticker = tick; NextTwitch = Environment.TickCount + get_BlockTime(); Resumed.Invoke(); Plane.start_Flying(); TargetSpeed = 1; }

    public void pause_Game() { Ticker = () => { }; Paused.Invoke(); TargetSpeed = 0; }

    public void stop_Game() { Ticker = () => { }; clear_Blocks(); return_Plane(); }

    void tick() 
    {
        NeedDraw();
        if (Environment.TickCount > TreeTwitch)
        {
            if (Environment.TickCount > NextTwitch)
            {
                choose_Danger();
                NextTwitch = Environment.TickCount + get_BlockTime();

            }
            draw_Tree();
            TreeTwitch = Environment.TickCount + 200;
        }

    }

    int get_BlockTime() 
    {
        return TimeInterval - Difficulty * 32;
    }

    void choose_Danger() 
    {
        int seed = Mathf.CeilToInt(UnityEngine.Random.Range(1f, Difficulty) / 4f);

        if (BlockDrawQueue.Count == 0) { NeedDraw = draw; }
        switch (seed) 
        {
            case 1:
                scatter_Block();
                break;
            case 2:
                scatter_LineAxial();
                break;
            case 3:
                scatter_Box();
                break;
            case 4:
                scatter_LineOblique();
                break;
            case 5:
                scatter_Block();
                break;
        }
    }

    void add_to_Queue(Vector2 pos) { BlockDrawQueue.Enqueue(pos);}

    Vector2 get_RandomPos() { return new Vector2(UnityEngine.Random.Range(-SpawnRange.x, SpawnRange.x), UnityEngine.Random.Range(-SpawnRange.y, SpawnRange.y)); }

    void scatter_Block() { add_to_Queue(get_RandomPos()); }

    void scatter_LineAxial() 
    {
        if (UnityEngine.Random.Range(0, 2) == 1)
        {
            Vector2 pos_base = get_RandomPos() - new Vector2(BlockWidth * 4, 0f);
            foreach (int n in Enumerable.Range(0, 5))
            {
                add_to_Queue(new Vector2(pos_base.x + n * 2 * BlockWidth, pos_base.y));
            }
        }
        else 
        {
            Vector2 pos_base = get_RandomPos() - new Vector2(0f, BlockWidth * 4);
            foreach (int n in Enumerable.Range(0, 5))
            {
                add_to_Queue(new Vector2(pos_base.x, pos_base.y + n * 2 * BlockWidth));
            }
        }
    }

    void scatter_LineOblique() 
    {
        int sign = 1 - 2 * UnityEngine.Random.Range(0, 2);
        Vector2 pos_base = get_RandomPos() - new Vector2(BlockWidth * 4 * sign, BlockWidth * 4);
        foreach (int n in Enumerable.Range(0, 5))
        {
            add_to_Queue(new Vector2(pos_base.x + n * BlockWidth * 2 * -sign, pos_base.y + BlockWidth * 2 * n));
        }
    }

    void scatter_Box() 
    {
        Vector2 pos_base = get_RandomPos();
        foreach (int n in Enumerable.Range(0, 3))
        {
            foreach (int m in Enumerable.Range(0, 3))
            {
                add_to_Queue(new Vector2(pos_base.x + 2 * BlockWidth * n, pos_base.y + 2 * BlockWidth * m));
            }
        }
    }

    void draw() 
    {
        Vector2 pos = BlockDrawQueue.Dequeue();
        draw_Block(pos, Block);
        if (Counter % 10 == 0) { draw_Block(get_RandomPos(), Coin); }
        Counter++;
    }

    void draw_Block(Vector2 pos, GameObject prefab) 
    {
        GameObject new_block = Instantiate(prefab, BlockArray.transform);
        new_block.transform.position = new Vector3(pos.x, pos.y, SpawnDistance);
        new_block.GetComponent<FlyingBlock>().init(this);
        if (BlockDrawQueue.Count == 0) { NeedDraw = () => { }; }
    }

    void draw_Tree() 
    {
        draw_Block(new Vector2(UnityEngine.Random.Range(-SpawnRange.x - 1600f, SpawnRange.x + 1600f), -2084f), Tree);
    }

    void clear_Blocks() { foreach (Transform block in BlockArray.transform) { Destroy(block.gameObject); } }

    void return_Plane()
    {
        Plane.transform.position = Vector3.zero;
    }

    void lower_Speed() 
    {
        if (TargetSpeed != Mountain["SkyMove"].speed) 
        {
            Mountain["SkyMove"].speed = Mathf.Lerp(Mountain["SkyMove"].speed, TargetSpeed, 0.02f);
            Mountain2["SkyMove"].speed = Mathf.Lerp(Mountain2["SkyMove"].speed, TargetSpeed, 0.02f);
            Bush["ddd"].speed = Mathf.Lerp(Bush["ddd"].speed, TargetSpeed, 0.02f);
            Bush2["ddd"].speed = Mathf.Lerp(Bush2["ddd"].speed, TargetSpeed, 0.02f);
            Trees["ddd"].speed = Mathf.Lerp(Trees["ddd"].speed, TargetSpeed, 0.02f);
            Trees2["ddd"].speed = Mathf.Lerp(Trees2["ddd"].speed, TargetSpeed, 0.02f);
            Ground["BushMove"].speed = Mathf.Lerp(Ground["BushMove"].speed, TargetSpeed, 0.02f);
        }
    }
}
