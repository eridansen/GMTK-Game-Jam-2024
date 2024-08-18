using UnityEngine;
using UnityEngine.UI;

namespace Core.Levels.Settings
{
    public class SettingsWindow : MonoBehaviour
    {
        [SerializeField] private Button _mainMenuButton;
        private SettingsController _levelController;

        public void Initialize(SettingsController levelController)
        {
            _levelController = levelController;
        }
        
        private void Awake()
        {
            _mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
        }
        
        private void OnMainMenuButtonClicked()
        {
            SceneLoader.Instance.LoadSceneWithLoadingScreen(Constants.Scenes.MainMenu);
        }
    }
}