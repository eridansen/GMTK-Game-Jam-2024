using UnityEngine;

public class SawAttack : MonoBehaviour
{
    [SerializeField] private float _damage = 5;
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.gameObject.TryGetComponent(out PlayerCombat player))
        {
            return;
        }
        
        player.Damage(_damage);
    }
}