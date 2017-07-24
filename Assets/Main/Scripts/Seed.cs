using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Seed : MonoBehaviour
{

    WorldController WC;
    bool isInteractable = false;
    [SerializeField] private LayerMask layerMask;

    // ------------------
    public enum SeedType
    {
        Corn = 0,
        Wheat = 1,
        Carrot = 2,
        Tomato = 3,
        Potato = 4
    }

    public SeedType _type;
    public SeedType Type
    {
        get { return _type; }
        private set { _type = value; }
    }

    public int TotalTypes
    {
        get { return Enum.GetNames(typeof(SeedType)).Length; }
    }
    // ------------------

    public enum SeedState
    {
        ForSale = 0,
        InInventory = 1,
        InHand = 2,
        Planted = 3,
        Discarded = 4,
    }

    public SeedState _state;
    public SeedState State
    {
        get { return _state; }
        private set { _state = value; }
    }

    public int TotalStates
    {
        get { return Enum.GetNames(typeof(SeedState)).Length; }
    }
    // ------------------


    [SerializeField] private int[] prices;
    [SerializeField] private Color[] colors;

    public int amount = 100;


    void Awake()
    {
        WC = WorldController.Instance;
        WC.OnWorldStateChange += HandleWorldStateChange;

    }

    void Start()
    {
        GetComponent<MeshRenderer>().material.SetColor("_Color", GetColor());
        UpdateSeedState(SeedState.ForSale);
    }

    void Update()
    {

        switch(State)
        {
            case SeedState.ForSale:

                if (isInteractable && Input.GetKeyDown(KeyCode.E))
                {
                    int price = GetPrice();
                    if (WC.CanAfford(price))
                    {
                        WC.SpendDollars(price);
                        WC.EquipSeed(this);

                        UpdateSeedState(SeedState.InInventory);
                    }
                }

                break;
            case SeedState.InInventory:

                break;
            case SeedState.InHand:

                break;
            case SeedState.Planted:

                break;
            case SeedState.Discarded:

                break;
        }
    }

    public void UpdateSeedState(SeedState state)
    {
        State = state;

        switch (State)
        {
            case SeedState.ForSale:
                gameObject.SetActive(true);

                break;
            case SeedState.InInventory:
                gameObject.SetActive(false);

                break;
            case SeedState.InHand:
                gameObject.SetActive(true);

                break;
            case SeedState.Planted:

                break;
            case SeedState.Discarded:
                gameObject.SetActive(true);

                break;
        }

    }

    public int GetPrice()
    {
        return prices[(int)Type];
    }

    public Color GetColor()
    {
        return colors[(int)Type];
    }

    public string GetSeedName()
    {
        switch(Type)
        {
            case SeedType.Corn:
                return "Corn";
            case SeedType.Carrot:
                return "Carrot";
            case SeedType.Wheat:
                return "Wheat";
            case SeedType.Potato:
                return "Potato";
            case SeedType.Tomato:
                return "Tomato";

        }

        return "?";
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!ValidLayer(other)) { return; }
        isInteractable = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!ValidLayer(other)) { return; }
        isInteractable = false;
    }

    private bool ValidLayer(Collider other)
    {
        int layer = other.gameObject.layer;
        return (layerMask == (layerMask | (1 << layer)));
    }

    private void HandleWorldStateChange()
    {
        return;
    }



}