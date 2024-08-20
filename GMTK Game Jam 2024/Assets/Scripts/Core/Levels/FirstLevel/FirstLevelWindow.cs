using UnityEngine;
using UnityEngine.UI;

namespace Core.Levels.FirstLevel
{
    public class FirstLevelWindow : MonoBehaviour
    {
        [SerializeField] private Button _mainMenuButton;
        [SerializeField] private Button _mainMenuButton2;
        [SerializeField] private Button _restartButton;
        [SerializeField] private GameObject _healthBar;
        [SerializeField] private Image _healthAmountImage;
        [SerializeField] private GameObject _endGameWindow;
        private FirstLevelController _levelController;
        private PlayerCombat _playerCombat;
        
        public void Initialize(FirstLevelController levelController, PlayerCombat playerCombat)
        {
            _levelController = levelController;
            _playerCombat = playerCombat;

            _playerCombat.healed += OnPlayerHealed;
            _playerCombat.damaged += OnPlayerDamaged;
            _playerCombat.died += OnPlayerDied;
        }

        private void Awake()
        {
            _mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
            _mainMenuButton2.onClick.AddListener(OnMainMenuButtonClicked);
            _restartButton.onClick.AddListener(OnRestartButtonClicked);
        }

        private void OnMainMenuButtonClicked()
        {
            SceneLoader.Instance.LoadSceneWithLoadingScreen(Constants.Scenes.MainMenu);
        }
        
        private void OnRestartButtonClicked()
        {
            SceneLoader.Instance.LoadSceneWithLoadingScreen(Constants.Scenes.FirstLevel);
        }
        
        private void OnPlayerHealed(float value)
        {
            _healthAmountImage.fillAmount += value/100;
        }
        
        private void OnPlayerDamaged(float value)
        {
            _healthAmountImage.fillAmount -= value/100;
        }
        
        private void OnPlayerDied()
        {
            _healthAmountImage.fillAmount = 0;
            _endGameWindow.SetActive(true);
            _mainMenuButton.gameObject.SetActive(false);
        }
    }
}