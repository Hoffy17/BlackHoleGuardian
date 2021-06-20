using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    //The speed of the player's projectiles
    public float projectileSpeed;
    //The amount of time before destroying the player's projectiles
    public float projectileLifeTime;

    //When this script is run, destroy the gameObject it is assigned to after a variable time period
    void Start()
    {
        Destroy(gameObject, projectileLifeTime);
    }

    //On every update, translate the projectile forward at a variable speed
    void Update()
    {
        transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);
    }
}
