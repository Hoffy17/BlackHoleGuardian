using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //Sets an enemy's speed
    public float enemySpeed;

    //Calls the GameController.cs script
    private GameController gameController;
    //Calls the UIController.cs script
    private UIController uiController;

    void Start()
    {
        //Finds the Game Controller and updates its public variables
        gameController = GameObject.Find("Game Controller").GetComponent<GameController>();
        //Finds the UI Controller and updates its public variables
        uiController = GameObject.Find("UI Controller").GetComponent<UIController>();
    }

    //On every update, transform the enemy's position to move towards the origin, at a variable speed
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, enemySpeed * Time.deltaTime);
        //Rotate the enemy to face the origin
        transform.LookAt(Vector3.zero);
    }

    //Checks if the enemy collides with something
    void OnTriggerEnter(Collider other)
    {
        //If an enemy collides with the Black Hole, destroy the enemy and decrease health
        if (other.tag == "BlackHole")
        {
            Destroy(gameObject);
            gameController.health -= 1;
            uiController.healthBar.value -= 1;

            //If health falls to zero, the player is dead
            if (gameController.health <= 0)
            {
                gameController.isDead = true;
                gameController.blackHoleCollapsed = true;
            }
        }

        //If an enemy collides with a player projectile, destroy the enemy and increase the score
        if (other.tag == "PlayerProjectile")
        {
            Destroy(gameObject);
            gameController.score += 100;
        }
    }
}
