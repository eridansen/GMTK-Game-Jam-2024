using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour,IDamageable
{
    [Header("Player Stats")]
    [SerializeField] private int currentHealth = 100; // The player's health
    [SerializeField] private int maxHealth = 100; // The player's maximum health


    private void Start() {
        currentHealth = maxHealth; // Set the player's health to the maximum health
    }

    #region Health
    public void Damage(float damageAmount)
    {
        currentHealth -= (int)damageAmount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player has died");
    }
    #endregion
    
    #region Attack 
    //[SerializeField] private int damage = 10; // The player's damage
    
    
    private IEnumerator Attack()
    {
        // Play the attack animation
        // Wait for the attack animation to finish
        // Check for enemies in range
        // Deal damage to enemies in range
        // Return to idle state
        yield return null;
    }
    #endregion
    
}
