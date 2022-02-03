using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


public class Enemy : MonoBehaviour
{
    public GameObject CoinPrefab;

    public int maxHealth = 100;
    int currentHealth;
    public bool damageTaken = false;

    public GameObject effect;
    private Rigidbody2D rb;
   

    public AIPath aiPath;


    private Collider2D col;

    [SerializeField]
    Transform player;

    [SerializeField]
    float agroRange;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
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
    void Update()
    {

        if (GameObject.FindWithTag("Player") == null)
        {
            return;
        }
        //Disable enemy tracking if player is too far away
        float distToPlayer = Vector2.Distance(transform.position, player.position);

        if(distToPlayer < agroRange)
        {
            aiPath.enabled = true;
        }

        else if (distToPlayer > agroRange)
        {
           aiPath.enabled = false;
        }

        //Sprite change direction
        if (aiPath.desiredVelocity.x >= 0.01f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (aiPath.desiredVelocity.x <= -0.01f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }


}
