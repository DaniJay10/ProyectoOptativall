using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Player : MonoBehaviour
{

    public float speed = 5;
    Rigidbody2D rb2D;
    Vector2 movementInput;
    private Animator animator;
    private int currentHealth;
    public int maxHealth = 50;
    private bool gameIsPaused = false;

    // Animacion de ataque
    private bool isAttacking = false;
    private bool canMove = true;
    Vector2 lastMovementDir = Vector2.right;
    Vector2 AttackDirection;
    public float AttackRange = 1.2f;

    // Ataque a objetos
    public LayerMask targetLayer;

    void Start()
    {
        //Como es una variable publica, se puede asignar desde el inspector, pero también se puede hacer por código
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        UIManager.Instance.UpdateHealth(currentHealth);
    }

    void Update()
    {
        movementInput.x = Input.GetAxisRaw("Horizontal");
        movementInput.y = Input.GetAxisRaw("Vertical");

        movementInput = movementInput.normalized;

        //cambio de animaciones
        animator.SetFloat("Horizontal", Mathf.Abs(movementInput.x));
        animator.SetFloat("Vertical", Mathf.Abs(movementInput.y));

        CheckFlip();
        OpenCloseInventory();
        OpenClousePauseMenu();

        //Ataque
        if (isAttacking)
        {
            canMove = false;
        }
        else
        {
            canMove = true;
        }

        if(movementInput != Vector2.zero)
        {
            lastMovementDir = movementInput;
        }

        Attack();
    }


    private void FixedUpdate()
    {
        if (canMove)
        {
            rb2D.linearVelocity = movementInput * speed;
        }
        else
        {
            rb2D.linearVelocity = Vector2.zero;
        }
        
       
    }

    /// <summary>
    /// Metodo de cambiar la orientacion segun el movimiento
    /// </summary>
    void CheckFlip()
    {
        if(movementInput.x > 0 && transform.localScale.x < 0 || movementInput.x < 0 && transform.localScale.x > 0)
        {
            //Al multiplicar por -1, se invierte el valor, es decir, si era positivo, se vuelve negativo y viceversa
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
        
    }

    ///<summary>
    /// Abrir y cerar inventario
    /// </summary>
    void OpenCloseInventory()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            UIManager.Instance.OpenOrCloseInventory();
        }
    
    }

    /// <summary>
    /// Metodo para abrir y cerrar inventario con P
    /// </summary>
    void OpenClousePauseMenu()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            if(gameIsPaused)
            {
                UIManager.Instance.ResumeGame();
                gameIsPaused = false;
            }
            else
            {
                UIManager.Instance.PauseGame();
                gameIsPaused = true;
            }
        }
    }

    /// <summary>
    /// Metodo de ataque
    /// </summary>
    void Attack()
    {
        if(Input.GetMouseButtonDown(0) && !isAttacking)
        {
            int direction = GetDirectionIndex(lastMovementDir);
            AttackDirection = GetAttackInputDirection();
            int attackDirection = GetDirectionIndex(AttackDirection);

            animator.SetInteger("AttackDirection", attackDirection);


            int randomIndex = Random.Range(0, 2);
            animator.SetInteger("AttackIndex", randomIndex);
            animator.SetTrigger("DoAttack");
        }
    }

    /// <summary>
    /// Metodo iniciar ataque
    /// </summary>
    public void StartAttack()
    {
        isAttacking = true;
    }

    /// <summary>
    /// Metodo finalizar ataque
    /// </summary>
    public void EndAttack()
    {
        isAttacking = false;
    }

    Vector2 GetAttackInputDirection()
    {
        Vector2 InputDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        if(InputDirection != Vector2.zero)
        {
            return InputDirection;
        }
        else
        {
            if (transform.localScale.x > 0)
            {
                return Vector2.right;
            }
            else
            {
                return Vector2.left;
            }
        }
    }

    int GetDirectionIndex(Vector2 Direction)
    {
        if(Mathf.Abs(Direction.x) > Mathf.Abs(Direction.y))
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
        Vector2 AttackPoint = (Vector2)transform.position + AttackDirection * AttackRange * 0.5f;
        Collider2D[] hitTarget = Physics2D.OverlapCircleAll(AttackPoint, AttackRange, targetLayer);

        foreach(Collider2D target in hitTarget)
        {
            Vector2 hitDirection = target.transform.position - transform.position;
            GameObject obj = target.gameObject;
            int layer = obj.layer;

            //Enemigo y obveja
            if (layer == LayerMask.NameToLayer("Enemy") || layer == LayerMask.NameToLayer("Sheep"))
            {
                target.GetComponent<DamageReceiver>().ApplyDamage(10, true, false, hitDirection);
            }
            //Arbol
            else if (layer == LayerMask.NameToLayer("Tree"))
            {
                target.GetComponent<DamageReceiver>().ApplyDamage(10, false, true, hitDirection);
            }
        }
    }

}
