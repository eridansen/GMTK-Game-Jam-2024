using System;
using UnityEngine;

namespace Cancer_Cells
{
    public class CancerCellMonitor : MonoBehaviour
    {
        public event Action<int> childCountChanged;

        private int _initialChildCount;
        private int _currentChildCount;

        public int GetInitialCount()
        {
            return _initialChildCount;
        }
        
        private void Awake()
        {
            _initialChildCount = transform.childCount;
            _currentChildCount = _initialChildCount;
            Debug.Log(_initialChildCount);
        }

        private void Update()
        {
            var currentChildCount = transform.childCount;
            if (currentChildCount != _currentChildCount)
            {
                if (currentChildCount < _initialChildCount)
                {
                    childCountChanged?.Invoke(currentChildCount);
                    Debug.Log(currentChildCount);
                }
                
                _currentChildCount = currentChildCount;
            }
        }
    }

}