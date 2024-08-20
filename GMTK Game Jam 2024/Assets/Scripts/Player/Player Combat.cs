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
    [SerializeField] private float _healAmount = 30; // The player's maximum health
    public Slider healthBar;
    public Slider adBar;
    public int maxHealth = 1000;
    public int currentHealth;
    public int maxAD = 1000;
    public int currentAD;

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

        
        if(Input.GetKeyDown(KeyCode.H))
        {
            Heal(_healAmount);
        }
        if (invincibilityCounter > 0){            
            invincibilityCounter -= Time.deltaTime; // Decrease the invincibility counter
        }
    }

    #region Health

    [Header("Health Bar Settings")]
    [SerializeField] private Image _healthBar;
    [SerializeField] private float _healthBarSpeed = 0.05f;


    [Header("Hurt Settings")]
    [SerializeField] private AudioClip[] _hurtSounds; // The player's hurt sounds
    [SerializeField] private float invincibilityTime = 1f; // The time the player is invincible after being hit
    [SerializeField] private Vector2 knockbackForce = new Vector2(15f, 5f); // The force of the knockback

    [HideInInspector] public Vector3 enemyPosition = Vector3.zero; // The position of the enemy that hit the player 

    private float invincibilityCounter = 0; // The counter for the invincibility time

    [Header("Healing Particles")]
    [SerializeField] private ParticleSystem _healParticles; // The player's healing particles
    [SerializeField] private Transform healParticlePosition; // The position of the healing particles

    public void Damage(float damageAmount)
    {
        if (invincibilityCounter > 0) return; // If the player is invincible, return

        invincibilityCounter = invincibilityTime; // Set the invincibility counter to the invincibility time
        playerMovement.PlayHurtAnim(); // Play the hit animation
        AudioManager.Instance.PlayRandomSoundFXClip(_hurtSounds, transform, 0.5f); // Play a random hurt sound

        ApplyKnockback(); // Apply the knockback
        _currentHealth -= damageAmount;
        StartCoroutine(UpdateHealthBarUI());
        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            Die();
        }
    }

    public void SetEnemyPosition(Vector3 enemyPos)
    {
        enemyPosition = enemyPos;
    }
    void ApplyKnockback()
    {
        Vector3 direction = transform.position - enemyPosition; // Get the direction of the knockback
        direction.Normalize(); // Normalize the direction
        direction = -direction; // Invert the direction
        StartCoroutine(playerMovement.Knockback(direction, knockbackForce, 0.1f)); // Apply the knockback
    }



    public void SetHealth(int health)
    {
        healthBar.value = health;
    }

    public void SetAD(int AD)
    {
        adBar.value = AD;
    }

    public void SetMaxHealth(int maxHealth)
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = maxHealth;
    }

    public void SetMaxAD(int maxAD)
    {
        adBar.maxValue = maxAD;
        adBar.value = maxAD;
    }

    public void DoHeal(int heal)
    {
        currentHealth += heal;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

    }

    public void AddAD(int addedAD)
    {
        currentAD += addedAD;
        if (currentAD > maxAD)
        {
            currentAD = maxAD;
        }

    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

    }

    public void LoseAD(int lostAD)
    {
        currentAD -= lostAD;
        if (currentAD < 0)
        {
            currentAD = 0;
        }

    }



    public void Heal(float healAmount)
    {
        if(_currentHealth == _maxHealth) return;

        playerMovement.PlayParticleEffectInstance(healParticlePosition.position,_healParticles);
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
        playerMovement.PlayDeathAnim();
        _healthBar.enabled = false;
    }
    #endregion
    
    #region Attack 
    [Header("Combat Stats")]
    [SerializeField] public float _damage = 10; // The player's damage
    [SerializeField] private AudioClip[] _attackSounds; // The player's attack sounds

    public bool isAttacking = false; // Is the player attacking?
    private void Attacking()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if(isAttacking || playerMovement.isHurt || playerMovement.isDashing) return;
            Attack();
            AudioManager.Instance.PlayRandomSoundFXClip(_attackSounds, transform, 0.5f);
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
