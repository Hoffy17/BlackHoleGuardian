using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //-----------------------------------------------------------------------------Public Variables (Value-Types)
    //Sets an enemy's speed
    public float enemySpeed;

    //-----------------------------------------------------------------------------Public Variables (Reference-Types)
    //Particle system when an enemy dies
    public ParticleSystem enemyDiePS;

    //-----------------------------------------------------------------------------Private Variables (Reference-Types)
    //Calls the following scripts
    private GameController gameController;
    private UIController uiController;
    private MovementController movementController;
    //Sound effects
    private AudioSource sfxEnemyDie;
    private AudioSource sfxBlackholeDamage;


    void Start()
    {
        //Find these scripts and updates their public variables
        gameController = GameObject.Find("Game Controller").GetComponent<GameController>();
        uiController = GameObject.Find("UI Controller").GetComponent<UIController>();
        movementController = GameObject.Find("Guardian Controller").GetComponent<MovementController>();
        //Find these audio sources
        sfxEnemyDie = GameObject.Find("sfx_enemy_die").GetComponent<AudioSource>();
        sfxBlackholeDamage = GameObject.Find("sfx_blackhole_damage").GetComponent<AudioSource>();
    }


    void Update()
    {
        //Transform the enemy's position to move towards the origin, at a variable speed
        transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, enemySpeed * Time.deltaTime);
        //Rotate the enemy to face the origin
        transform.LookAt(Vector3.zero);
    }


    //Checks if the enemy collides with something
    void OnTriggerEnter(Collider other)
    {
        //Checks if an enemy collides with the Black Hole
        if (other.tag == "BlackHole")
            TakeDamage();

        //Checks if an enemy collides with a player projectile
        if (other.tag == "PlayerProjectile")
            KillEnemy();
    }


    private void TakeDamage()
    {
        //Destroy the enemy
        Destroy(gameObject);

        //Decrease Black Hole's health
        gameController.health -= 1;
        uiController.healthBar.value -= 1;

        //Play sound effect
        sfxBlackholeDamage.Play();

        //If health falls to zero, the player is dead and the Black Hole starts to collapse
        if (gameController.health <= 0)
        {
            gameController.isDead = true;
            movementController.blackHoleCollapsed = true;

            movementController.overheatAlarm.SetActive(false);
        }
    }


    private void KillEnemy()
    {
        //Destroy the enemy and instantiate particles
        Destroy(gameObject);
        Instantiate(enemyDiePS, transform.position, transform.rotation);

        //Increase the score
        gameController.score += 100;

        //Play sound effect
        sfxEnemyDie.Play();
    }
}
