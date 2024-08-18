using Assets.Scripts;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent (typeof(Rigidbody2D), typeof(Collider2D))]
public class SprayedCancer : MonoBehaviour, IAttacker
{
    [SerializeField] public float _damage;
    [SerializeField] private float _lifeDuration;
    [SerializeField] private float _speed;

    public UnityAction<Transform> OnPlayerSpotted;

    private Rigidbody2D rigidbody;
    private Transform player;
    private Detector detector;
    private AttackingAlg attackingAlg;
    private AttackingCancerAnimator animator;
    private bool IsPlayerSpotted = false;
    private LayerMask playerLayer;
    public Vector3 Position => this.gameObject.transform.position;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        detector = GetComponentInChildren<Detector>();
        player = FindAnyObjectByType<PlayerMovement>().transform;
        animator = GetComponent<AttackingCancerAnimator>();
        playerLayer = LayerMask.GetMask("Player");

        detector.OnPlayerDetected += SpotPlayer;
        attackingAlg = new AttackingAlg(player.gameObject, this);
    }

    private void SpotPlayer(Transform player)
    {
        OnPlayerSpotted?.Invoke(player);
        IsPlayerSpotted = true;
    }

    private void Start()
    {
        Vector3 direction = new Vector3(
            Random.Range(-1, 1f),
            Random.Range(-1, 1f),
            0f);

        rigidbody.AddForce(direction * _speed);
    }

    private void Update()
    {
        if (IsPlayerSpotted)
        {
            attackingAlg.Tick();
        }
    }

    public void StartAttacking(Transform target)
    {
        player = target;
        IsPlayerSpotted = true;
    }


    public void MoveToTarget(Vector3 position)
    {
        Vector2 direction = position - transform.position;

        transform.Translate(direction.normalized * _speed);
    }

    public void Attack()
    {
        animator.PlayAttackAnim();
    }




    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("NormalCell"))
        {
            Cell normalCell = collision.gameObject.GetComponent<Cell>();
            Debug.Log(normalCell!=null);
            //Get component and infect
            if (normalCell != null && _damage > normalCell.cellData.Defense)
            {
                // Do stuff???????
            }
            else 
            {
                Debug.Log("Normal cell object not null: "+ (normalCell != null));
                Debug.Log("cancer cell damage ["+_damage+"]"+", normal cell defense [" +normalCell.cellData.Defense+"]");
            }
        }
        // else just bounce
    }

    private void OnDisable()
    {
        
    }
}
