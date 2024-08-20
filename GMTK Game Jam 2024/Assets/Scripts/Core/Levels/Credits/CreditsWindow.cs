using UnityEngine;
using UnityEngine.UI;

namespace Core.Levels.Credits
{
    public class CreditsWindow : MonoBehaviour
    {
        [SerializeField] private RawImage _background;
        [SerializeField] private Button _mainMenuButton;
        [SerializeField] private Button _mathaeuzButton;
        [SerializeField] private Button _sebastianButton;
        [SerializeField] private Button _rySixButton;
        [SerializeField] private Button _jakeButton;
        [SerializeField] private Button _jIssacGadientButton;
        [SerializeField] private Button _andybugsButton;
        [SerializeField] private Button _jkingyButton;
        [SerializeField] private Button _knoxyButton;
        [SerializeField] private Button _bevisavaButton;
        [SerializeField] private Button _minHtetNaingButton;
        [SerializeField] private Button _reixlenButton;
        [SerializeField] private Button _nimbusButton;
        
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
            _rySixButton.onClick.AddListener(OnRySixButtonClicked);
            _jakeButton.onClick.AddListener(OnJakeButtonClicked);
            _jIssacGadientButton.onClick.AddListener(OnJIssacGadientButtonClicked);
            _andybugsButton.onClick.AddListener(OnAndybugsButtonClicked);
            _jkingyButton.onClick.AddListener(OnJkingyButtonClicked);
            _knoxyButton.onClick.AddListener(OnKnoxyButtonClicked);
            _bevisavaButton.onClick.AddListener(OnBevisavaButtonClicked);
            _minHtetNaingButton.onClick.AddListener(OnMinHtetNaingButtonClicked);
            _reixlenButton.onClick.AddListener(OnReixlenButtonClicked);
            _nimbusButton.onClick.AddListener(OnNimbusButtonClicked);

            SetBackgroundPosition();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnMainMenuButtonClicked();
            }
        }
        
        private void SetBackgroundPosition()
        {
            Vector2 position = new Vector2(SaveModule.Instance.LoadBackgroundPositionX(), SaveModule.Instance.LoadBackgroundPositionY());
            _background.uvRect = new Rect(position, _background.uvRect.size);
        }

        private void OnMainMenuButtonClicked()
        {
            SaveModule.Instance.SaveBackgroundPositionX(_background.uvRect.position.x);
            SaveModule.Instance.SaveBackgroundPositionY(_background.uvRect.position.y);
            SceneLoader.Instance.LoadSceneWithoutLoadingScreen(Constants.Scenes.MainMenu);
        }

        private void OnMathaeuzButtonClicked()
        {
            Application.OpenURL("https://mathaeuz.itch.io/");
        }

        private void OnSebastianButtonClicked()
        {
            Application.OpenURL("https://zebravibe.itch.io/");
        }

        private void OnRySixButtonClicked()
        {
            Application.OpenURL("https://ryesix.itch.io/");
        }

        private void OnJakeButtonClicked()
        {
            Application.OpenURL("https://jake52.itch.io");
        }

        private void OnJIssacGadientButtonClicked()
        {
            Application.OpenURL("http://jisaacgadient.com");
        }

        private void OnAndybugsButtonClicked()
        {
        }
        
        private void OnJkingyButtonClicked()
        {
            Application.OpenURL("https://jkingy.itch.io/");
        }

        private void OnKnoxyButtonClicked()
        {
            Application.OpenURL("https://x.com/qknoxy_");
        }
        
        private void OnBevisavaButtonClicked()
        {
        }

        private void OnMinHtetNaingButtonClicked()
        {
            Application.OpenURL("https://osbert.itch.io/");
        }

        private void OnReixlenButtonClicked()
        {
            Application.OpenURL("https://reixlen-dev.itch.io");
        }
        
        private void OnNimbusButtonClicked()
        {
        }
    }
}
