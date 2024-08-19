using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;
using UnityEngine.UI;

public class CombatDummy : MonoBehaviour, IDamageable, IHealable
{

    [SerializeField] private float _currentHealth = 100; // The player's health
    [SerializeField] private float _maxHealth = 100; // The player's maximum health

    [SerializeField] private Image _healthBar;

    [SerializeField] private float _healthBarSpeed = 0.05f;

    [SerializeField] private float _timeTilHeal = 2f;
    private float healCounter = 0;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _currentHealth = _maxHealth;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Damage(30);
        }

        if (healCounter > 0)
        {
            healCounter -= Time.deltaTime;
        }
        else
        {
            if (_currentHealth < _maxHealth)
            {
                Heal(9999999);
            }
        }
    }

    public void Damage(float damageAmount)
    {
        _currentHealth -= damageAmount;
        healCounter = _timeTilHeal;
        _animator.SetTrigger("Damaged");
        StartCoroutine(UpdateHealthBarUI());
        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
        }
    }

    public void Heal(float healAmount)
    {
        if (_currentHealth == _maxHealth) return;

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

        while (iterator < 1)
        {
            _healthBar.fillAmount = Mathf.Lerp(initValue, percent, iterator);
            iterator += 0.05f;
            yield return new WaitForSeconds(_healthBarSpeed);
        }

        _healthBar.fillAmount = percent;
    }


    private void OnTriggerEnter2D(Collider2D other) {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable == null)  return;

        damageable.Damage(10);

        PlayerCombat playerCombat = other.GetComponent<PlayerCombat>();
        if(playerCombat == null) return;

        playerCombat.SetEnemyPosition(transform.position);
        
    }
}
