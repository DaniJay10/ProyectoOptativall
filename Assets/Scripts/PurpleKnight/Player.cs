using UnityEngine;

public class Player : MonoBehaviour
{

    public float speed = 5;
    Rigidbody2D rb2D;
    Vector2 movementInput;
    private Animator animator;
    private int currentHealth;
    private int maxHealth = 100;
    private bool gameIsPaused = false;

    // Ataque
    private bool isAttacking = false;
    private bool canMove = true;


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
            int randomIndex = Random.Range(0, 2);
            animator.SetInteger("AttackIndex", randomIndex);
            animator.SetTrigger("DoAttack");
        }
    }

    public void StartAttack()
    {
        isAttacking = true;
    }

    public void EndAttack()
    {
        isAttacking = false;
    }

}
