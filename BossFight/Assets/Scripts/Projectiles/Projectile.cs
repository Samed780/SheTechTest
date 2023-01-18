using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float life = 4f;

    //damage system
    [SerializeField] float damage = 10f;
    [SerializeField] float burnDamage = 0.1f;
    [SerializeField] float burnDuration = 5f;
    string target = "Player";


    private void Awake()
    {
        Destroy(gameObject, life);
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == target)
        {
            Destroy(gameObject);
            collision.gameObject.GetComponent<Health>().TakeDamage(damage);
            collision.gameObject.GetComponent<Health>().DamageOverSec(burnDamage, burnDuration);
        }
    }

}
