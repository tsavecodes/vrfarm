using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour {

    WorldController WC;
    public GameObject prefabItem;
    public RectTransform ParentPanel;

    private List<GameObject> seedButtons = new List<GameObject>(); 

    void Awake()
    {
        WC = WorldController.Instance;
        WC.OnWorldStateChange += HandleWorldStateChange;
        WC.OnInventoryChange += HandleInventoryStateChange;

    }

    // Use this for initialization
    void Start () {

        

        

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void DrawInventory()
    {

        for(int i=0; i<seedButtons.Count; i++)
        {
            GameObject seedButton = seedButtons[i];
            Destroy(seedButton);
        }

        for (int i = 0; i < WC.seedInventory.Count; i++)
        {
            Seed seed = WC.seedInventory[i];

            GameObject itemButton = (GameObject)Instantiate(prefabItem);
            itemButton.transform.SetParent(ParentPanel, false);
            itemButton.transform.localScale = new Vector2(1, 1);

            int x = -(i * 90) - 45;
            Debug.Log("X: " + x);


            RectTransform rectTransform = itemButton.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(0, 1);

            itemButton.transform.localPosition = new Vector2(x, 20);
            itemButton.GetComponent<Button>().GetComponentInChildren<Text>().text = seed.GetSeedName();
  
            seedButtons.Add(itemButton);

        }
    }

    void ButtonClicked(int i)
    {
        Debug.Log("Button clicked = " + i);
    }

    private void HandleInventoryStateChange()
    {
        DrawInventory();
    }

    private void HandleWorldStateChange()
    {

    }
}
