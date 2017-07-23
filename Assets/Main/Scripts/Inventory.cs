using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

public class Seed : Inventory
{

    public enum Kind
    {
        Corn,
        Wheat,
        Carrot,
        Tomato,
        Potato
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

}

public class Tool : Inventory
{

    public enum Kind
    {
        WateringCan,
        Hoe
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

}