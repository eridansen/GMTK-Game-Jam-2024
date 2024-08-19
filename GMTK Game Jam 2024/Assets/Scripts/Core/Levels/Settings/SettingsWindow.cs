using UnityEngine;
using UnityEngine.UI;

namespace Core.Levels.Settings
{
    public class SettingsWindow : MonoBehaviour
    {
        [SerializeField] private Button _mainMenuButton;
        [SerializeField] private Slider _musicVolumeSlider;
        [SerializeField] private Slider _sfxVolumeSlider;
        [SerializeField] private Animator _animator;
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
        }
        
        private void LoadSliderValues()
        {
            _musicVolumeSlider.value = AudioManager.Instance.GetMusicVolume();
            _sfxVolumeSlider.value = AudioManager.Instance.GetSfxVolume();
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
            SceneLoader.Instance.LoadSceneWithoutLoadingScreen(Constants.Scenes.MainMenu);
        }
    }
}