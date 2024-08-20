using UnityEngine;
using UnityEngine.Events;


[RequireComponent (typeof(Rigidbody2D), typeof(Collider2D))]
public class SprayedCancer : MonoBehaviour, IDamageable
{
    [SerializeField] private float _lifeDuration;
    [SerializeField] private float _xBias = 0.3f; // bias towards more x value when Start()
    [SerializeField] private float _yBias = 0.3f; // bias towards more y value when Start()
    [SerializeField] private float _healthPoint = 100f;


    public UnityAction<Transform> OnPlayerSpotted;
    public UnityAction<SprayedCancer> OnDie;

    private Transform player;
    private Detector detector;

    private AttackingCancerAnimator animator;
    private AttackingCellBattleBehaviour battleBehaviour;
    private Health health;
    

    private void Awake()
    {
        detector = GetComponentInChildren<Detector>();
        animator = GetComponent<AttackingCancerAnimator>();
        battleBehaviour = GetComponent<AttackingCellBattleBehaviour>();
        health = GetComponent<Health>();

        detector.OnPlayerDetected += SpotPlayer;
    }

    private void OnEnable()
    {
        Invoke(nameof(Die), _lifeDuration);
    }


    private void Start()
    {
        float x = Random.Range(-1f, 1f);
        if (Mathf.Abs(x) < 0.5)
        {
            x += _xBias * (x / Mathf.Abs(x)); // (x / Mathf.Abs(x)) for sign
        }

        float y = Random.Range(-1f, 1f);
        if (Mathf.Abs(y) < 0.5)
        {
            y += _yBias * (y / Mathf.Abs(y)); // (y / Mathf.Abs(y)) for sign
        }


        Vector2 forceDir = new Vector2(x, y);
        GetComponent<Rigidbody2D>().AddForce(forceDir * 10f);
    }

    private void SpotPlayer(Transform player)
    {
        OnPlayerSpotted?.Invoke(player);
    }


    public void StartAttacking(Transform target)
    {
        battleBehaviour.StartFighting(target);
    }


    private void Die()
    {
        animator.PlayDeathAnim();
        OnDie?.Invoke(this);
        ObjectPooler.ReturnGameObject(this);
        Destroy(this.gameObject);
    }

    public void Damage(float damageAmount)
    {
        _healthPoint -= damageAmount;
        if (_healthPoint <= 0)
        {
            this.gameObject.SetActive(false);
        }
        //health.TakeDamage(damageAmount);
    }
}
