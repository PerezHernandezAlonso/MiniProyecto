using UnityEngine;

public class WeaponAdd : MonoBehaviour
{
    public Weapon weapon;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            WeaponSystem weaponSystem = other.GetComponent<WeaponSystem>();
            weaponSystem.AddWeapon(weapon);
            Destroy(this.gameObject);
        }
    }
}
