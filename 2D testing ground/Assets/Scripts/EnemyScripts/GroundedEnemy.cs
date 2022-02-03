using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedEnemy : MonoBehaviour
{
    public GameObject Enemy;
    // Start is called before the first frame update
    void Start()
    {
        Enemy = gameObject.transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            Enemy.GetComponent<JumpingEnemy>().isGrounded = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            Enemy.GetComponent<JumpingEnemy>().isGrounded = false;
        }
    }
}
