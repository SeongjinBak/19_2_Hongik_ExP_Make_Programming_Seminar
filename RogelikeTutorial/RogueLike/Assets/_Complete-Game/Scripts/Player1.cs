using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class Player1 : MovingObject1
{
    public Text foodText;
    public int wallDamage = 1;
    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;
    public float restartLevelDelay = 1f;
    private Animator animator;

    private int food;

    // Start is called before the first frame update
    protected override void Start()
    {
        animator = GetComponent<Animator>();
        food = GameManager1.instance.playerFoodPoints;
        foodText.text = "Food : " + food;
        base.Start();
    }


    // 이 스크립트가 활성화 되었을 때
    private void OnEnable()
    {

    }

    protected override void AttemptMove<T> (int xDir, int yDir)
    {
        food--;
        base.AttemptMove<T>(xDir, yDir);
        foodText.text = "Food : " + food;

        RaycastHit2D hit;
        CheckIfGameOver();
        GameManager1.instance.playersTurn = false;
    }


    // 이 스크립트가 비활성화 되었을 때
    private void OnDisable()
    {
        GameManager1.instance.playerFoodPoints = food;
    }

    private void CheckIfGameOver()
    {
        if (food <= 0)
            GameManager1.instance.GameOver();
    }
    protected override void OnCantMove<T>(T component)
    {

        Wall1 hitWall = component as Wall1;
        hitWall.DamageWall(wallDamage);
        animator.SetTrigger("playerChop");

    }

    private void Restart()
    {
        // Application.LoadLevel(Application.loadedLevel);
        SceneManager.LoadScene("Main");
    }

    public void LoseFood(int loss)
    {
        animator.SetTrigger("playerHit");
        food -= loss;
        foodText.text = "-" + loss + " Food : " + food;

        CheckIfGameOver();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Check if the tag of the trigger collided with is Exit.
        if (other.tag == "Exit")
        {
            //Invoke the Restart function to start the next level with a delay of restartLevelDelay (default 1 second).
            Invoke("Restart", restartLevelDelay);

            //Disable the player object since level is over.
            enabled = false;
        }

        //Check if the tag of the trigger collided with is Food.
        else if (other.tag == "Food")
        {
            //Add pointsPerFood to the players current food total.
            food += pointsPerFood;

            foodText.text = "+" + pointsPerFood + " Food : " + food;


            //Disable the food object the player collided with.
            other.gameObject.SetActive(false);
        }

        //Check if the tag of the trigger collided with is Soda.
        else if (other.tag == "Soda")
        {
            //Add pointsPerSoda to players food points total
            food += pointsPerSoda;

            foodText.text = "+" + pointsPerSoda + " Food : " + food;



            //Disable the soda object the player collided with.
            other.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager1.instance.playersTurn) return;

        int horizontal = 0;
        int vertical = 0;

        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");

        if(horizontal != 0)
        {
            vertical = 0;
        }
        if(horizontal != 0 || vertical != 0)
        {
            AttemptMove<Wall1>(horizontal, vertical);
        }

    }
}
