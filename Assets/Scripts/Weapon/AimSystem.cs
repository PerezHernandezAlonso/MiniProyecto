using UnityEngine;
using Unity.Cinemachine;

public class AimSystem : MonoBehaviour
{
    [Header("References")]
    public CinemachineCamera aimCamera; // Cámara de apuntado
    public Transform firePoint; // Orbe flotante desde donde se disparan las balas
    public Transform playerTransform; // Referencia al jugador
    public LayerMask aimLayerMask; // Capas con las que interactúa el raycast

    [Header("Settings")]
    public float aimDistance = 2f; // Distancia fija frente al jugador
    public float smoothSpeed = 10f; // Velocidad de suavizado de posición
    public float rotationSpeed = 10f; // Velocidad de suavizado de rotación
    public float minHeightOffset = 1.5f; // Mínima altura sobre el suelo

    private PlayerInputActions inputActions;
    private bool isAiming = false;
    private Vector3 originalFirePointPosition; // Guarda la posición inicial del orbe
    private Quaternion originalFirePointRotation; // Guarda la rotación inicial del orbe

    void Awake()
    {
        inputActions = GameManager.Singleton.PlayerInputActions;
        inputActions.Player.Aim.performed += ctx => StartAiming();
        inputActions.Player.Aim.canceled += ctx => StopAiming();
    }

    void Start()
    {
        originalFirePointPosition = firePoint.localPosition; // Guarda la posición inicial relativa al jugador
        originalFirePointRotation = firePoint.localRotation; // Guarda la rotación original relativa al jugador
    }

    void OnEnable() => inputActions.Enable();

    void OnDisable()
    {
        inputActions.Disable();
        StopAiming(); // Asegura que el orbe vuelve a su posición al deshabilitarse
    }

    void Update()
    {
        if (isAiming)
        {
            UpdateFirePointPositionAndRotation();
        }
        else
        {
            ResetFirePointPositionAndRotation();
        }
    }

    void StartAiming()
    {
        isAiming = true;
        if (aimCamera != null)
        {
            aimCamera.Priority = 20; // Asegurar que la cámara de apuntado sea prioritaria
        }
    }

    void StopAiming()
    {
        isAiming = false;
        if (aimCamera != null)
        {
            aimCamera.Priority = 0; // Volver a la cámara normal
        }
    }

    void UpdateFirePointPositionAndRotation()
    {
        Ray ray = new Ray(aimCamera.transform.position, aimCamera.transform.forward);
        RaycastHit hit;

        Vector3 newPosition;
        Vector3 targetDirection;

        if (Physics.Raycast(ray, out hit, 100f, aimLayerMask))
        {
            // Si el raycast golpea algo, el firePoint se coloca al frente del jugador, pero rotará hacia el punto de impacto
            Vector3 direction = (hit.point - playerTransform.position).normalized;
            newPosition = playerTransform.position + new Vector3(direction.x, 0, direction.z) * aimDistance;

            // La dirección de la rotación será hacia el punto de impacto
            targetDirection = (hit.point - firePoint.position).normalized;
        }
        else
        {
            // Si no golpea nada, mover el firePoint al frente del jugador en la dirección de la cámara
            Vector3 direction = aimCamera.transform.forward.normalized;
            newPosition = playerTransform.position + new Vector3(direction.x, 0, direction.z) * aimDistance;

            // La dirección de la rotación será hacia adelante en la dirección de la cámara
            targetDirection = aimCamera.transform.forward;
        }

        // Ajustar la altura para evitar que baje demasiado
        newPosition.y = Mathf.Max(playerTransform.position.y + minHeightOffset, newPosition.y);

        // Aplicamos suavizado para la posición
        firePoint.position = Vector3.Lerp(firePoint.position, newPosition, Time.deltaTime * smoothSpeed);

        // Solo aplicar rotación cuando se está apuntando
        if (targetDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            firePoint.rotation = Quaternion.Slerp(firePoint.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    void ResetFirePointPositionAndRotation()
    {
        // Suavizar el regreso a la posición original relativa al jugador
        firePoint.localPosition = Vector3.Lerp(firePoint.localPosition, originalFirePointPosition, Time.deltaTime * smoothSpeed);

        // Volver a la rotación original relativa al jugador
        firePoint.localRotation = Quaternion.Slerp(firePoint.localRotation, originalFirePointRotation, Time.deltaTime * rotationSpeed);
    }

    public Vector3 GetShootDirection()
    {
        if (isAiming && aimCamera != null)
        {
            return firePoint.forward; // Dispara en la dirección del firePoint cuando se apunta
        }
        else
        {
            return playerTransform.forward; // Cuando no se apunta, dispara en la dirección del jugador
        }
    }
}