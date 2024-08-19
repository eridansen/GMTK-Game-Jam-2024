using UnityEngine;

public class AttackingCancerAnimator : MonoBehaviour
{
    [SerializeField] private string attackTriggerName;
    [SerializeField] private string deathTriggerName;

    private Animator animator;

    private int deathHash;
    private int attackHash;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        
        deathHash = Animator.StringToHash(deathTriggerName);
        attackHash = Animator.StringToHash(attackTriggerName);
    }

    public void PlayAttackAnim()
    {
        animator.SetTrigger(attackHash);
    }

    public void PlayDeathAnim()
    {
        animator.SetTrigger(deathHash);
    }
}
