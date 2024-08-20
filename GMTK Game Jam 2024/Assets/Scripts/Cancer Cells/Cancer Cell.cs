using System.Collections;
using UnityEngine;

public class CancerCell : MonoBehaviour, IDamageable
{
    [Header("Spray settings")]
    [SerializeField] private SprayedCancer _sprayedPrefab;
    [SerializeField] private float _sprayInterval;
    [SerializeField] private float _sprayAmount;
    [SerializeField] private int _spawnLimit = 5;
    [SerializeField] private SprayedCancer[] _attackersArray;
    private Health health;

    private void Awake()
    {
        health = GetComponent<Health>();
        _attackersArray = new SprayedCancer[_spawnLimit];
    }

    private void Start()
    {
        StartCoroutine(SprayRoutine());
    }

    private IEnumerator SprayRoutine()
    {
        while (true)
        {
            Spray();
            yield return new WaitForSeconds(_sprayInterval);
        }
    }

    private void Spray()
    {
        int availableSlotIndex = GetAvailableSlotIndex();

        if (availableSlotIndex != -1)
        {
            var attacker = Instantiate(_sprayedPrefab, transform.position, _sprayedPrefab.transform.rotation);
            _attackersArray[availableSlotIndex] = attacker;
            AddListener(attacker);
        }
    }

    private int GetAvailableSlotIndex()
    {
        for (int i = 0; i < _attackersArray.Length; i++)
        {
            if (_attackersArray[i] == null)
            {
                return i;
            }
        }
        return -1; // No available slot
    }

    private void AddListener(SprayedCancer sprayedCancer)
    {
        sprayedCancer.OnPlayerSpotted += HiveAttack;
        sprayedCancer.OnDie += RemoveListener;
    }

    private void HiveAttack(Transform attackTarget)
    {
        foreach (var attacker in _attackersArray)
        {
            if (attacker != null)
            {
                attacker.StartAttacking(attackTarget);
            }
        }
    }

    private void RemoveListener(SprayedCancer listener)
    {
        listener.OnPlayerSpotted -= HiveAttack;
        listener.OnDie -= RemoveListener;

        for (int i = 0; i < _attackersArray.Length; i++)
        {
            if (_attackersArray[i] == listener)
            {
                _attackersArray[i] = null;
                Destroy(listener.gameObject);
                break;
            }
        }

        ClearMissingReferences();
    }

    private void ClearMissingReferences()
    {
        for (int i = 0; i < _attackersArray.Length; i++)
        {
            if (_attackersArray[i] == null)
            {
                _attackersArray[i] = null;
            }
        }
    }

    public void Damage(float damageAmount)
    {
        health.TakeDamage(damageAmount);
    }
}