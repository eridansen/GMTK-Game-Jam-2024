using Core.Cutscene;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Levels.Intro
{
    public class IntroWindow : MonoBehaviour
    {
        [SerializeField] private CutsceneManager _cutsceneManager;
        [SerializeField] private Cutscene.Cutscene _cutsceneToPlay;
        [SerializeField] private RawImage _background;
        private IntroController _levelController;
        
        public void Initialize(IntroController levelController)
        {
            _levelController = levelController;
        }

        private void Awake()
        {
            SetBackgroundPosition();
            _cutsceneManager.PlayCutscene(_cutsceneToPlay);
            
            _cutsceneManager.ended += OnCutsceneEnded;
        }
        private void OnCutsceneEnded()
        {
            _levelController.GoToFirstLevel();
        }

        private void OnDestroy()
        {
            _cutsceneManager.ended -= OnCutsceneEnded;
            
            SaveModule.Instance.SaveBackgroundPositionX(_background.uvRect.position.x);
            SaveModule.Instance.SaveBackgroundPositionY(_background.uvRect.position.y);
        }

        private void SetBackgroundPosition()
        {
            Vector2 position = new Vector2(SaveModule.Instance.LoadBackgroundPositionX(), SaveModule.Instance.LoadBackgroundPositionY());
            _background.uvRect = new Rect(position, _background.uvRect.size);
        }
    }
}