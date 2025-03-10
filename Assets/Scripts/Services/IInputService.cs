using UnityEngine;
using UnityEngine.InputSystem;

public interface IInputService
{
    Vector2 GetMoveInput();
    bool IsJumpPressed();
    bool IsRunning();
    bool IsShooting();
    bool IsReloading();
    bool IsSwitchingWeapon();
    event System.Action PauseActionPerformed;
}

public class InputService : IInputService
{
    private PlayerInputActions _playerInput;
    public event System.Action PauseActionPerformed;

    public InputService()
    {
        _playerInput = new PlayerInputActions();
        _playerInput.Enable();
        _playerInput.UI.Pause.performed += ctx => PauseActionPerformed?.Invoke();
    }

    public Vector2 GetMoveInput()
    {
        return _playerInput.Player.Move.ReadValue<Vector2>();
    }

    public bool IsJumpPressed()
    {
        return _playerInput.Player.Jump.WasPressedThisFrame();
    }

    public bool IsRunning()
    {
        return _playerInput.Player.Run.IsPressed();
    }

    public bool IsShooting()
    {
        return _playerInput.Player.Shoot.WasPressedThisFrame();
    }

    public bool IsReloading()
    {
        return _playerInput.Player.Reload.WasPressedThisFrame();
    }

    public bool IsSwitchingWeapon()
    {
        return _playerInput.Player.SwitchWeapon.WasPressedThisFrame();
    }
}
