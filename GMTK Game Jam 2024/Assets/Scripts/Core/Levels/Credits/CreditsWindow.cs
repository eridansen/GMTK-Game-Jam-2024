using UnityEngine;
using UnityEngine.UI;

namespace Core.Levels.Credits
{
    public class CreditsWindow : MonoBehaviour
    {
        [SerializeField] private Button _mainMenuButton;
        [SerializeField] private Button _mathaeuzButton;
        [SerializeField] private Button _sebastianButton;
        [SerializeField] private Button _andybugsButton;
        [SerializeField] private Button _rySixButton;
        [SerializeField] private Button _jakeButton;
        [SerializeField] private Button _jIssacGadientButton;
        [SerializeField] private Button _jkingyButton;
        [SerializeField] private Button _knoxyButton;
        [SerializeField] private Button _bevisavaButton;
        [SerializeField] private Button _minHtetNaingButton;
        [SerializeField] private Button _reixlenButton;
        
        private CreditsController _levelController;

        public void Initialize(CreditsController levelController)
        {
            _levelController = levelController;
        }

        private void Awake()
        {
            _mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);

            _mathaeuzButton.onClick.AddListener(OnMathaeuzButtonClicked);
            _sebastianButton.onClick.AddListener(OnSebastianButtonClicked);
            _andybugsButton.onClick.AddListener(OnAndybugsButtonClicked);
            _rySixButton.onClick.AddListener(OnRySixButtonClicked);
            _jakeButton.onClick.AddListener(OnJakeButtonClicked);
            _jIssacGadientButton.onClick.AddListener(OnJIssacGadientButtonClicked);
            _jkingyButton.onClick.AddListener(OnJkingyButtonClicked);
            _knoxyButton.onClick.AddListener(OnKnoxyButtonClicked);
            _bevisavaButton.onClick.AddListener(OnBevisavaButtonClicked);
            _minHtetNaingButton.onClick.AddListener(OnMinHtetNaingButtonClicked);
            _reixlenButton.onClick.AddListener(OnReixlenButtonClicked);
        }

        private void OnMainMenuButtonClicked()
        {
            SceneLoader.Instance.LoadSceneWithoutLoadingScreen(Constants.Scenes.MainMenu);
        }

        private void OnMathaeuzButtonClicked()
        {
            Application.OpenURL("https://www.youtube.com/watch?v=dQw4w9WgXcQ");
        }

        private void OnSebastianButtonClicked()
        {
            Application.OpenURL("https://www.youtube.com/watch?v=dQw4w9WgXcQ");
        }

        private void OnAndybugsButtonClicked()
        {
            Application.OpenURL("https://www.youtube.com/watch?v=dQw4w9WgXcQ");
        }

        private void OnRySixButtonClicked()
        {
            Application.OpenURL("https://www.youtube.com/watch?v=dQw4w9WgXcQ");
        }

        private void OnJakeButtonClicked()
        {
            Application.OpenURL("https://www.youtube.com/watch?v=dQw4w9WgXcQ");
        }

        private void OnJIssacGadientButtonClicked()
        {
            Application.OpenURL("https://www.youtube.com/watch?v=dQw4w9WgXcQ");
        }

        private void OnJkingyButtonClicked()
        {
            Application.OpenURL("https://www.youtube.com/watch?v=dQw4w9WgXcQ");
        }

        private void OnKnoxyButtonClicked()
        {
            Application.OpenURL("https://x.com/qknoxy_");
        }
        
        private void OnBevisavaButtonClicked()
        {
            Application.OpenURL("https://www.youtube.com/watch?v=dQw4w9WgXcQ");
        }

        private void OnMinHtetNaingButtonClicked()
        {
            Application.OpenURL("https://www.youtube.com/watch?v=dQw4w9WgXcQ");
        }

        private void OnReixlenButtonClicked()
        {
            Application.OpenURL("https://www.youtube.com/watch?v=dQw4w9WgXcQ");
        }
    }
}
