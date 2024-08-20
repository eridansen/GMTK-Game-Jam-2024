using Cinemachine;
using UnityEngine;

namespace Core.Levels.BossLevel
{
    public class BossLevelController : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;
        [SerializeField] private GameObject _windowPrefab;
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private BossSpawner _bossSpawner;
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
            _window.GetComponent<BossLevelWindow>().Initialize(this, _playerCombat, _bossSpawner);
        }
    }
}