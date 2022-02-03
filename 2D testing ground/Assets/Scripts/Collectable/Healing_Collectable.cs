using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healing_Collectable : MonoBehaviour
{
    //The amount of damage that needs to be negated from the player's currentHealth value
    [SerializeField]
    private int healingAmount;
    //The collider of whatever gameObject has this script on it
    private Collider2D col;

 
    private void OnEnable()
    {
        //A reference to the collider of whatever gameObject would apply damage; for the sake of the tutorial, this is on the red square, but in most games this script would be on a projectile that is fired or a melee weapon as the hitboxes are active.
        col = GetComponent<Collider2D>();
        //A quick line that ensures whatever gameObject has this script also has it's collider set to trigger so the logic that causes damage can flow if you forget to set it as a trigger collider in the inspector
        col.isTrigger = true;
    }

    //This method is called when this object enters the player
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Quick reference to the player so the game is better optimized
        GameObject player = GameObject.FindWithTag("Player");
        //Quick reference to the Health script on the player for optimization
        Health health = player.GetComponent<Health>();
        //Checks to see if this gameObject is in fact colliding with the player
        if (collision.gameObject == player)
        {
            FindObjectOfType<AudioManager>().Play("Health_Pickup");
            health.heal(healingAmount);
            Destroy(gameObject);
        }
    }
}