﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    //A public variable for setting any enemy's speed
    public float enemySpeed;

    private GameController gameController;
    private UIController uiController;

    void Start()
    {
        gameController = GameObject.Find("Game Controller").GetComponent<GameController>();
        uiController = GameObject.Find("UI Controller").GetComponent<UIController>();
    }

    //On every update, transform the enemy's position to move towards the origin, at the delegated speed
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, enemySpeed * Time.deltaTime);
        //Set the enemy to face the origin
        transform.LookAt(Vector3.zero);
    }

    //Checks if the enemy colliders with something
    void OnTriggerEnter(Collider other)
    {
        //If an enemy collides with the BlackHole, destroy the enemy and decrease health
        if (other.tag == "BlackHole")
        {
            Destroy(gameObject);
            gameController.health -= 1;
            uiController.healthBar.value -= 1;

            if (gameController.health <= 0)
                gameController.isDead = true;
        }

        //If an enemy collides with a player projectile, destroy the enemy and increase the score
        if (other.tag == "PlayerProjectile")
        {
            Destroy(gameObject);
            gameController.score += 100;
        }
    }
}
