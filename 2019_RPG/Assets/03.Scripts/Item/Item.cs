using UnityEngine;

public enum ItemCategory{
    EquipmentItem,
    Potion,
    Materials,
    Heal,
    Gold,
    Exp
};

public class Item : ScriptableObject {

    new public string name = "NewItem";
    public Sprite icon = null;
    public bool isStoreAble;
    public ItemCategory category;

    public int amount;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
