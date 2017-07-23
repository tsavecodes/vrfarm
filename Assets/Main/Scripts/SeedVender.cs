using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SeedVender : MonoBehaviour {

    WorldController WC;


    bool isInteractable = false;
    [SerializeField] private LayerMask layerMask;

    void Awake()
    {
        WC = WorldController.Instance;
        WC.OnStateChange += HandleOnStateChange;

        Debug.Log("Current game state when Awakes: " + WC.gameState);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (isInteractable && Input.GetKeyDown(KeyCode.E))
        {
            if (WC.CanAfford(10))
            {
                WC.SpendDollars(10);

                Inventory item = new Inventory();
                WC.Equip(item);
            }
            
        }


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

    public void HandleOnStateChange()
    {
        Debug.Log("OnStateChange!");
    }


}
