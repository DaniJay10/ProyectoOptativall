using UnityEngine;
using System.Collections;

public class DamageReceiver : MonoBehaviour
{
    [Header("Stats")]
    public int maxHealth = 20;
    private int currentHealth;

    [Header("Drop")]
    public GameObject[] itemToDrop;
    
    private Rigidbody2D rb2D;
    public float forceImpulse = 5;
    //Variable exclusiva del arbol
    private Animator animator;

    //Cambio de color al realizarse el golpe
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    Coroutine hitCoroutine;

    void Start()
    {
        currentHealth = maxHealth;
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
        StartCoroutine(HitFlash());

        if (applyForceOrNot)
        {
            rb2D.bodyType = RigidbodyType2D.Dynamic;
            rb2D.linearVelocity = Vector2.zero;
            rb2D.AddForce(hitDirection.normalized * forceImpulse, ForceMode2D.Impulse);
            Invoke("ReturnToKinematic", 0.2f);
        }
        if (applyWithAnimation)
        {
            animator.SetTrigger("Hit");
        }
        if(currentHealth <= 0)
        {
            DropItem();
            Die();
        }
    }

    void ReturnToKinematic()
    {
        rb2D.linearVelocity = Vector2.zero;
        rb2D.bodyType = RigidbodyType2D.Kinematic;
    }

    void DropItem()
    {
        //ItemToDrop se pone como array porque en el caso del arbol hay dos objetos
        for (int i = 0; i < itemToDrop.Length; i++)
        {
            Instantiate(itemToDrop[i], transform.position, Quaternion.identity);
        }
    }

    void Die()
    {
        Destroy(gameObject);
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
