using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Health: MonoBehaviour
{
    [SerializeField] private Image _healthBar;
    [SerializeField] private float _maxHealth;
    [SerializeField] [Range(0.01f,0.1f)] private float _animationSpeed;
    [Space]
    [SerializeField] private float _currentHealth;
    private Coroutine _coroutine;

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (_currentHealth - damage < 0)
        {
            Die();
        }
        
        if(_coroutine == null)
        {
            _coroutine = StartCoroutine(DecreaseHealthRoutine(damage));
        }
    }
    public IEnumerator DecreaseHealthRoutine(float damage)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0) 
            _currentHealth = 0;

        float initValue = _healthBar.fillAmount;
        float percent = _currentHealth / _maxHealth;

        float iterator = 0;

        while (iterator < 1)
        {
            _healthBar.fillAmount = Mathf.Lerp(initValue, percent, iterator);
            iterator += 0.05f;
            yield return null;
        }

        _coroutine = null;
    }

    public void RestoreHealth(float health)
    {
        if(_coroutine == null)
        {
            _coroutine = StartCoroutine(IncreaseHealthRoutine(health));
        }
    }
    public IEnumerator IncreaseHealthRoutine(float health)
    {
        _currentHealth += health;
        if(_currentHealth > _maxHealth)
            _currentHealth = _maxHealth;

        float initValue = _healthBar.fillAmount;
        float percent = _currentHealth / _maxHealth;

        float iterator = 0;

        while (iterator < 1)
        {
            _healthBar.fillAmount = Mathf.Lerp(initValue, percent, iterator);
            iterator += 0.05f;
            yield return null;
        }

        _coroutine = null;
    }
    
    private void Die()
    {
        Destroy(gameObject);
    }
}
