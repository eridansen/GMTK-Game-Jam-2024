using UnityEngine;
using UnityEngine.UI;

public class Health: MonoBehaviour
{
    [SerializeField] private Image _healthBar;
    [SerializeField] private float _maxHealth;
    [Space]
    [SerializeField] private float _currentHealth;
    private Coroutine _coroutine;

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    private void UpdateHealthBar()
    {
        _healthBar.fillAmount = _currentHealth/_maxHealth;
    }
    
    public void TakeDamage(float damage)
    {
        if (_currentHealth - damage <= 0)
        {
            UpdateHealthBar();
            Die();
        }
        _currentHealth -= damage;

        UpdateHealthBar();
    }

    public void RestoreHealth(float health)
    {
        if (_currentHealth + health > _maxHealth)
        {
            _currentHealth = _maxHealth;
        }
        else
        {
            _currentHealth += health;
        }
        
        UpdateHealthBar();
    }
    
    private void Die()
    {
        Destroy(gameObject);
    }
}
