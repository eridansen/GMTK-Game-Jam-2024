using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombat : MonoBehaviour,IDamageable,IHealable
{
    [Header("Health Stats")]
    [SerializeField] private float _currentHealth = 100; // The player's health
    [SerializeField] private float _maxHealth = 100; // The player's maximum health

    private PlayerMovement playerMovement;


    private void Awake() {
        playerMovement = GetComponent<PlayerMovement>();
    }
    private void Start() {
        _currentHealth = _maxHealth; // Set the player's health to the maximum health
        _healthBar = GameObject.FindGameObjectWithTag("Player Health").GetComponentInChildren<Image>();

    }
    private void Update() {
        Attacking();
     
        
        if(Input.GetKeyDown(KeyCode.B))
        {
            Damage(30);
        }
        if(Input.GetKeyDown(KeyCode.N))
        {
            Heal(30);
        }
    }

    #region Health

    [Header("Health Bar Settings")]
    [SerializeField] private Image _healthBar;
    [SerializeField] private float _healthBarSpeed = 0.05f;

    public void Damage(float damageAmount)
    {
        _currentHealth -= damageAmount;
        StartCoroutine(UpdateHealthBarUI());
        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            Die();
        }
    }

    public void Heal(float healAmount)
    {
        if(_currentHealth == _maxHealth) return;
        
        _currentHealth += healAmount;
        if (_currentHealth > _maxHealth)
        {
            _currentHealth = _maxHealth;
        }
        StartCoroutine(UpdateHealthBarUI());
    }


    public IEnumerator UpdateHealthBarUI()
    {

        float initValue = _healthBar.fillAmount;
        float percent = _currentHealth / _maxHealth;


        float iterator = 0;

        while(iterator < 1) 
        {
            _healthBar.fillAmount = Mathf.Lerp(initValue, percent, iterator);
            iterator += 0.05f;
            yield return new WaitForSeconds(_healthBarSpeed);
        }

        _healthBar.fillAmount = percent;
    }


    private void Die()
    {
        Debug.Log("Player has died");
        _healthBar.enabled = false;
    }
    #endregion
    
    #region Attack 
    [Header("Combat Stats")]
    [SerializeField] private int _damage = 10; // The player's damage

    public bool isAttacking = false; // Is the player attacking?
    private void Attacking()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Attack();
        }
    }
    private void Attack()
    {
        playerMovement.PlayAttackAnim();  // Play the attack animation
        isAttacking = true; // Set the player to attacking
    }


    public void Hit(IDamageable damageable)
    {
        damageable.Damage(_damage); // Deal damage to the damageable - allows for configuring light and heavy attacks - deal damage based on current attack swing
    }
    private void AttackAnimFinished()
    {
        isAttacking = false;
    }


    #endregion
    
}
