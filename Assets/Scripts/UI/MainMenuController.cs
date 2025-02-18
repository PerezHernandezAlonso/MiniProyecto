using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class MainMenuController : MonoBehaviour
{
    private PlayerInputActions inputActions;
    private UnityEngine.UI.Button[] buttons;
    private int selectedButtonIndex = 0;
    private bool canNavigate = true;

    void Awake()
    {
        inputActions = new PlayerInputActions();
        inputActions.UI.Navigate.performed += ctx => Navigate(ctx.ReadValue<Vector2>());
        inputActions.UI.Submit.performed += ctx => SelectButton();
        inputActions.UI.Cancel.performed += ctx => GoBack();
    }

    void OnEnable() => inputActions.Enable();
    void OnDisable() => inputActions.Disable();

    void Start()
    {
        buttons = GetComponentsInChildren<UnityEngine.UI.Button>();
        if (buttons.Length == 0)
        {
            Debug.LogError("No se encontraron botones en el menú principal.");
            return;
        }

        SetSelectedButton(0);
    }

    private void Navigate(Vector2 direction)
    {
        if (!canNavigate || buttons.Length == 0) return;

        int previousIndex = selectedButtonIndex;

        if (direction.y > 0.5f)
        {
            selectedButtonIndex = (selectedButtonIndex - 1 + buttons.Length) % buttons.Length;
        }
        else if (direction.y < -0.5f)
        {
            selectedButtonIndex = (selectedButtonIndex + 1) % buttons.Length;
        }

        if (previousIndex != selectedButtonIndex)
        {
            SetSelectedButton(selectedButtonIndex);
            canNavigate = false;
            Invoke(nameof(ResetNavigation), 0.5f);
        }
    }

    private void SetSelectedButton(int index)
    {
        if (buttons.Length == 0) return;

        // Solo seleccionar el botón si el ratón no ha interferido
        if (EventSystem.current.currentSelectedGameObject == null || EventSystem.current.currentInputModule.input.GetMouseButton(0))
        {
            var selectedButton = buttons[index];
            EventSystem.current?.SetSelectedGameObject(selectedButton.gameObject);
            selectedButton.Select();
        }
    }

    private void ResetNavigation()
    {
        canNavigate = true;
    }

    private void SelectButton()
    {
        if (buttons.Length > 0 && selectedButtonIndex >= 0 && selectedButtonIndex < buttons.Length)
        {
            var button = buttons[selectedButtonIndex];
            if (button != null && button.interactable)
            {
                button.onClick.Invoke();
            }
        }
    }

    private void GoBack()
    {
        Debug.Log("Volver al menú anterior o cerrar menú");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OpenOptions()
    {
        Debug.Log("Abriendo opciones...");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
