using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float dirX, moveSpeed;
    public bool isGrounded = false;
    private Rigidbody2D rb;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    public int attackDamage = 40;

    public float attackRate = 2f;
    float nextAttackTime = 0f;

    public Animator animator;
    public bool facingRight = true;

    private bool doubleJumped;
    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 5f;

        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        dirX = Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime;
        transform.position = new Vector2(transform.position.x + dirX, transform.position.y);
    }
    // Update is called once per frame
    void Update()
    {
        Jump();

        if (dirX != 0 && !animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Punch"))
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
        if(Time.time >= nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
                FindObjectOfType<AudioManager>().Play("Player_Punch");
            }
        }
        float h = Input.GetAxis("Horizontal");
        if (h > 0 && !facingRight)
            Flip();
        else if (h < 0 && facingRight)
            Flip();

    }
    void Jump()
    {
        if (isGrounded)
            doubleJumped = false;

        if (Input.GetButtonDown("Jump") && isGrounded == true)
        {
            FindObjectOfType<AudioManager>().Play("Player_Jump");
            //gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 8f), ForceMode2D.Impulse);
            rb.AddForce(new Vector2(0f, 8f), ForceMode2D.Impulse);
        }
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * .5f);
        }
        if (Input.GetButtonDown("Jump") && !doubleJumped && !isGrounded)
        {
            FindObjectOfType<AudioManager>().Play("Player_Jump");
            //gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 8f), ForceMode2D.Impulse);
            rb.AddForce(new Vector2(0f, 7f), ForceMode2D.Impulse);
            doubleJumped = true;
        }
    }
    void Attack()
    {
        //Play an attack animation
        animator.SetTrigger("Attack");
        // Detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        // Damage them
        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

}
