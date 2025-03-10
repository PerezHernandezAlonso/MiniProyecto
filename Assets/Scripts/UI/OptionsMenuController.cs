namespace Game.UI
{
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.InputSystem;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class OptionsMenuController : MenuController
    {
        [Header("UI Elements")]
        public Slider volumeSlider;
        public Slider sensitivitySlider;
        public TMPro.TMP_Dropdown resolutionDropdown;
        public Toggle fullscreenToggle;
        public Button backButton;

        private Resolution[] resolutions;

        protected override void Awake()
        {
            base.Awake();
            LoadSettings();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            PopulateResolutions();
            SetSelectedButton();
        }

        private void PopulateResolutions()
        {
            resolutions = Screen.resolutions;
            resolutionDropdown.ClearOptions();

            int currentResolutionIndex = 0;
            for (int i = 0; i < resolutions.Length; i++)
            {
                string resolutionString = resolutions[i].width + "x" + resolutions[i].height;
                resolutionDropdown.options.Add(new TMPro.TMP_Dropdown.OptionData(resolutionString));
                if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = i;
                }
            }
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();
        }

        public void SetVolume(float volume)
        {
            AudioListener.volume = volume;
            PlayerPrefs.SetFloat("Volume", volume);
        }

        public void SetSensitivity(float sensitivity)
        {
            PlayerPrefs.SetFloat("Sensitivity", sensitivity);
        }

        public void SetResolution(int resolutionIndex)
        {
            Resolution resolution = resolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
            PlayerPrefs.SetInt("Resolution", resolutionIndex);
        }

        public void SetFullscreen(bool isFullscreen)
        {
            Screen.fullScreen = isFullscreen;
            PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
        }

        private void LoadSettings()
        {
            volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1f);
            sensitivitySlider.value = PlayerPrefs.GetFloat("Sensitivity", 1f);
            fullscreenToggle.isOn = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        }

        private void SetSelectedButton()
        {
            if (backButton != null)
            {
                EventSystem.current.SetSelectedGameObject(backButton.gameObject);
            }
        }

        public void BackToMainMenu()
        {
            this.gameObject.SetActive(false);
            Transform pauseMenu = transform.parent.Find("PauseMenuCanvas");
            if (pauseMenu != null)
            {
                pauseMenu.gameObject.SetActive(true);
                EventSystem.current.SetSelectedGameObject(pauseMenu.GetComponentInChildren<Button>().gameObject);
            }
            this.gameObject.SetActive(false);
            Transform mainMenu = transform.parent.Find("MainMenuCanvas");
            if (mainMenu != null)
            {
                mainMenu.gameObject.SetActive(true);
                EventSystem.current.SetSelectedGameObject(mainMenu.GetComponentInChildren<Button>().gameObject);
            }
}
    }
}


