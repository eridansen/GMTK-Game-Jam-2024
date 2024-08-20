using Cancer_Cells;
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
        [SerializeField] private GameObject _cancerBar;
        [SerializeField] private Image _cancerAmountImage;
        [SerializeField] private GameObject _endGameWindow;
        private FirstLevelController _levelController;
        private CancerCellMonitor _cancerCellMonitor;
        private PlayerCombat _playerCombat;
        
        public void Initialize(FirstLevelController levelController, PlayerCombat playerCombat, CancerCellMonitor cancerCellMonitor)
        {
            _levelController = levelController;
            _cancerCellMonitor = cancerCellMonitor;
            _playerCombat = playerCombat;

            _playerCombat.healed += OnPlayerHealed;
            _playerCombat.damaged += OnPlayerDamaged;
            _playerCombat.died += OnPlayerDied;
            _cancerCellMonitor.childCountChanged += OnCancerCellsChanged;
        }

        private void Awake()
        {
            _mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
            _mainMenuButton2.onClick.AddListener(OnMainMenuButtonClicked);
            _restartButton.onClick.AddListener(OnRestartButtonClicked);
        }
        
        private void OnDestroy()
        {
            _cancerCellMonitor.childCountChanged -= OnCancerCellsChanged;
        }
        
        private void OnCancerCellsChanged(int value)
        {
            int cancelCells = _cancerCellMonitor.GetInitialCount();
            float fillAmount = (float)value/cancelCells;
            _cancerAmountImage.fillAmount = fillAmount;

            if (fillAmount == 0)
            {
                SceneLoader.Instance.LoadSceneWithLoadingScreen(Constants.Scenes.BossLevel);
            }
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