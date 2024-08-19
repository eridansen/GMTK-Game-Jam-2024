using UnityEngine;
using UnityEngine.UI;

namespace Core.Levels.Loading
{
    public class LoadingWindow : MonoBehaviour
    {
        [SerializeField] private RawImage _background;
        [SerializeField] private Image _image;
        [SerializeField] private float _rotationSpeed;
        private LoadingController _levelController;
        
        public void Initialize(LoadingController levelController)
        {
            _levelController = levelController;
        }

        private void Awake()
        {
            SetBackgroundPosition();
        }
        
        private void Update()
        {
            _image.transform.Rotate(Vector3.back * _rotationSpeed);
        }

        private void OnDestroy()
        {
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