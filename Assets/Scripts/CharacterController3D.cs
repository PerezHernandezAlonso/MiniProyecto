using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;
using System.ComponentModel.Design.Serialization;
using System;


public class CharacterController3D : MonoBehaviour
{
    public float jumpForce = 5f;
    public float gravity = 9.81f;
    public float rotationSpeed = 10f; // Velocidad de rotación del personaje

    private CharacterController characterController;
    private Animator animator;
    private Vector3 playerVelocity;
    private bool isGrounded;
    private bool jumpInput;
    private bool isRunning;

    private PlayerInputActions inputActions;
    private Vector2 moveInput;

    [Header("Camera Reference")]
    public CinemachineCamera cinemachineCamera; // Referencia a la cámara de Cinemachine
    private Transform cameraTransform; // Transform de la cámara para obtener la dirección
    
    

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        inputActions = new PlayerInputActions();

        // Leer Input del Movimiento
        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        // Leer Input de Salto
        inputActions.Player.Jump.performed += ctx => jumpInput = true;

        // Leer Input de Sprint (Shift Izquierdo)
        inputActions.Player.Run.performed += ctx => isRunning = true;
        inputActions.Player.Run.canceled += ctx => isRunning = false;
    }

    void Start()
    {
        if (cinemachineCamera != null)
        {
            cameraTransform = cinemachineCamera.transform;
        }
    }

    void OnEnable() => inputActions.Enable();
    void OnDisable() => inputActions.Disable();

    void Update()
    {
        isGrounded = characterController.isGrounded;

        // Obtener la dirección del movimiento relativo a la cámara
        Vector3 moveDirection = Vector3.zero;

        if (moveInput.magnitude > 0.1f)
        {
            Vector3 forward = cameraTransform.forward;
            Vector3 right = cameraTransform.right;

            // Asegurar que forward y right están en un solo plano (horizontal)
            forward.y = 0;
            right.y = 0;
            forward.Normalize();
            right.Normalize();

            // Calcular la dirección de movimiento
            moveDirection = forward * moveInput.y + right * moveInput.x;
            moveDirection.Normalize();

            // Rotar el personaje en la dirección de movimiento
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Aplicar Root Motion para movimiento
        if (animator.applyRootMotion)
        {
            Vector3 rootMotionMovement = animator.deltaPosition;
            characterController.Move(rootMotionMovement);
        }

        // Actualizar animaciones de movimiento y correr
        animator.SetBool("Moving", moveInput.magnitude > 0.1f);
        animator.SetBool("Running", isRunning);

        // Saltar si está en el suelo
        if (jumpInput && isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpForce * 2f * gravity);
            animator.SetTrigger("Jump"); // Activar animación de salto
        }

        // Aplicar gravedad
        playerVelocity.y -= gravity * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);

        // Resetear el salto después de aplicarlo
        jumpInput = false;
    }
}