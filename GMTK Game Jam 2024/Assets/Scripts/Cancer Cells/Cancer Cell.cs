using System.Collections.Generic;
using UnityEngine;

public class CancerCell : MonoBehaviour
{
    [Header ("Cell settings")]
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
        { 
            var attacker = ObjectPooler.ProvideObject(_sprayedPrefab, transform.position, 
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
}
