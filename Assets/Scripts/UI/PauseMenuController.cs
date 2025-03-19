namespace Game.UI
{
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.InputSystem;
    using UnityEngine.EventSystems;

    public class PauseMenuController : MenuController
    {
        public GameObject pauseMenuUI;
        private bool isPaused = false;

        protected override void Awake()
        {
            base.Awake();
            inputActions = GameManager.Singleton.PlayerInputActions;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            if (inputActions != null)
            {
                inputActions.UI.Pause.performed += TogglePause;
            }
            SetSelectedButton(0);
            inputActions.Player.Enable();
        }

        protected virtual void OnDisable()
        {
            if (inputActions != null)
            {
                inputActions.UI.Pause.performed -= TogglePause;
            }
        }

        private void TogglePause(InputAction.CallbackContext context)
        {
            isPaused = !isPaused;
            pauseMenuUI.SetActive(isPaused);
            Time.timeScale = isPaused ? 0f : 1f;
            Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = isPaused;
            
            if (isPaused)
            {
                if (buttons == null || buttons.Length == 0)
                {
                    buttons = pauseMenuUI.GetComponentsInChildren<UnityEngine.UI.Button>(true);
                }
                if (buttons.Length > 0)
                {
                    SetSelectedButton(0);
                }
                canNavigate = false;
                inputActions.Player.Disable();
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(null);
                canNavigate = true;
                inputActions.Player.Enable();
            }
        }

        public void ResumeGame()
        {
            isPaused = false;
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            inputActions.Player.Enable();
        }

        public void OpenOptions()
        {
            Debug.Log("Abrir opciones desde el menú de pausa");
        }

        public void ReturnToMainMenu()
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene("MenuPrincipal");
        }
    }
}
