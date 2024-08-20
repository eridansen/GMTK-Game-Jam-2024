using UnityEngine;
using UnityEngine.UI;

namespace Core.Levels.BossLevel
{
    public class BossLevelWindow : MonoBehaviour
    {
        [SerializeField] private Button _mainMenuButton;
        [SerializeField] private Button _mainMenuButton2;
        [SerializeField] private Button _restartButton;
        [SerializeField] private GameObject _healthBar;
        [SerializeField] private Image _healthAmountImage;
        [SerializeField] private Image _bossHealthAmountImage;
        [SerializeField] private GameObject _endGameWindow;
        private BossLevelController _levelController;
        private PlayerCombat _playerCombat;
        private BossSpawner _bossSpawner;

        public void Initialize(BossLevelController levelController, PlayerCombat playerCombat, BossSpawner bossSpawner)
        {
            _levelController = levelController;
            _playerCombat = playerCombat;
            _bossSpawner = bossSpawner;

            _bossSpawner.SetBossBar(_bossHealthAmountImage);
            
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