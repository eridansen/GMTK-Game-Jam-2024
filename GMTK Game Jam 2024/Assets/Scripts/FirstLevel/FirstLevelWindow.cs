using Core;
using UnityEngine;
using UnityEngine.UI;

namespace FirstLevel
{
    public class FirstLevelWindow : MonoBehaviour
    {
        [SerializeField] private Button _mainMenuButton;
        private FirstLevelController _levelController;
        
        public void Initialize(FirstLevelController levelController)
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