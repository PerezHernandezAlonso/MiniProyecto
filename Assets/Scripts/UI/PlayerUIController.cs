using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUIController : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text ammoText;
    public Slider healthBar;
    public TMP_Text notificationText;

    private WeaponSystem weaponSystem;
    private GameManager gameManager;
    private PlayerHealth playerHealth;

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        weaponSystem = GetComponent<WeaponSystem>();
        playerHealth = GetComponent<PlayerHealth>();
        GameManager.Singleton.Player = playerHealth;

        if (weaponSystem == null) Debug.LogError("WeaponSystem no encontrado en el GameObject actual");
        if (playerHealth == null) Debug.LogError("PlayerHealth no encontrado en el GameObject actual");
        if (healthBar == null) Debug.LogError("HealthBar no encontrado en el GameObject actual");
    }

    void Update()
    {
        UpdateAmmoDisplay();
        UpdateHealthDisplay();
    }

  

    void UpdateAmmoDisplay()
    {
        if (weaponSystem.weapons.Count == 0) return;

        var currentWeapon = weaponSystem.weapons[weaponSystem.currentWeaponIndex];

        ammoText.text = $"{currentWeapon.currentAmmo} / {currentWeapon.additionalAmmo}";
    }

    void UpdateHealthDisplay()
    {
        healthBar.value = GameManager.Singleton.Player.GetCurrentHealthPercentage();
    }

    public void UpdateNotification(string message)
    {
        notificationText.text = message;
    }
}