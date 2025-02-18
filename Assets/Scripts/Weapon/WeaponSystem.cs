
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class WeaponSystem : MonoBehaviour
{
    [Header("Weapon Management")]
    public List<Weapon> weapons = new List<Weapon>();
    public int currentWeaponIndex = 0;

    private bool isReloading = false;
    private float nextFireTime = 0f;
    private PlayerInputActions inputActions;

    [Header("Fire Point")]
    public Transform firePoint;

    private PlayerUIController uiController;

    void Awake()
    {
        inputActions = new PlayerInputActions();
        inputActions.Player.Shoot.performed += ctx => Shoot();
        inputActions.Player.Reload.performed += ctx => StartCoroutine(Reload());
        inputActions.Player.SwitchWeapon.performed += ctx => SwitchWeapon();

        uiController = GetComponent<PlayerUIController>();

        if (weapons.Count > 0)
        {
            foreach (var weapon in weapons)
            {
                weapon.InitializeAmmo();
                weapon.additionalAmmo = weapon.initialAdditionalAmmo;
            }
        }
    }

    void OnEnable() => inputActions.Enable();
    void OnDisable() => inputActions.Disable();

    void Update()
    {
        if (weapons.Count == 0) return;

        Weapon currentWeapon = weapons[currentWeaponIndex];

        if (currentWeapon.isAutomatic && inputActions.Player.Shoot.IsPressed() && Time.time >= nextFireTime)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (isReloading || weapons.Count == 0) return;

        Weapon currentWeapon = weapons[currentWeaponIndex];
        if (Time.time < nextFireTime) return;

        if (currentWeapon.currentAmmo <= 0)
        {
            uiController.UpdateNotification("Sin munición. Recarga primero.");
            return;
        }

        nextFireTime = Time.time + currentWeapon.fireRate;

        Instantiate(currentWeapon.bulletPrefab, firePoint.position, firePoint.rotation);
        currentWeapon.currentAmmo--;
        uiController.UpdateNotification($"Disparo con: {currentWeapon.weaponName}. Munición: {currentWeapon.currentAmmo}/{currentWeapon.additionalAmmo}");
    }

    IEnumerator Reload()
    {
        if (isReloading || weapons.Count == 0) yield break;

        isReloading = true;
        uiController.UpdateNotification("Recargando...");
        yield return new WaitForSeconds(1.5f);

        Weapon currentWeapon = weapons[currentWeaponIndex];
        currentWeapon.Reload();

        uiController.UpdateNotification($"Recarga completa. Munición: {currentWeapon.currentAmmo}/{currentWeapon.additionalAmmo}");
        isReloading = false;
    }

    void SwitchWeapon()
    {
        if (weapons.Count < 2) return;

        currentWeaponIndex = (currentWeaponIndex + 1) % weapons.Count;
        uiController.UpdateNotification("Arma cambiada a: " + weapons[currentWeaponIndex].weaponName);
    }

    public void AddWeapon(Weapon newWeapon)
    {
        if (!weapons.Contains(newWeapon))
        {
            weapons.Add(newWeapon);
            newWeapon.InitializeAmmo();
            newWeapon.additionalAmmo = newWeapon.initialAdditionalAmmo;
            uiController.UpdateNotification("Arma añadida: " + newWeapon.weaponName);
        }
    }

    public void RemoveWeapon(string weaponName)
    {
        Weapon weaponToRemove = weapons.Find(w => w.weaponName == weaponName);
        if (weaponToRemove != null)
        {
            weapons.Remove(weaponToRemove);
            uiController.UpdateNotification("Arma eliminada: " + weaponName);
        }
    }
}
