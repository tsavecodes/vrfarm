using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterWell : MonoBehaviour {

    WorldController WC;
    bool isInteractable = false;
    [SerializeField] private LayerMask layerMask;


    void Awake()
    {
        WC = WorldController.Instance;
        WC.OnWorldStateChange += HandleWorldStateChange;

    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (isInteractable && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("getting water");
            WC.AddWater(5);         
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

    private void HandleWorldStateChange()
    {
        return;
    }

    
}
