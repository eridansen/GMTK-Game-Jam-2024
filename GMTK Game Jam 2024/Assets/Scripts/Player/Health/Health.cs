using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Health: MonoBehaviour
{
    [SerializeField] private Image _healthBar;
    [SerializeField] private float _maxHealth;
    [SerializeField] [Range(0.01f,0.1f)] private float _animationSpeed;
    
    private float currentHealth;
    private Coroutine _coroutine;

    private void Awake()
    {
        currentHealth = _maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if(_coroutine == null)
        {
            _coroutine = StartCoroutine(DecreaseHealthRoutine(damage));
        }
    }
    public IEnumerator DecreaseHealthRoutine(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0) 
            currentHealth = 0;

        float initValue = _healthBar.fillAmount;
        float percent = currentHealth / _maxHealth;

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
        currentHealth += health;
        if(currentHealth > _maxHealth)
            currentHealth = _maxHealth;

        float initValue = _healthBar.fillAmount;
        float percent = currentHealth / _maxHealth;

        float iterator = 0;

        while (iterator < 1)
        {
            _healthBar.fillAmount = Mathf.Lerp(initValue, percent, iterator);
            iterator += 0.05f;
            yield return null;
        }

        _coroutine = null;
    }
}
