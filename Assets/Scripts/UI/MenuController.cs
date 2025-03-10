namespace Game.UI
{
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.InputSystem;
    using UnityEngine.EventSystems;

    public class MenuController : MonoBehaviour
    {
        protected PlayerInputActions inputActions;
        protected int selectedButtonIndex = 0;
        protected UnityEngine.UI.Button[] buttons;
        protected bool canNavigate = true;
        private float navigationCooldown = 0.2f;
        private float lastNavigationTime = 0f;
        private bool navigationPressed = false;

        protected virtual void Awake()
        {
            inputActions = GameManager.Singleton.PlayerInputActions;
        }

        protected virtual void OnEnable()
        {
            buttons = GetComponentsInChildren<UnityEngine.UI.Button>(true);
            if (buttons.Length > 0)
            {
                SetSelectedButton(0);
            }
        }

        protected virtual void Update()
        {
            if (!navigationPressed || Time.time - lastNavigationTime < navigationCooldown) return;

            Vector2 navigationInput = inputActions.UI.Navigate.ReadValue<Vector2>();
            if (canNavigate)
            {
                if (navigationInput.y > 0.5f)
                {
                    ChangeSelection(-1);
                }
                else if (navigationInput.y < -0.5f)
                {
                    ChangeSelection(1);
                }
            }

            lastNavigationTime = Time.time;
        }

        private void ChangeSelection(int direction)
        {
            if (buttons == null || buttons.Length == 0) return;

            selectedButtonIndex = (selectedButtonIndex + direction + buttons.Length) % buttons.Length;
            SetSelectedButton(selectedButtonIndex);
        }

        protected void SetSelectedButton(int index)
        {
            if (buttons == null || buttons.Length == 0 || EventSystem.current == null) return;

            selectedButtonIndex = index;
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(buttons[index].gameObject);
            buttons[index].Select();
        }

        protected void SelectButton()
        {
            if (buttons != null && buttons.Length > 0 && selectedButtonIndex >= 0 && selectedButtonIndex < buttons.Length)
            {
                GameObject currentSelected = EventSystem.current.currentSelectedGameObject;
                if (currentSelected == buttons[selectedButtonIndex].gameObject && currentSelected.activeInHierarchy)
                {
                    buttons[selectedButtonIndex].onClick.Invoke();
                }
            }
        }

        protected virtual void GoBack()
        {
            Debug.Log("Volver al menú anterior o cerrar menú");
        }
    }
}
