using Core;
using UnityEngine;

namespace MainMenu
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private GameObject _windowPrefab;
        private GameObject _window;
        
        private void Awake()
        {
            InitializeWindow();
        }
        
        private void InitializeWindow()
        {
            _window = UIModule.Instance.InstantiateUIPrefab(_windowPrefab);
            _window.GetComponent<MainMenuWindow>().Initialize(this);
        }
    }
}