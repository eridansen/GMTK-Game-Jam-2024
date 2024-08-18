using UnityEngine;
using UnityEngine.UI;

namespace Core.Levels.MainMenu
{
    public class MainMenuWindow : MonoBehaviour
    {
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _exitButton;
        [SerializeField] private Button _creditsButton;
        
        private MainMenuController _levelController;
        
        public void Initialize(MainMenuController levelController)
        {
            _levelController = levelController;
        }

        private void Awake()
        {
            _startButton.onClick.AddListener(OnStartButtonClicked);
            _settingsButton.onClick.AddListener(OnSettingsButtonClicked);
            _exitButton.onClick.AddListener(OnExitButtonClicked);
            _creditsButton.onClick.AddListener(OnCreditsButtonClicked);
        }

        private void OnStartButtonClicked()
        {
            SceneLoader.Instance.LoadSceneWithLoadingScreen(Constants.Scenes.FirstLevel);
        }
        
        private void OnSettingsButtonClicked()
        {
            SceneLoader.Instance.LoadSceneWithLoadingScreen(Constants.Scenes.Settings);
        }
        
        private void OnExitButtonClicked()
        {
            Application.Quit();
        }
        
        private void OnCreditsButtonClicked()
        {
            SceneLoader.Instance.LoadSceneWithLoadingScreen(Constants.Scenes.Credits);
        }
    }
}