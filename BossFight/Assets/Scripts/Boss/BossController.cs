using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossController : MonoBehaviour
{
    //references
    [SerializeField] Animator animator;
    [SerializeField] GameObject player;
    NavMeshAgent boss;
    Health bossHealth;

    //attacking
    [SerializeField] float attackRange = 20f, meleeRange = 10f;
    [SerializeField] GameObject fireBall;
    [SerializeField] GameObject specialAttack;

    //animation
    int isWalkingHash, isRunningHash;

    float originalSpeed;


    private void Awake()
    {
        boss = GetComponent<NavMeshAgent>();
        bossHealth = GetComponent<Health>();
    }

    // Start is called before the first frame update
    void Start()
    {
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");

        originalSpeed = boss.speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (bossHealth.currentHealth >= bossHealth.maxHealth * 0.75)
            TrackPlayer();

        else if (bossHealth.currentHealth <= bossHealth.maxHealth * 0.2)
            SpecialAttack();
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
                    animator.SetTrigger("Attack1");
                    break;
                case 1:
                    animator.SetTrigger("Attack2");
                    break;
                case 2:
                    animator.SetTrigger("Attack3");
                    break;
            }
        }
    }

    void TrackPlayer()
    {
        if(!PlayerInAttackRange())
        {
            if(boss.speed > originalSpeed)
                boss.speed = originalSpeed;

            boss.SetDestination(player.transform.position);
            animator.SetBool(isWalkingHash, true);
            animator.SetBool(isRunningHash, false);
        }

        else if(PlayerInAttackRange() && !PlayerInMeleeRange())
        {
            boss.SetDestination(transform.position);
            animator.SetBool(isWalkingHash, false);
            gameObject.transform.LookAt(player.transform);
            RangedAttack();
        }

        else if (PlayerInMeleeRange())
        {
            boss.speed += 5;
            boss.SetDestination(player.transform.position);
            animator.SetBool(isWalkingHash, false);
            animator.SetBool(isRunningHash, true);


            if (Vector3.Distance(transform.position, player.transform.position) <= 4f)
            {
                boss.SetDestination(transform.position);
                animator.SetBool(isRunningHash, false);
                boss.SetDestination(transform.position);
                MeleeAttack();
            }
        }
    }

    void SpecialAttack()
    {

    }

    bool PlayerInAttackRange()
    {
        return Vector3.Distance(transform.position, player.transform.position) <= attackRange;
    }
    bool PlayerInMeleeRange()
    {
        return Vector3.Distance(transform.position, player.transform.position) <= meleeRange;
    }
}
