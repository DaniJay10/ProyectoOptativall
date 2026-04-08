using System.Collections;
using UnityEngine;

public class DamageReceiverWarrior : MonoBehaviour
{
    [Header("Stats")]
    public int maxHealth = 50;
    private int currentHealth;

    private Rigidbody2D rb2D;
    public float forceImpulse = 2;

    private Animator animator;

    //Cambio de color al realizarse el golpe
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    Coroutine hitCoroutine;


    void Start()
    {
        currentHealth = maxHealth;
        UIManager.Instance.UpdateHealth(currentHealth);
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    /// <summary>
    /// Metodo de aplicar el daño
    /// </summary>
    public void ApplyDamage(int amount, bool applyForceOrNot, bool applyWithAnimation, Vector2 hitDirection)
    {
        currentHealth -= amount;
        UIManager.Instance.UpdateHealth(currentHealth);
        StartCoroutine(HitFlash());

        if (applyForceOrNot)
        {
            GetComponent<Player>().canMove = false;
            rb2D.AddForce(hitDirection.normalized * forceImpulse, ForceMode2D.Impulse);
            Invoke("ResetMovement", 0.2f);
        }
        if (applyWithAnimation)
        {
            animator.SetTrigger("Hit");
        }
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void ResetMovement()
    {
        GetComponent<Player>().canMove = true;
    }

    void Die()
    {
        //Reset Level
    }

    IEnumerator HitFlash()
    {
        if (spriteRenderer == null)
            yield break;

        if (hitCoroutine != null)
        {
            StopCoroutine(hitCoroutine);
        }

        hitCoroutine = StartCoroutine(HitFlashRoutine());
    }

    IEnumerator HitFlashRoutine()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = originalColor;
    }
}
