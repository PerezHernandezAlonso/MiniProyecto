using System.Collections;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapons/Weapon")]
public class Weapon : ScriptableObject
{
    public string weaponName;
    public GameObject bulletPrefab;
    public float fireRate;
    public int maxAmmo;
    public int currentAmmo;
    public int additionalAmmo;
    public int initialAdditionalAmmo;
    public bool isAutomatic;
    public float timeToShoot = 0f;
    public bool canShoot = true;
    

    public void InitializeAmmo()
    {
        currentAmmo = maxAmmo;
        additionalAmmo = initialAdditionalAmmo;
    }

    public void Reload()
    {
        int neededAmmo = maxAmmo - currentAmmo;
        int ammoToReload = Mathf.Min(neededAmmo, additionalAmmo);

        currentAmmo += ammoToReload;
        additionalAmmo -= ammoToReload;
    }




}
