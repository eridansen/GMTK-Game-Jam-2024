using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour,IDamageable
{
    [Header("Player Stats")]
    [SerializeField] private int currentHealth = 100; // The player's health
    [SerializeField] private int maxHealth = 100; // The player's maximum health

    private PlayerMovement playerMovement;
    private void Awake() {
        playerMovement = GetComponent<PlayerMovement>();
    }
    private void Start() {
        currentHealth = maxHealth; // Set the player's health to the maximum health
    }
    private void Update() {
        Attacking();
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
    [SerializeField] private int damage = 10; // The player's damage
    public bool isAttacking = false; // Is the player attacking?
    private void Attacking()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            StartCoroutine(Attack());
        }
    }
    private IEnumerator Attack()
    {
        playerMovement.PlayAttackAnim();  // Play the attack animation
        isAttacking = true; // Set the player to attacking
        while(isAttacking){
            yield return null;// Wait for the attack animation to finish
        }
        // Check for enemies in range
        // Deal damage to enemies in range
 // Return to idle state
        yield return null;
    }

    private void AttackAnimFinished()
    {
        isAttacking = false;
    }


    #endregion
    
}
