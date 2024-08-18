using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private float _maxHealth;
    [SerializeField] private Image _healthBar;

    private float currentHealth;
    private void Awake()
    {
        currentHealth = _maxHealth;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            if(currentHealth <= 0) 
                _healthBar.enabled = false;
            StartCoroutine(TakeDamage(80));
        }
        if(Input.GetKeyDown(KeyCode.N))
        {
            StartCoroutine(RestoreHealth(10));
        }
    }

    public IEnumerator TakeDamage(float damage)
    {
        currentHealth -= damage;

        float initValue = _healthBar.fillAmount;
        float percent = currentHealth / _maxHealth;

        float iterator = 0;

        while(iterator < 1) 
        {
            _healthBar.fillAmount = Mathf.Lerp(initValue, percent, iterator);
            iterator += 0.05f;
            yield return null;
        }
    }

   

    public IEnumerator RestoreHealth(float health)
    {
        currentHealth += health;

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
