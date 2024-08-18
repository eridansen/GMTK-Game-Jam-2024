using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CancerCell : MonoBehaviour
{
    [Header ("Cell settings")]
    [SerializeField] private float _lifeDuration;
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

    private void HiveAttack(Transform attackTarget)
    {
        foreach (var attacker in _attackersList)
            attacker.Attack();
    }

    private void Spray()
    { 
        for (int i = 0; i < _sprayAmount; i++)
        { 
            var attacker = ObjectPooler.ProvideObject(_sprayedPrefab, transform.position, 
                _sprayedPrefab.transform.rotation) as SprayedCancer;

            _attackersList.Add(attacker);

            attacker.OnPlayerSpotted += HiveAttack;
        }
    }

    
}
