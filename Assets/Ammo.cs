using UnityEngine;

public class Ammo : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        WeaponSystem weaponSystem = other.GetComponent<WeaponSystem>();
        for (int i = 0; i < weaponSystem.weapons.Count; i++)
        {
            weaponSystem.weapons[i].InitializeAmmo();
        }
    }
}
