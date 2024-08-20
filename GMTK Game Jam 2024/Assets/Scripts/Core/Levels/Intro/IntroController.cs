using UnityEngine;

namespace Core.Levels.Intro
{
    public class IntroController : MonoBehaviour
    {
        [SerializeField] private GameObject _windowPrefab;
        private GameObject _window;

        public void GoToFirstLevel()
        {
            SceneLoader.Instance.LoadSceneWithLoadingScreen(Constants.Scenes.FirstLevel);
        }
        
        private void Awake()
        {
            InitializeWindow();
        }

        private void InitializeWindow()
        {
            _window = UIModule.Instance.InstantiateUIPrefab(_windowPrefab);
            _window.GetComponent<IntroWindow>().Initialize(this);
        }
    }
}