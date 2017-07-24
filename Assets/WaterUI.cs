using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterUI : MonoBehaviour {

    WorldController WC;
    public Slider slider;

    void Awake()
    {
        WC = WorldController.Instance;
        WC.OnWaterChange += HandleWaterChange;
    }

    // Use this for initialization
    void Start()
    {
        DrawWater();
    }

    // Update is called once per frame
    void Update () {
		
	}

    private void DrawWater()
    {
        slider.value = WC.water;
    }

    private void HandleWorldStateChange()
    {

    }

    private void HandleWaterChange()
    {
        DrawWater();
    }
}
