using UnityEngine;
using UnityEngine.Events;

public class CancerCell : MonoBehaviour
{
    [Header ("Cell settings")]
    [SerializeField] private float _damage;
    [SerializeField] private float _lifeDuration;
    [SerializeField] private float _healthPoint;

    [Header("Spray settings")]
    [SerializeField] private SprayedCancer _sprayedPrefab;
    [SerializeField] private float _sprayInterval;
    [SerializeField] private float _sprayAmount;
    [SerializeField] private float _spawnOffsetMultiplicator;


    private void Start()
    {
        InvokeRepeating(nameof(Spray), 0, _sprayInterval);
    }

    private void Spray()
    { 
        for (int i = 0; i < _sprayAmount; i++)
        { 
            Vector3 offset = new Vector2(1f * _spawnOffsetMultiplicator, 1f * _spawnOffsetMultiplicator);
            Vector2 initialPos = this.transform.position + offset;
            var spr = ObjectPooler.ProvideObject(_sprayedPrefab, initialPos, 
                _sprayedPrefab.transform.rotation) as SprayedCancer;
            //spr.OnCollisionWithNormalCell += OnCellInfected;
            spr._damage = _damage;
        }
    }

    
}
