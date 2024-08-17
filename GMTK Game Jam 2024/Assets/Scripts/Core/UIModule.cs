using Helpers;
using UnityEngine;

namespace Core
{
    public class UIModule : Singleton<UIModule>
    {
        [SerializeField] private Canvas _canvas;
        private GameObject _currentUIElement;

        public GameObject InstantiateUIPrefab(GameObject uiPrefab)
        {
            if (_canvas == null)
            {
                Debug.LogError("Canvas reference is not set.");
                return null;
            }

            if (_currentUIElement != null)
            {
                Destroy(_currentUIElement);
            }

            _currentUIElement = Instantiate(uiPrefab, _canvas.transform);

            var rectTransform = _currentUIElement.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.localPosition = Vector3.zero;
                rectTransform.localScale = Vector3.one;
            }

            return _currentUIElement;
        }

        public void SetCanvas(Canvas canvasReference)
        {
            _canvas = canvasReference;
        }
    }

}