namespace Game.UI
{
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.InputSystem;

    public class MainMenuController : MenuController
    {
        protected override void Awake()
        {
            base.Awake();
            inputActions = GameManager.Singleton.PlayerInputActions;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            SetSelectedButton(0);
        }

        public void StartGame()
        {
            SceneManager.LoadScene("GameScene");
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
