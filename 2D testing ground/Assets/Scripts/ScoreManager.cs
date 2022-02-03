using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    [SerializeField]
    public Text text;
    public int score;
    SaveSystem scoreSystem;

    [SerializeField]
    private int damageAmount;
    //The collider of whatever gameObject has this script on it
    private Collider2D col;

    public Animator transition;

    public float transitionTime = 1f;



    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        scoreSystem = FindObjectOfType<SaveSystem>();
        score = scoreSystem.CurrentScore;
        text.text = score + "";

    }

   public void ChangeScore(int coinValue)
   {
        score += coinValue;
        text.text = "X" + score.ToString();
   }
   

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

        /*scoreSystem = FindObjectOfType<SaveSystem>();
        score = scoreSystem.CurrentScore;
        text.text = score + "";*/
    }
    //This method is called when this object enters the player
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Quick reference to the player so the game is better optimized
        GameObject player = GameObject.FindWithTag("Player");

        //Checks to see if this gameObject is in fact colliding with the player
        if (collision.gameObject == player)
        {
            LoadNextLevel();
            scoreSystem.SetScore(score);
        }
        void LoadNextLevel()
        {
            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
        }

        IEnumerator LoadLevel(int levelIndex)
        {
            transition.SetTrigger("Start");

            yield return new WaitForSeconds(transitionTime);

            SceneManager.LoadScene(levelIndex);
        }
    }
}
