using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthBar;

    public int maxHealth = 1000;
    public int Health;

    // Start is called before the first frame update
    void Start()
    {
        Health = maxHealth;
        SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMaxHealth(int health)
    {
        healthBar.maxValue = health;
        healthBar.value = health;
    }

    public void SetHealth(int health)
    {
        healthBar.value = health;
    }

    public void DoDamage(int damage)
    {
        Health -= damage;
        if (Health < 0)
        {
            Health = 0;
        }
        SetHealth(Health);
    }

    public void DoHealing(int heal)
    {
        Health += heal;
        if (Health > maxHealth)
        {
            Health = maxHealth;
        }
        SetHealth(Health);
    }
}
