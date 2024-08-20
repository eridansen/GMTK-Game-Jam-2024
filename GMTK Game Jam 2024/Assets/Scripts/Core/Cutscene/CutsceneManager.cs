using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Cutscene
{
    public class CutsceneManager : MonoBehaviour
    {
        public event Action ended;
        
        [SerializeField] private Image _cutsceneImage;
        [SerializeField] private TextMeshProUGUI _cutsceneText;
        [SerializeField] private float _typingSpeed = 0.05f;
        [SerializeField] private Button _nextSlideButton;
        
        private bool _isTextFullyDisplayed = false;
        private int _currentSlideIndex = 0;
        private Cutscene _currentCutscene;

        public void PlayCutscene(Cutscene cutscene)
        {
            _currentCutscene = cutscene;
            _currentSlideIndex = 0;
            StartCoroutine(DisplaySlide());
        }

        private void Awake()
        {
            _nextSlideButton.onClick.AddListener(OnNextSlideButtonClicked);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0))
            {
                OnNextSlideButtonClicked();
            }
        }

        private IEnumerator DisplaySlide()
        {
            _nextSlideButton.gameObject.SetActive(false);
            var slide = _currentCutscene.slides[_currentSlideIndex];

            if (_cutsceneImage.sprite != slide.image)
            {
                _cutsceneImage.sprite = slide.image;
            }

            if (slide.image == null)
            {
                var color = _cutsceneImage.color;
                color.a = 0;
                _cutsceneImage.color = color;
            }
            else
            {
                var color = _cutsceneImage.color;
                color.a = 255;
                _cutsceneImage.color = color;
            }

            _cutsceneText.text = "";
            foreach (var letter in slide.text.ToCharArray())
            {
                _cutsceneText.text += letter;
                yield return new WaitForSeconds(_typingSpeed);
            }

            _isTextFullyDisplayed = true;
            _nextSlideButton.gameObject.SetActive(true);
        }

        private void OnNextSlideButtonClicked()
        {
            if (_isTextFullyDisplayed)
            {
                _currentSlideIndex++;
                if (_currentSlideIndex < _currentCutscene.slides.Length)
                {
                    _isTextFullyDisplayed = false;
                    StartCoroutine(DisplaySlide());
                }
                else
                {
                    SaveModule.Instance.SaveIntroCutscene(1);
                    ended?.Invoke();
                }
            }
        }
    }
}