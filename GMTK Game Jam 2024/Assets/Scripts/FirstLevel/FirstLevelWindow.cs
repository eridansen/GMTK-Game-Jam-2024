using UnityEngine;

namespace FirstLevel
{
    public class FirstLevelWindow : MonoBehaviour
    {
        private FirstLevelController _levelController;
        
        public void Initialize(FirstLevelController levelController)
        {
            _levelController = levelController;
        }
    }
}