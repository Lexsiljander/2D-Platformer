using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class JumpingEnemy : MonoBehaviour
{
    public GameObject CoinPrefab;

    public int maxHealth = 100;
    int currentHealth;
    public bool damageTaken = false;

    public GameObject effect;
    private Rigidbody2D enemyRB;
    public bool isGrounded;

    [SerializeField]
    Transform groundCheck;

    private Collider2D col;

    [SerializeField]
    Transform player;

    [SerializeField]
    float agroRange;

   
    

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        enemyRB = GetComponent<Rigidbody2D>();

       
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        //Play hurt animation

        if (currentHealth <= 0)
        {
            Die();
            GameObject a = Instantiate(CoinPrefab, transform.position, Quaternion.identity) as GameObject;

        }
    }
    void Die()
    {
        Instantiate(effect, transform.position, Quaternion.identity);
        FindObjectOfType<AudioManager>().Play("Monster_Death");
        Destroy(gameObject);
    }
    void FixedUpdate()
    {
       
        if(Input.GetKeyDown(KeyCode.K))
        {
            JumpAttack();
        }
        if (GameObject.FindWithTag("Player") == null)
        {
            return;
        }
        //Disable enemy tracking if player is too far away
        float distToPlayer = Vector2.Distance(transform.position, player.position);

        if (distToPlayer < agroRange && isGrounded == true)
        {
            Debug.Log("UO");
            if (player.position.x >= 0.01f)
            {
                enemyRB.AddForce(new Vector2(1f, 8f), ForceMode2D.Impulse);

            }
            else if (player.position.x <= -0.01f)
            {
                enemyRB.AddForce(new Vector2(-1f, 8f), ForceMode2D.Impulse);

            }
        }

        else if (distToPlayer > agroRange)
        {
            
        }

    }
    void JumpAttack()
    {
       
       
    }


}