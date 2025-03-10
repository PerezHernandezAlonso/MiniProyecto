
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;

public class WeaponSystem : MonoBehaviour
{
    [Header("Weapon Management")]
    public List<Weapon> weapons = new List<Weapon>();
    public int currentWeaponIndex = 0;

    private bool isReloading = false;
    private float nextFireTime = 0f;
    private PlayerInputActions inputActions;

    [Header("Fire Point")]
    public Transform firePoint; // El punto donde aparecen las balas
    public CinemachineCamera aimCamera; // Referencia a la cámara de apuntado

    private PlayerUIController uiController;
    private bool isAiming = false;

    void Awake()
    {
        inputActions = GameManager.Singleton.PlayerInputActions;
        inputActions.Player.Shoot.performed += ctx => Shoot();
        inputActions.Player.Reload.performed += ctx => StartCoroutine(Reload());
        inputActions.Player.SwitchWeapon.performed += ctx => SwitchWeapon();
        inputActions.Player.Aim.performed += ctx => isAiming = true;
        inputActions.Player.Aim.canceled += ctx => isAiming = false;

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
        if (Time.deltaTime > 0.0f)
        {
            if (weapons.Count == 0) return;

            Weapon currentWeapon = weapons[currentWeaponIndex];

            if (currentWeapon.isAutomatic && inputActions.Player.Shoot.IsPressed() && Time.time >= nextFireTime)
            {
                Shoot();
            }
        }
    }

    void Shoot()
    {
        if (CheckPause()) return;
        if (isReloading || weapons.Count == 0) return;

        Weapon currentWeapon = weapons[currentWeaponIndex];
        if (Time.time < nextFireTime) return;

        if (currentWeapon.currentAmmo <= 0)
        {
            uiController.UpdateNotification("Sin munición. Recarga primero.");
            return;
        }

        nextFireTime = Time.time + currentWeapon.fireRate;

        // Obtener la dirección correcta del disparo
        Vector3 shootDirection = GetShootDirection();

        GameObject bullet = Instantiate(currentWeapon.bulletPrefab, firePoint.position, Quaternion.LookRotation(shootDirection));

        currentWeapon.currentAmmo--;
        uiController.UpdateNotification($"Disparo con: {currentWeapon.weaponName}. Munición: {currentWeapon.currentAmmo}/{currentWeapon.additionalAmmo}");
    }

    Vector3 GetShootDirection()
    {
        if (isAiming && aimCamera != null)
        {
            // Dispara hacia donde está mirando la cámara de apuntado
            return aimCamera.transform.forward;
        }
        else
        {
            // Dispara hacia adelante desde el firePoint
            return firePoint.forward;
        }
    }

    IEnumerator Reload()
    {
        if (CheckPause()) yield break;
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
        if (CheckPause()) return;
        if (weapons.Count < 2) return;

        currentWeaponIndex = (currentWeaponIndex + 1) % weapons.Count;
        uiController.UpdateNotification("Arma cambiada a: " + weapons[currentWeaponIndex].weaponName);
    }

    public bool CheckPause()
    {
        return Time.deltaTime <= 0.0f;
    }
}
