using UnityEngine;

public enum WeaponType
{
    Knight,
    Mage,
    TwoHanded
}

[CreateAssetMenu(fileName = "New Weapon", menuName = "Inventory/Weapon")]
public class Weapon : Item {
    public WeaponType weaponType;
    public int Damage;
}
