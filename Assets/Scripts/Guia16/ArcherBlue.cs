using UnityEngine;

public class ArcherBlue : MonoBehaviour
{
    public GameObject arrowPrefab;
    public Transform firePoint;
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("Attack");
        }
    }

    public void Shoot()
    {
        Debug.Log("Entro a shoot");

        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);

        Arrow arrowScript = arrow.GetComponent<Arrow>();

        if (transform.localScale.x > 0)
        {
            arrowScript.direction = Vector2.right;
        }
        else
        {
            arrowScript.direction = Vector2.left;
        }
    }
}