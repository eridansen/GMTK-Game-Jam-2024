using Helpers;
using UnityEngine;

namespace Core
{
    public class CameraManager : Singleton<CameraManager>
    {
        [SerializeField] private Camera _mainCamera;
    }
}