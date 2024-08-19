using UnityEngine;
using UnityEngine.UI;

namespace Core.Levels.MainMenu
{
    public class MainMenuWindow : MonoBehaviour
    {
        [SerializeField] private RawImage _background;
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
            
            SetBackgroundPosition();
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                OnStartButtonClicked();
            }
        }

        private void SetBackgroundPosition()
        {
            Vector2 position = new Vector2(SaveModule.Instance.LoadBackgroundPositionX(), SaveModule.Instance.LoadBackgroundPositionY());
            _background.uvRect = new Rect(position, _background.uvRect.size);
        }

        private void SaveBackgroundPosition()
        {
            SaveModule.Instance.SaveBackgroundPositionX(_background.uvRect.position.x);
            SaveModule.Instance.SaveBackgroundPositionY(_background.uvRect.position.y);
        }
        
        private void OnStartButtonClicked()
        {
            SaveBackgroundPosition();
            SceneLoader.Instance.LoadSceneWithLoadingScreen(Constants.Scenes.FirstLevel);
        }
        
        private void OnSettingsButtonClicked()
        {
            SaveBackgroundPosition();
            SceneLoader.Instance.LoadSceneWithoutLoadingScreen(Constants.Scenes.Settings);
        }
        
        private void OnExitButtonClicked()
        {
            SaveBackgroundPosition();
            Application.Quit();
        }
        
        private void OnCreditsButtonClicked()
        {
            SaveBackgroundPosition();
            SceneLoader.Instance.LoadSceneWithoutLoadingScreen(Constants.Scenes.Credits);
        }
    }
}