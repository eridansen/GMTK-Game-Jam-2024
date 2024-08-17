using UnityEngine;

namespace Core.Levels.Loading
{
    public class LoadingWindow : MonoBehaviour
    {
        private LoadingController _levelController;
        
        public void Initialize(LoadingController levelController)
        {
            _levelController = levelController;
        }
    }
}