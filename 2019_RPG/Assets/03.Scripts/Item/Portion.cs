using UnityEngine;

public enum PortionType
{
    HealPortion,
    StaminaPortion
}

[CreateAssetMenu(fileName = "New Weapon", menuName = "Inventory/Portion")]
public class Portion : Item
{
    public PortionType portionType;
    public int amount;
}

