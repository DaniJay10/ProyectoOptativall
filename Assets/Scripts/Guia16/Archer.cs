using UnityEngine;
using System.Collections;

public class Archer : MonoBehaviour
{
    public GameObject arrowPrefab;
    public Transform firePoint;

    private Animator animator;
    private bool isAttacking = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isAttacking)
        {
            StartCoroutine(AttackRoutine());
        }
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;

        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(0.3f); 

        Shoot();

        yield return new WaitForSeconds(0.5f);

        isAttacking = false;
    }

    void Shoot()
    {
        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);

        Arrow arrowScript = arrow.GetComponent<Arrow>();

        if (transform.localScale.x > 0)
            arrowScript.direction = Vector2.right;
        else
            arrowScript.direction = Vector2.left;
    }
}