using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Health: MonoBehaviour
{
    [SerializeField] private Image _healthBar;
    [SerializeField] private float _maxHealth;
    [SerializeField] [Range(0.01f,0.1f)] private float _animationSpeed;
    
    private float currentHealth;


    private void Awake()
    {
        currentHealth = _maxHealth;
    }

    public IEnumerator TakeDamage(float damage)
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
    }

    public IEnumerator RestoreHealth(float health)
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
    }
}
