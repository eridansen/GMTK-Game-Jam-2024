using Cancer_Cells;
using Cinemachine;
using UnityEngine;

namespace Core.Levels.FirstLevel
{
    public class FirstLevelController : MonoBehaviour
    {
        [SerializeField] private CancerCellMonitor _cancerCellMonitor;
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;
        [SerializeField] private GameObject _windowPrefab;
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private Transform _spawnPoint;
        private GameObject _window;
        private GameObject _player;
        private PlayerCombat _playerCombat;
        
        private void Awake()
        {
            SpawnPlayer();
            InitializeCamera();
            InitializeWindow();
        }

        private void SpawnPlayer()
        {
            _player = Instantiate(_playerPrefab, _spawnPoint.position, Quaternion.identity);
            _playerCombat = _player.GetComponent<PlayerCombat>();
        }

        private void InitializeCamera()
        {
            _virtualCamera.Follow = _player.transform;
        }
        
        private void InitializeWindow()
        {
            _window = UIModule.Instance.InstantiateUIPrefab(_windowPrefab);
            _window.GetComponent<FirstLevelWindow>().Initialize(this, _playerCombat, _cancerCellMonitor);
        }
    }
}