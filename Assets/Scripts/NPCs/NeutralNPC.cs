using UnityEngine;
using UnityEngine.AI;

public class NeutralNPC : MonoBehaviour
{
    public float speed = 6;
    private Rigidbody2D rb2D;
    public Transform targetTransform;

    NavMeshAgent navMeshAgent;

    Animator animator;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();

        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;

        animator = GetComponent<Animator>();

    }

    void Update()
    {
        navMeshAgent.SetDestination(targetTransform.position);
        AdjustAnimationAndRotation();
    }

    public void AdjustAnimationAndRotation()
    {
        //movimiento
        bool isMoving = navMeshAgent.velocity.magnitude > 0.1f;
        animator.SetBool("isRunning", isMoving);

        //rotacion imagen
        if (navMeshAgent.velocity.x > 0.01f)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (navMeshAgent.velocity.x < -0.1f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}
