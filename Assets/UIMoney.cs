using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMoney : MonoBehaviour {

    WorldController WC;
    Text txt;

    void Awake()
    {
        WC = WorldController.Instance;
        WC.OnWorldStateChange += HandleWorldStateChange;

    }

    // Use this for initialization
    void Start () {
        txt = gameObject.GetComponent<Text>();
        txt.text = "$" + WC.dollars;
    }
	
	// Update is called once per frame
	void Update () {

        txt.text = "$" + WC.dollars;

    }

    private void HandleWorldStateChange()
    {

    }
}
