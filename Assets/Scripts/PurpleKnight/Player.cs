using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Player : MonoBehaviour
{

    public float speed = 5;
    Rigidbody2D rb2D;
    Vector2 movementInput;
    private Animator animator;
    private bool gameIsPaused = false;

    // Animacion de ataque
    private bool isAttacking = false;

    [HideInInspector]
    public  bool canMove = true;
    Vector2 lastMovementDir = Vector2.right;
    Vector2 AttackDirection;
    public float AttackRange = 1.2f;
    public int attackDamage = 10;

    // Ataque a objetos
    public LayerMask targetLayer;

    private int xp = 0;
    [HideInInspector]
    public int currentLevel = 1;

    void Start()
    {
        //Como es una variable publica, se puede asignar desde el inspector, pero también se puede hacer por código
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        UIManager.Instance.UpdatePlayerStats(xp, currentLevel, speed, attackDamage);
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
        OpenCloseStatsPlayer();

        if (movementInput != Vector2.zero)
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

    ///<summary>
    /// Abrir y cerar stats del jugador
    /// </summary>
    void OpenCloseStatsPlayer()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            UIManager.Instance.OpenOrCloseStatsPlayer();
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
        rb2D.linearVelocity = Vector2.zero;
        canMove = false;
    }

    /// <summary>
    /// Metodo finalizar ataque
    /// </summary>
    public void EndAttack()
    {
        isAttacking = false;
        canMove = true;
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
                obj.GetComponent<DamageReceiver>().ApplyDamage(attackDamage, true, false, hitDirection);
            }
            //Arbol
            else if (layer == LayerMask.NameToLayer("Tree"))
            {
                obj.GetComponent<DamageReceiver>().ApplyDamage(attackDamage, false, true, hitDirection);
            }
            else
            {
                Debug.LogWarning("Objeto con layer no valida");
            }
        }
    }


    private void OnEnable()
    {
        DamageReceiver.OnTargetKilled += AddExp;
    }

    private void OnDisable()
    {
        DamageReceiver.OnTargetKilled -= AddExp;
    }

    public void AddExp(int xpAmount)
    {
        Debug.Log("Entro a AddExp");
        xp += xpAmount;
        if (xp >= 100)
        {
            Debug.Log("¡SUBIENDO DE NVIEL!");
            xp -= 100;
            LevelUp();
        }
        UIManager.Instance.UpdatePlayerStats(xp, currentLevel, speed, attackDamage);
    }

    private void LevelUp()
    {
        Debug.Log("Entro a Level Up");
        currentLevel++;

        switch (currentLevel)
        {
            case 2:
                speed += 1;
                attackDamage += 1;
                GetComponent<DamageReceiverWarrior>().GainHealth(1);
                break;
            case 3:
                speed += 1;
                attackDamage += 1;
                GetComponent<DamageReceiverWarrior>().GainHealth(1);
                break;
            case 4:
                speed += 1;
                attackDamage += 1;
                GetComponent<DamageReceiverWarrior>().GainHealth(1);
                break;
            case 5:
                speed += 1;
                attackDamage += 1;
                GetComponent<DamageReceiverWarrior>().GainHealth(1);
                break;
            case 6:
                speed += 1;
                attackDamage += 1;
                GetComponent<DamageReceiverWarrior>().GainHealth(1);
                break;
        }
    }

}
