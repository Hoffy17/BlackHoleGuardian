using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    //A public variable for setting any enemy's speed
    public float enemySpeed;

    //On every updtae, translate the enemy forward at the delegated speed
    void Update()
    {
        transform.Translate(Vector3.forward * enemySpeed * Time.deltaTime);
    }

    //Checks if the enemy colliders with something
    private void OnTriggerEnter(Collider other)
    {
        //If an enemy collides with the BlackHole, destroy the enemy
        if (other.tag == "BlackHole")
        {
            Destroy(gameObject);
        }

        //If an enemy collides with a player projectile", destroy the enemy
        if (other.tag == "PlayerProjectile")
        {
            Destroy(gameObject);
        }
    }
}
