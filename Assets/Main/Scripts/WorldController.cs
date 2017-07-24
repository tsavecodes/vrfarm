using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum GameState { WORLD }
public delegate void OnStateChangeHandler();

public class WorldController
{

    public int dollars = 100;
    public int water = 3;
    public int maxWater = 10;
    public List<Seed> seedInventory = new List<Seed>();

    protected WorldController() { }
    private static WorldController instance = null;
    public event OnStateChangeHandler OnWorldStateChange;
    public event OnStateChangeHandler OnInventoryChange;
    public event OnStateChangeHandler OnWaterChange;
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
        OnInventoryChange();
    }

    public Seed PopSeed()
    {
        if(seedInventory.Count < 1) {
            return null;
        }

        int i = seedInventory.Count - 1;

        Seed seed = seedInventory[i];
        seedInventory.RemoveAt(i);
        OnInventoryChange();
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

    public void AddWater(int amount)
    {
        water += amount;
        if(water > maxWater) { water = maxWater; }
        OnWaterChange();
    }
    public void UseWater()
    {
        if(water <= 0)
        {
            Debug.Log("No Water");
            return;
        }

        water--;
        OnWaterChange();
    }

    private void UpdateInventory()
    {
        OnInventoryChange();
    }

 


}
