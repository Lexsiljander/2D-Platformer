using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Health : MonoBehaviour
{
   
    [HideInInspector]
    public bool takingDamage;
    [HideInInspector]
    public bool isDead;
    private Animator anim;
    private Rigidbody2D rb;

    public Health_bar healthBar;

    public GameObject effect;
    

    //The maximum number of health points the player can have
    [SerializeField]
    private int maxHealthPoints;
    //How high the player goes when they receive damage
    [SerializeField]
    private float verticalKnockbackForce;
    //How far backwards the player goes when they receive damage
    [SerializeField]
    private float horizontalKnockbackForce;
    //The max amount of time after receiving damage that the player can no longer receive damage
    [SerializeField]
    private float invulnerabilityTime;
    //How long movement should be disabled after receiving damage
    [SerializeField]
    private float cancelMovementTime;

    //Bool that manages if the player can receive more damage
    [HideInInspector]
    public bool hit;
    //A reference point of whatever caused damage so the player can knockback in the appropriate direction
    [HideInInspector]
    public GameObject enemy;

    //The current number of health points on the player after damage is applied
    public static int currentHealthPoints;
    //Unique for this solution if you player uses a CapsuleCollider2D; if you don't have a CapsuleCollider2D, you probably won't need to reference you exact collider type as you don't need to change the direction
    private CapsuleCollider2D playerCollider;



    


    private void Start()
    {
        //This is for testing and certain games that refil health when starting a new scene; a lot of games currentHealth doesn't reset to maxHealth at the start of each scene, but for this tutorial it helps manage the values. I would use a PlayerPref int value to manage currentHealth between scenes
        if(currentHealthPoints == 0)
        {
          currentHealthPoints = maxHealthPoints; 
        }
       
        //Unique for this solution; I need to reference this Collider type because I need to change the direction of the CapusleCollider2D through code, I can't do it through the animator
        playerCollider = GetComponent<CapsuleCollider2D>();
        //Reference to the Rigidbody2D component to handle knockback force
        rb = GetComponent<Rigidbody2D>();
        //Reference to the Animator component to play animations
        anim = GetComponent<Animator>();

        healthBar.SetMaxHealth(maxHealthPoints);

        
     
    }
    void Update()
    {
       
    }

    private void FixedUpdate()
    {
        //The hit bool is set to true in the DamageField script, and changed to false later in this script; this manages if knockback should be applied
        if (hit)
        {
            if (enemy == null)
                return;
            HandleKnockBack();
        }
        
    }

    //This method is called by any script that would need to handle damage; for this tutorial it is called by the DamageField script
    public void Damage(int amount)
    {
        //First checks to see if the player is currently in an invulnerable state; if not it runs the following logic.
        if (!hit)
        {
            //First sets invulnerable to true
            hit = true;
            //Reduces currentHealthPoints by the amount value that was set by whatever script called this method, for this tutorial in the OnTriggerEnter2D() method
            currentHealthPoints -= amount;
            healthBar.SetHealth(currentHealthPoints);
            Damagecolor();
            GetComponent<PlayerController>().enabled = false;
            FindObjectOfType<AudioManager>().Play("Player_Hurt");
            //If currentHealthPoints is below zero, player is dead, and then we handle all the logic to manage the dead state
            if (currentHealthPoints <= 0)
            {
                FindObjectOfType<AudioManager>().Play("Player_Death");
                Instantiate(effect, transform.position, Quaternion.identity);
                FindObjectOfType<SaveSystem>().SetScore(0);
                StartCoroutine(RestartScreen());
                GetComponent<SpriteRenderer>().enabled = false;
                GetComponent<BoxCollider2D>().enabled = false;
                GetComponent<PlayerController>().enabled = false;
                currentHealthPoints = maxHealthPoints;
                //Unique for my solution in which I use a CapsuleCollider2D; this sets the direction of the CapsuleCollider2D horizontally for the animation of the player dying, this is unique only if you player has a CapsuleCollider2D
                //playerCollider.direction = CapsuleDirection2D.Horizontal;
                //A bool in the Character script is set to true so other scripts can manage logic differently if the player is dead; if you don't have a Character script, then use the commented out variables I provided in this solution so you can manage this state
                //character.isDead = true;
                //Play the dead animation
                //anim.SetBool("Dead", true);
            }
        }
    }

    //This method will move the player in a backwards slightly upwards direction when taking damage; this is a very common feature in nearly all platformers
    private void HandleKnockBack()
    {
        //Bool that lets other scripts know the player is currently taking damage; if you don't have a Character script, then use the commented out variables I provided in this solution so you can manage this state
        this.takingDamage = true;
        //Plays the damage animation
        //anim.SetBool("Damage", true);
        //Adds a slightly upwards knockback force, this value probably shouldn't be as strong as the horizontal
        rb.AddForce(Vector2.up * verticalKnockbackForce);
        //This if statement checks to see if you are facing left or right when taking damage; depending on what direction you are facing, the knockback force will be applied appropriately backwards 
        if (transform.position.x < enemy.transform.position.x)
        {
            //If the player is facing right, then backwards knockback would be going to the left
            rb.AddForce(Vector2.left * horizontalKnockbackForce, ForceMode2D.Impulse);
        }
        else
        {
            //If the player is facing left, then backwards knockback would be going to the right
            rb.AddForce(Vector2.right * horizontalKnockbackForce, ForceMode2D.Impulse);
        }
        
        //This method is called very quickly to stop knockback forces from being applied
        Invoke("CancelHit", invulnerabilityTime);
        //This method is called less quickly to allow player control again after taking damage
        Invoke("EnableMovement", cancelMovementTime);
    }

    //Method that changes the hit value back to false to stop knockback forces from constantly being applied
    private void CancelHit()
    {
        hit = false;
    }

    //Method that allows player movement again after taking damage; also turns off the animation for taking damage if the player is still alive and gets the player out of the taking damage state as well.
    private void EnableMovement()
    {
        if (!this.isDead)
        {
            //anim.SetBool("Damage", false);
            this.takingDamage = false;
        }
    }
    private void Damagecolor()
    {
        if (takingDamage = true)
        {
            GetComponent<SpriteRenderer>().color = Color.red;

            StartCoroutine(Recovery());
        }
        
    }
    IEnumerator Recovery()
    {
        

        //yield on a new YieldInstruction that waits for 1 seconds.
        yield return new WaitForSeconds(1);

        GetComponent<SpriteRenderer>().color = Color.white;
        GetComponent<PlayerController>().enabled = true;

    }
    public void heal(int amount)
    {
        if (maxHealthPoints > currentHealthPoints)
        {
            currentHealthPoints += amount;
            healthBar.SetHealth(currentHealthPoints);
        }
        
    }

    IEnumerator RestartScreen()
    {
        //yield on a new YieldInstruction that waits for 1 seconds.
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("GameOver");
        Destroy(gameObject);
    }
}
