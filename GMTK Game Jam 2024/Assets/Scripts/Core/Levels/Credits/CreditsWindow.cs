using UnityEngine;
using UnityEngine.UI;

namespace Core.Levels.Credits
{
    public class CreditsWindow : MonoBehaviour
    {
        [SerializeField] private Button _mainMenuButton;
        private CreditsController _levelController;

        public void Initialize(CreditsController levelController)
        {
            _levelController = levelController;
        }
        
        private void Awake()
        {
            _mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
        }
        
        private void OnMainMenuButtonClicked()
        {
            SceneLoader.Instance.LoadSceneWithoutLoadingScreen(Constants.Scenes.MainMenu);
        }
    }
}