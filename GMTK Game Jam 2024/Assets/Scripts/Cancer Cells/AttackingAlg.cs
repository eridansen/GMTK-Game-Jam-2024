using Assets.Scripts;
using UnityEngine;

public class AttackingAlg 
{
    public AttackingAlg(GameObject target, IAttacker attacker) 
    {
        this.target = target;
        this.attacker = attacker;
    }

    private IAttacker attacker;
    private GameObject target;
    private bool isTargetNear = false;

    public void Tick()
    {
        float distToTarget = Vector2.Distance(target.transform.position, attacker.Position);

        if(distToTarget > 1f)
        {
            attacker.Attack();
        }
        else
        {
            attacker.MoveToTarget(target.transform.position);
        }
    }
}
