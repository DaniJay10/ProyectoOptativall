using System.Collections;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;

public class NeutralNPC : MonoBehaviour
{
    public float speed = 6;
    private Rigidbody2D rb2D;

    NavMeshAgent navMeshAgent;

    Animator animator;

    [Header("Skin")]
    public AnimatorController[] animationControllers;

    public enum NPCSkin
    {
        Purple, 
        Blue, 
        Red, 
        Yellow
    }

    public NPCSkin selectedSkin;


    //Movement type
    [Header("MovementType")]
    public MovementType movementType;

    public enum MovementType
    {
        Path,
        RandomMovement
    }


    [Header("Path")]
    public Transform[] pathPoints;
    public float waitTimeInPount = 3;
    private int indexPath = 0;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();

        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;

        animator = GetComponent<Animator>();

        ApplySkin();

        if(movementType == MovementType.Path)
        {
            StartCoroutine(FollowPath());
        }

    }

    void Update()
    {
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

    /// <summary>
    /// Cambia de skin segun la seleccionada
    /// </summary>
    public void ApplySkin()
    {
        int skinIndex = (int)selectedSkin;

        animator.runtimeAnimatorController = animationControllers[skinIndex];
    }

    /// <summary>
    /// Movimiento NPC por puntos
    /// </summary>
    /// <returns></returns>
    IEnumerator FollowPath()
    {
        while (true)
        {
            if(pathPoints.Length > 0)
            {
                navMeshAgent.SetDestination(pathPoints[indexPath].position);

                while (!navMeshAgent.pathPending && navMeshAgent.remainingDistance>0.1f)
                {
                    yield return null;
                }

                yield return new WaitForSeconds(waitTimeInPount);

                indexPath = (indexPath + 1) % pathPoints.Length;

            }
        }
    }
}
