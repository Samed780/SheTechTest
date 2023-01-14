using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    //references
    [SerializeField] Animator animator;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MeleeAttack();

        RangedAttack();
    }

    void RangedAttack()
    {
        animator.SetTrigger("RangedAttack");
    }

    void MeleeAttack()
    {
        int i = Random.Range(0, 3);

        if (Input.GetKeyDown(KeyCode.S))
        {
            switch (i)
            {
                case 0:
                    animator.Play("Mutant Swiping");
                    break;
                case 1:
                    animator.Play("Zombie Attack");
                    break;
                case 2:
                    animator.Play("Mutant Jump Attack");
                    break;
            }
        }
    }
}
