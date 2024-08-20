using UnityEngine;

public enum TargetType
{
    Player,
}

public class AttackingCellBattleBehaviour : MonoBehaviour
{
    [SerializeField] public float _damage;
    [SerializeField] private float _speed;
    [SerializeField] private float _attackDelay;
    [SerializeField] private float _range;

    public TargetType TargetType = TargetType.Player;

    private Transform target;
    private bool IsFighting;
    private AttackingCancerAnimator animator;

    private void Awake()
    {
        animator = GetComponent<AttackingCancerAnimator>();
    }

    public void StartFighting(Transform target)
    {
        IsFighting = true;
        this.target = target;
    }

    private void FixedUpdate()
    {
        if(IsFighting)
        {
            float distToTarget = Vector2.Distance(target.position, this.gameObject.transform.position);

            if (distToTarget < 1f)
            {
                Invoke(nameof(Attack),_attackDelay);
            }
            else
            {
                MoveToTarget();
            }
        }
    }

    public void MoveToTarget()
    {
        Vector2 direction = target.position - transform.position;

        transform.Translate(direction.normalized * _speed);
    }

    public void Attack()
    {
        animator.PlayAttackAnim();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _range); 

        foreach (Collider2D collider in colliders)
        {
            if(collider.gameObject.TryGetComponent<PlayerCombat>(out  PlayerCombat player))
            {
                player.Damage(_damage);
                Destroy(this.gameObject);
            }
        }
    }

}
