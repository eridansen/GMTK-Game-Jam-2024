using UnityEngine;

namespace Core
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private SceneLoader _sceneLoader;
        [SerializeField] private UIModule _uiModule;
        [SerializeField] private AudioManager _audioManager;

        private void Awake()
        {
            InitializeGame();
        }

        private void InitializeGame()
        {
            Application.targetFrameRate = 60;
            InitializeSystems();
            LoadInitialScene();
        }

        private void InitializeSystems()
        {
            //Initialize any global systems, like Audio, Input, etc
            DontDestroyOnLoad(_sceneLoader);
            DontDestroyOnLoad(_uiModule);
            DontDestroyOnLoad(_audioManager);
        }

        private void LoadInitialScene()
        {
            _sceneLoader.LoadSceneWithLoadingScreen(Constants.Scenes.MainMenu);
        }
    }
}