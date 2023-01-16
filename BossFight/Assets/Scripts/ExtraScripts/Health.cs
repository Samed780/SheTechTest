using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] float maxHealth;
    [SerializeField] float currentHealth;

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
            }
            else
                damage = 0;
        }
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

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<Arrow>() != null)
            numberOfHits++;
    }
}
