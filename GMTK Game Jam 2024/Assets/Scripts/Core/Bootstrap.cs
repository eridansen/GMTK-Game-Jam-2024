using UnityEngine;

namespace Core
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private SceneLoader _sceneLoader;
        [SerializeField] private UIModule _uiModule;
        [SerializeField] private AudioManager _audioManager;
        [SerializeField] private SaveModule _saveModule;
        [SerializeField] private CameraManager _cameraManager;

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
            DontDestroyOnLoad(_cameraManager);
        }
        
        private void LoadSoundValues()
        {
            _audioManager.SetMusicVolume(_saveModule.LoadMusicVolume());
            _audioManager.SetSfxVolume(_saveModule.LoadSfxVolume());
        }

        private void LoadInitialScene()
        {
            _sceneLoader.LoadSceneWithoutLoadingScreen(Constants.Scenes.MainMenu);
        }
    }
}