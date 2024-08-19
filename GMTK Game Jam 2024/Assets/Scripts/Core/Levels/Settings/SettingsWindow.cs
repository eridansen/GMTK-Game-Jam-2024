using System;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Levels.Settings
{
    public class SettingsWindow : MonoBehaviour
    {
        [SerializeField] private RawImage _background;
        [SerializeField] private Button _mainMenuButton;
        [SerializeField] private Slider _musicVolumeSlider;
        [SerializeField] private Slider _sfxVolumeSlider;
        private SettingsController _levelController;

        public void Initialize(SettingsController levelController)
        {
            _levelController = levelController;
        }
        
        private void Awake()
        {
            _mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
            _musicVolumeSlider.onValueChanged.AddListener(SaveMusicValue);
            _sfxVolumeSlider.onValueChanged.AddListener(SaveSfxValue);

            LoadSliderValues();
            SetBackgroundPosition();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnMainMenuButtonClicked();
            }
        }

        private void LoadSliderValues()
        {
            _musicVolumeSlider.value = AudioManager.Instance.GetMusicVolume();
            _sfxVolumeSlider.value = AudioManager.Instance.GetSfxVolume();
        }
        
        private void SetBackgroundPosition()
        {
            Vector2 position = new Vector2(SaveModule.Instance.LoadBackgroundPositionX(), SaveModule.Instance.LoadBackgroundPositionY());
            _background.uvRect = new Rect(position, _background.uvRect.size);
        }

        private void SaveMusicValue(float value)
        {
            AudioManager.Instance.SetMusicVolume(value);
            SaveModule.Instance.SaveMusicVolume(value);
        }
        
        private void SaveSfxValue(float value)
        {
            AudioManager.Instance.SetSfxVolume(value);
            SaveModule.Instance.SaveSfxVolume(value);
        }
        
        private void OnMainMenuButtonClicked()
        {
            SaveModule.Instance.SaveBackgroundPositionX(_background.uvRect.position.x);
            SaveModule.Instance.SaveBackgroundPositionY(_background.uvRect.position.y);
            SceneLoader.Instance.LoadSceneWithoutLoadingScreen(Constants.Scenes.MainMenu);
        }
    }
}