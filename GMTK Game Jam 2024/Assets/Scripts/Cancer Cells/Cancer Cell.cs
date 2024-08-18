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


    private void Start()
    {
        InvokeRepeating(nameof(Spray), 0, _sprayInterval);
    }

    private void Spray()
    { 
        for (int i = 0; i < _sprayAmount; i++)
        { 
            var spr = ObjectPooler.ProvideObject(_sprayedPrefab, transform.position, 
                _sprayedPrefab.transform.rotation) as SprayedCancer;
        }
    }

    
}
