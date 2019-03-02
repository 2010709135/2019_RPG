using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour {
    Inventory inventory;

	// Use this for initialization
	void Start () {
        inventory = CharacterInfo.instance._inventory;
        inventory.onItemChangedCallback += UpdateUI;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void UpdateUI()
    {

    }
}
