using UnityEngine;

public enum MaterialType
{
    Wood,
    Stone
}

[CreateAssetMenu(fileName = "New Weapon", menuName = "Inventory/MaterialItem")]
public class MaterialItem : Item
{
    public MaterialType materialType;
}

