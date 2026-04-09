using JetBrains.Annotations;
using System.Collections;
using System.Diagnostics;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{

    protected NavMeshAgent navMeshAgent;
    protected Animator animator;
    protected Transform playerTransform;

    [Header("MovementType")]
    public MovementType movementType;
    public enum MovementType
    {
        Static,
        Path,
        RandomMovement
    }

    [Header("Skin")]
    public NPCSkin selectedSkin;
    public AnimatorController[] animationControllers;
    public enum NPCSkin
    {
        Purple,
        Blue,
        Red,
        Yellow
    }

    [Header("PathMovement")]
    public Transform[] pathPoints;
    public float waitTimeInPount = 3;
    private int indexPath = 0;
    private Coroutine currentMovementRoutine;

    [Header("RandonMovement")]
    public float movementRadius = 5f;
    public float waitTimeRandomMovement = 4f;

    protected virtual void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
        animator = GetComponent<Animator>();
        ApplySkin();

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        ResumeOriginalBehaviour();
    }

    protected virtual void Update()
    {
        AdjustAnimationAndRotation();
    }


    /// <summary>
    /// Cambia de skin segun la seleccionada
    /// </summary>
    void ApplySkin()
    {
        if(animationControllers != null && animationControllers.Length > 0)
        {
            int skinIndex = (int)selectedSkin;
            if(animator != null && skinIndex < animationControllers.Length)
            {
                animator.runtimeAnimatorController = animationControllers[skinIndex];
            }
        }
    }

    public virtual void ResumeOriginalBehaviour()
    {
        ResumeMovementBehavior(movementType);
    }

    private void ResumeMovementBehavior(MovementType type)
    {
        StopCurrentRoutine();
        switch(type)
        {
            case MovementType.Static:
                navMeshAgent.ResetPath();
                break;
            case MovementType.Path:
                StartPathRoutine();
                break;
            case MovementType.RandomMovement:
                StartRandomRoutine();
                break;
            
        }
    }

    public void StopCurrentRoutine()
    {
        if (currentMovementRoutine != null)
        {
            StopCoroutine(currentMovementRoutine);
            currentMovementRoutine = null;
        }
    }

    protected void StartPathRoutine()
    {
        StopCurrentRoutine();
        currentMovementRoutine = StartCoroutine(FollowPath());

    }

    protected void StartRandomRoutine()
    {
        StopCurrentRoutine();
        currentMovementRoutine = StartCoroutine(RandonMovement());
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
    /// Movimiento NPC por puntos
    /// </summary>
    /// <returns></returns>
    protected IEnumerator FollowPath()
    {
        while (true)
        {
            if (pathPoints.Length > 0)
            {
                navMeshAgent.SetDestination(pathPoints[indexPath].position);

                yield return WaitUntilDestinationReached();
                yield return new WaitForSeconds(waitTimeInPount);

                indexPath = (indexPath + 1) % pathPoints.Length;

            }
        }
    }

    protected IEnumerator RandonMovement()
    {
        while (true)
        {
            Vector3 randomPos = GetRandomNavMeshPosition();
            navMeshAgent.SetDestination(randomPos);
            yield return WaitUntilDestinationReached();
            yield return new WaitForSeconds(waitTimeRandomMovement);
        }
    }

    private Vector3 GetRandomNavMeshPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * movementRadius + transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, movementRadius, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return transform.position; 
    }

    private IEnumerator WaitUntilDestinationReached()
    {
        while (!navMeshAgent.pathPending && navMeshAgent.remainingDistance > 0.1f)
        {
            yield return null;
        }
    }

}

