using UnityEngine;

public class EnemyTNT : MonoBehaviour
{
    public float speed = 2f;
    public float range = 2f; 
    public float waitTime = 2f;

    public LayerMask playerLayer;

    private Vector2 targetPoint;
    private bool isWaiting = false;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        GetRandomPoint();
    }

    void Update()
    {
        //Collider2D player = Physics2D.OverlapCircle(transform.position, range, playerLayer);

        //if (player != null)
        //{
        //    animator.SetBool("isRunning", false);
        //    animator.SetTrigger("attack");
        //    return; 
        //}

        //if (!isWaiting)
        //{
        //    Move();
        //}
        animator.SetBool("isRunning", true);
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
        float x = Random.Range(-5f, 5f);
        float y = Random.Range(-5f, 5f);

        targetPoint = new Vector2(x, y);
    }
}
