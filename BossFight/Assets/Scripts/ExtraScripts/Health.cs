using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] float maxHealth;
    [SerializeField] float currentHealth;

    //every 1 armor is 2% less damage 
    public float armor;


    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (currentHealth > 0)
        {
            if (armor < 50)
            {
                damage -= armor * damage * 0.02f; 
                currentHealth -= damage;
            }
            else
                damage = 0;
        }
    }
}
