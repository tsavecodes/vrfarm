using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum GameState { WORLD }
public delegate void OnStateChangeHandler();

public class WorldController
{

    private int dollars = 100;
    private List<Seed> seedInventory = new List<Seed>();

    protected WorldController() { }
    private static WorldController instance = null;
    public event OnStateChangeHandler OnWorldStateChange;
    public GameState gameState { get; private set; }

    public static WorldController Instance
    {
        get
        {
            if (WorldController.instance == null)
            {
                //DontDestroyOnLoad(WorldController.instance);
                WorldController.instance = new WorldController();
            }
            return WorldController.instance;
        }

    }

    public void SetGameState(GameState state)
    {
        this.gameState = state;
        OnWorldStateChange();
    }

    public void OnApplicationQuit()
    {
        WorldController.instance = null;
    }

    public void EquipSeed(Seed seed)
    {
        seedInventory.Add(seed);
        Debug.Log(seedInventory.Count);
    }

    public Seed PopSeed()
    {
        if(seedInventory.Count < 1) {
            return null;
        }

        int i = seedInventory.Count - 1;

        Seed seed = seedInventory[i];
        seedInventory.RemoveAt(i);
        return seed;

    }

    public bool CanAfford(int amount)
    {
        if (amount > dollars) { return false; }
        return true;
    }

    public int SpendDollars(int amount)
    {
        if (!CanAfford(amount))
        {
            Debug.Log("Can't afford that");
            return -1;
        }

        dollars -= amount;
        Debug.Log("Dollars:" + dollars);
        return dollars;
    }

    public int EarnDollars(int amount)
    {
        dollars += amount;
        return dollars;
    }


}
