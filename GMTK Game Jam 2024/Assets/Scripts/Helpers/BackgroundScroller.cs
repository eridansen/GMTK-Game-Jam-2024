using System;
using UnityEngine;
using UnityEngine.UI;

namespace Helpers
{
    public class BackgroundScroller : MonoBehaviour
    {
        [SerializeField] private RawImage _image;
        [SerializeField] private float _scrollingSpeedX;
        [SerializeField] private float _scrollingSpeedY;

        private void Update()
        {
            var position = _image.uvRect.position + new Vector2(_scrollingSpeedX, _scrollingSpeedY) * Time.deltaTime;
            _image.uvRect = new Rect(position, _image.uvRect.size);
        }
    }
}