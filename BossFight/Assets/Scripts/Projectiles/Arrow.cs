using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] float life = 3f;

    //damage system
    [SerializeField] float damage = 5f;
    [SerializeField] float armorPenetration = 0.1f;
    //[SerializeField] float numberOfHits = 0;
    string target = "Boss";


    private void Awake()
    {
        Destroy(gameObject, life);
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if(collision.gameObject.tag == target)
        {
            Destroy(gameObject);
            collision.gameObject.GetComponent<Health>().ArmorPen(armorPenetration);
            collision.gameObject.GetComponent<Health>().TakeDamage(damage);
        }

    }
}
