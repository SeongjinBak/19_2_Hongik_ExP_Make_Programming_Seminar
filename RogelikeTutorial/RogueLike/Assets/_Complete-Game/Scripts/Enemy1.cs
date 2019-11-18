﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MovingObject1
{
    public int playerDamage;

    private Animator animator;
    private Transform target;
    private bool skipMove;

    public AudioClip enemyAttack1;
    public AudioClip enemyAttack2;

    // Start is called before the first frame update
    protected override void Start()
    {
        GameManager1.instance.AddEnemyToList(this);
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
    }

   

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        if (skipMove)
        {
            skipMove = false;
            return;
        }

        base.AttemptMove<T>(xDir, yDir);
        skipMove = true;
    }
    

    public void MoveEnemy()
    {
        //Declare variables for X and Y axis move directions, these range from -1 to 1.
            //These values allow us to choose between the cardinal directions: up, down, left and right.
            int xDir = 0;
        int yDir = 0;

        //If the difference in positions is approximately zero (Epsilon) do the following:
        if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)

            //If the y coordinate of the target's (player) position is greater than the y coordinate of this enemy's position set y direction 1 (to move up). If not, set it to -1 (to move down).
            yDir = target.position.y > transform.position.y ? 1 : -1;

        //If the difference in positions is not approximately zero (Epsilon) do the following:
        else
            //Check if target x position is greater than enemy's x position, if so set x direction to 1 (move right), if not set to -1 (move left).
            xDir = target.position.x > transform.position.x ? 1 : -1;

        //Call the AttemptMove function and pass in the generic parameter Player, because Enemy is moving and expecting to potentially encounter a Player
        AttemptMove<Player1>(xDir, yDir);
    }

    protected override void OnCantMove<T>(T component)
    {
        //Declare hitPlayer and set it to equal the encountered component.
        Player1 hitPlayer = component as Player1;

        animator.SetTrigger("enemyAttack");

  
        //Call the LoseFood function of hitPlayer passing it playerDamage, the amount of foodpoints to be subtracted.
        hitPlayer.LoseFood(playerDamage);

        SoundManager1.instance.RandomizeSfx(enemyAttack1, enemyAttack2);

    }
}
