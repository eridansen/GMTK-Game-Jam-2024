using UnityEngine;

namespace Core
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private SceneLoader _sceneLoader;
        [SerializeField] private UIModule _uiModule;
        [SerializeField] private AudioManager _audioManager;
        [SerializeField] private SaveModule _saveModule;
        [SerializeField] private Camera _mainCamera;

        private void Awake()
        {
            InitializeGame();
        }

        private void InitializeGame()
        {
            Application.targetFrameRate = 60;
            InitializeSystems();
            LoadSoundValues();
            LoadInitialScene();
        }

        private void InitializeSystems()
        {
            //Initialize any global systems, like Audio, Input, etc
            DontDestroyOnLoad(_sceneLoader);
            DontDestroyOnLoad(_uiModule);
            DontDestroyOnLoad(_audioManager);
            DontDestroyOnLoad(_saveModule);
            DontDestroyOnLoad(_mainCamera);
        }
        
        private void LoadSoundValues()
        {
            _audioManager.SetMusicVolume(_saveModule.LoadMusicVolume());
            _audioManager.SetSfxVolume(_saveModule.LoadSfxVolume());
        }

        private void LoadInitialScene()
        {
            _sceneLoader.LoadSceneWithLoadingScreen(Constants.Scenes.Loading);
        }
    }
}