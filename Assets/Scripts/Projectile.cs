using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //-----------------------------------------------------------------------------Public Variables (Value-Types)
    //Speed of the player's projectiles
    public float projectileSpeed;
    //Amount of time before destroying the player's projectiles
    public float projectileLifeTime;


    void Start()
    {
        //Destroy the projectile after a variable time period
        Destroy(gameObject, projectileLifeTime);
    }


    void Update()
    {
        //Translate the projectile forward at a variable speed
        transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);
    }
}
