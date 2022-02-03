using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin_Collectable : MonoBehaviour
{
    private Collider2D col;
    public int coinValue = 1;


    // Update is called once per frame
    void Update()
    {
        
    }

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
        //Checks to see if this gameObject is in fact colliding with the player
        if (collision.gameObject == player)
        {
            FindObjectOfType<AudioManager>().Play("Coin_Pickup");
            ScoreManager.instance.ChangeScore(coinValue);
            Destroy(gameObject);
        }
    }
}
