using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitDetection : MonoBehaviour
{
    private PlayerCombat playerCombat;

    private void Awake() {
        playerCombat = GetComponentInParent<PlayerCombat>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            playerCombat.Hit(damageable);
        }
    }

}
