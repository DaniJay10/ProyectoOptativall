using UnityEngine;

public class Archer : MonoBehaviour
{
    public float speed = 2f;
    public float detectionRange = 2f;
    public float waitTime = 2f;

    public LayerMask playerLayer;

    public float attackCooldown = 1f;
    private float lastAttackTime;

    private Vector2 targetPoint;
    private bool isWaiting = false;

    private Animator animator;

    //lanzamiento de flecha
    public GameObject arrowPrefab;
    public Transform firePoint;
    public float arrowForce = 5f;
    private Transform player;

    void Start()
    {
        animator = GetComponent<Animator>();
        GetRandomPoint();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        Collider2D player = Physics2D.OverlapCircle(transform.position, detectionRange, playerLayer);

        if (player != null)
        {
            animator.SetBool("isRunning", false);

            if (Time.time >= lastAttackTime + attackCooldown)
            {
                animator.SetTrigger("attack");
                lastAttackTime = Time.time;
            }

            return; 
        }

        if (!isWaiting)
        {
            Move();
        }
    }

    void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPoint, speed * Time.deltaTime);

        animator.SetBool("isRunning", true);

        if (Vector2.Distance(transform.position, targetPoint) < 0.1f)
        {
            StartCoroutine(Wait());
        }
    }

    System.Collections.IEnumerator Wait()
    {
        isWaiting = true;
        animator.SetBool("isRunning", false);

        yield return new WaitForSeconds(waitTime);

        GetRandomPoint();
        isWaiting = false;
    }

    void GetRandomPoint()
    {
        float x = Random.Range(-4f, 4f);
        float y = Random.Range(-4f, 4f);

        targetPoint = new Vector2(x, y);
    }

    public void TestEvent()
    {
        Debug.Log("FUNCIONA");
    }

    /// <summary>
    /// Lanzamiento de flecha
    /// </summary>
    public void ShootArrow()
    {
        Debug.Log("DISPARANDO");

        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);

        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();

        Vector2 direction = (player.position - firePoint.position).normalized;

        rb.linearVelocity = direction * arrowForce;
    }
}
