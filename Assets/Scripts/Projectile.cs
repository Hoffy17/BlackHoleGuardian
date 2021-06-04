using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    //A public variable for the speed of the player's projectiles
    public float projectileSpeed;
    //A public variable for destroying the player's projectiles after a certain period of time has passed
    public float projectileLifeTime;

    //When this script is run, destroy the gameObject it is assigned to after the delegated time period
    void Start()
    {
        Destroy(gameObject, projectileLifeTime);
    }

    //On every update, translate the projectile forward at the delegated speed
    void Update()
    {
        transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);
    }
}
