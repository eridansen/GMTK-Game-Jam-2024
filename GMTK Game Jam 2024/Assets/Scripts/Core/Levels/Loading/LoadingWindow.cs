using UnityEngine;
using UnityEngine.UI;

namespace Core.Levels.Loading
{
    public class LoadingWindow : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private float _rotationSpeed;
        private LoadingController _levelController;
        
        public void Initialize(LoadingController levelController)
        {
            _levelController = levelController;
        }

        private void Update()
        {
            _image.transform.Rotate(Vector3.back * _rotationSpeed);
        }
    }
}