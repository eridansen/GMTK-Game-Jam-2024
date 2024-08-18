using UnityEngine;

namespace Core.Levels.Credits
{
    public class CreditsController : MonoBehaviour
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
            _window.GetComponent<CreditsWindow>().Initialize(this);
        }
    }
}