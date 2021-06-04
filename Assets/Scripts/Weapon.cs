using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    //Creates a variable GameObject for player's projectiles
    public GameObject projectileObject;

    //The rate at which the player can fire projectiles
    public float refireRate;
    //The amount of time that is required before the player can again fire projectiles
    private float timePassed;
    //A boolean to check whether the player is able to fire projectiles
    private bool canShoot;

    //At the start of the game, the player can shoot projectiles
    void Start()
    {
        canShoot = true;
        timePassed = 0.0f;
    }

    void Update()
    {
        //If the player presses the Space and they are able to shoot projectiles
        if ((Input.GetKey(KeyCode.Space)) && (canShoot == true))
        {
            //Run the Shoot function and disable the player's ability to shoot
            Shoot();
            canShoot = false;
            timePassed = 0.0f;
        }

        //Count time until the player can shoot again
        timePassed += Time.deltaTime;

        //If the amount of time passed is greater than or equal to the rate at which the player is able to shoot
        if (timePassed >= refireRate)
        {
            //Allow the player to shoot
            canShoot = true;
        }
    }

    //A function that handles the player's ability to fire projectiles
    private void Shoot()
    {
        //If the player is not dead
        if (!transform.root.GetComponent<Player>().isDead)
            //Instantiate projectiles at the player's location, in the direction the player is facing
            Instantiate(projectileObject, transform.position, transform.rotation);
    }
}
