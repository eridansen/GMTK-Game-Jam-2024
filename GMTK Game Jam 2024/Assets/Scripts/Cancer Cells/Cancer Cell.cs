using System.Collections.Generic;
using UnityEngine;

public class CancerCell : MonoBehaviour
{
<<<<<<< HEAD
    [Header ("Cell settings")]
=======
    [Header("Cell settings")]
    [SerializeField] private float _lifeDuration;
>>>>>>> Player-movement
    [SerializeField] private float _healthPoint;

    [Header("Spray settings")]
    [SerializeField] private SprayedCancer _sprayedPrefab;
    [SerializeField] private float _sprayInterval;
    [SerializeField] private float _sprayAmount;


    private List<SprayedCancer> _attackersList = new List<SprayedCancer>();


    private void Start()
    {
        InvokeRepeating(nameof(Spray), 0, _sprayInterval);
    }


    private void Spray()
    {
        for (int i = 0; i < _sprayAmount; i++)
<<<<<<< HEAD
        { 
            var attacker = ObjectPooler.ProvideObject(_sprayedPrefab, transform.position, 
=======
        {
            Vector3 offset = new Vector2(1f * _spawnOffsetMultiplicator, 1f * _spawnOffsetMultiplicator);
            Vector2 initialPos = this.transform.position + offset;
            var spr = ObjectPooler.ProvideObject(_sprayedPrefab, initialPos,
>>>>>>> Player-movement
                _sprayedPrefab.transform.rotation) as SprayedCancer;

            AddListender(attacker);
        }
    }
    private void AddListender(SprayedCancer sprayedCancer)
    {
        _attackersList.Add(sprayedCancer);
        sprayedCancer.OnPlayerSpotted += HiveAttack;
        sprayedCancer.OnDie += RemoveListener;
    }


    private void HiveAttack(Transform attackTarget)
    {
        foreach (var attacker in _attackersList)
            attacker.StartAttacking(attackTarget);
    }


    public void RemoveListener(SprayedCancer listener)
    {
        listener.OnPlayerSpotted -= HiveAttack;
        _attackersList.Remove(listener);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Cell cell = other.gameObject.GetComponent<Cell>();
        if (cell != null)
        {
            //cell.Damage(_healthPoint);
        }
    }
}
