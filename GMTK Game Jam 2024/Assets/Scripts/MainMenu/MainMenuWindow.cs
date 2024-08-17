using Core;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class MainMenuWindow : MonoBehaviour
    {
        [SerializeField] private Button _startButton;
        private MainMenuController _levelController;
        
        public void Initialize(MainMenuController levelController)
        {
            _levelController = levelController;
        }

        private void Awake()
        {
            _startButton.onClick.AddListener(OnStartButtonClicked);
        }
        
        private void OnStartButtonClicked()
        {
            SceneLoader.Instance.LoadSceneWithLoadingScreen(Constants.Scenes.FirstLevel);
        }
    }
}