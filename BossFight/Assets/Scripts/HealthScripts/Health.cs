using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    //references
    [SerializeField] Animator animator;

    public float maxHealth;
    public float currentHealth;

    //every 1 armor is 2% less damage 
    public float armor;
    float numberOfHits = 0;



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
                animator.SetTrigger("Damage");
            }
            else
                damage = 0;
        }
        else
            Death();
    }

    public void ArmorPen(float armorPen)
    {
        switch (numberOfHits)
        {
            case 5:
                armor -= armor * armorPen;
                break;
            case 10:
                armor -= armor * 2 * armorPen;
                break;
            case 20:
                armor -= armor * 5 * armorPen;
                break;
        }
    }

    public void DamageOverSec(float burnDamage, float duration)
    {
        float timer = 0f;

        while(timer < duration)
        {
            currentHealth -= burnDamage;
            timer += Time.time; 
        }
    }

    void Death()
    {
        animator.SetTrigger("Death");
        Destroy(gameObject, 5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<Arrow>() != null)
            numberOfHits++;
    }
}
