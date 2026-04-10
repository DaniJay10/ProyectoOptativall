using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyTorch : NPC
{

    public float attackRange = 2;
    public float stopDistance = 0.5f;
    public float attackCooldown = 2f;
    private float lastAttackTime = 0f;

    //saber si estamos atacando o no
    private bool isAttacking = false;
    private bool canMove = true;

    public LayerMask targetLayer;
    private Vector2 playerDirection;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        if (playerTransform == null) return;

        float distance = Vector3.Distance(transform.position, playerTransform.position);

        // Solo ataca si est� suficientemente cerca
        if (distance <= attackRange && !isAttacking && Time.time >= lastAttackTime + attackCooldown)
        {
            AttackPlayer();
            lastAttackTime = Time.time;
        }
    }


    /// <summary>
    /// Metodo de ataque al jugador
    /// </summary>
    private void AttackPlayer()
    {
        isAttacking = true;
        canMove = false;
        navMeshAgent.ResetPath();

        playerDirection = playerTransform.position - transform.position;

        int attackDirection = GetAttackDirection(playerDirection);

        if (playerTransform.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        animator.SetInteger("AttackDirection", attackDirection);
        animator.SetTrigger("DoAttack");

        Invoke("ResetAttack", 0.5f);

    }

    private void ResetAttack()
    {
        isAttacking = false;
        canMove = true;
    }

    private void FixedUpdate()
    {
        navMeshAgent.isStopped = !canMove;
    }

    private int GetAttackDirection(Vector2 Direction)
    {
        if (Mathf.Abs(Direction.x) > Mathf.Abs(Direction.y))
        {
            return Direction.x > 0 ? 0 : 1;
        }
        else
        {
            return Direction.y > 0 ? 2 : 3;
        }
    }

    /// <summary>
    /// Metodo golpear a enemigos/animales/arboles
    /// </summary>
    public void DetectAndDamageTargets()
    {


        Vector2 AttackPoint = (Vector2)transform.position + playerDirection.normalized * attackRange * 0.5f;
        Collider2D[] hitTarget = Physics2D.OverlapCircleAll(AttackPoint, attackRange, targetLayer);



        HashSet<GameObject> damagedTargets = new HashSet<GameObject>();

        foreach (Collider2D target in hitTarget)
        {


            GameObject obj = target.gameObject;
            int layer = obj.layer;
            if (damagedTargets.Contains(obj))
            {
                continue; //Como el jugador tiene 2 collider hay que evitar que ataque dos veces por cada golpe
            }

            //Jugador
            if (layer == LayerMask.NameToLayer("Player"))
            {
                Vector2 hitDirection = target.transform.position - transform.position;

                obj.GetComponent<DamageReceiverWarrior>().ApplyDamage(10, true, false, hitDirection);
            }
        }
    }
}
