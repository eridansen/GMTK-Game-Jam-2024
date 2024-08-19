using UnityEngine;
using UnityEngine.Events;


[RequireComponent (typeof(Rigidbody2D), typeof(Collider2D))]
public class SprayedCancer : MonoBehaviour
{
    [SerializeField] private float _lifeDuration;

    public UnityAction<Transform> OnPlayerSpotted;
    public UnityAction<SprayedCancer> OnDie;

    private Transform player;
    private Detector detector;

    private AttackingCancerAnimator animator;
    private AttackingCellBattleBehaviour battleBehaviour;
    


    private void Awake()
    {
        detector = GetComponentInChildren<Detector>();
        animator = GetComponent<AttackingCancerAnimator>();
        battleBehaviour = GetComponent<AttackingCellBattleBehaviour>();

        detector.OnPlayerDetected += SpotPlayer;
    }

    private void OnEnable()
    {
        Invoke(nameof(Die), _lifeDuration);
    }


    private void Start()
    {
        Vector2 forceDir = new Vector2(Random.Range(-1f,1f),Random.Range(-1f,1f));
        float forceMultiplier = 5f;
        GetComponent<Rigidbody2D>().AddForce(forceDir * forceMultiplier);
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
    }
}
