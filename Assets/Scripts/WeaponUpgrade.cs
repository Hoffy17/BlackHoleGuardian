using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUpgrade : MonoBehaviour
{
    //-----------------------------------------------------------------------------Public Variables (Value-Types)
    //Sets a weapon upgrade's speed
    public float weaponUpgradeSpeed;

    //-----------------------------------------------------------------------------Private Variables (Reference-Types)
    //Calls the following scripts
    private GameController gameController;
    private MovementController movementController;
    private Spawner spawner;
    //Sound effect played when the player collects a weapon upgrade
    private AudioSource getUpgrade;


    void Start()
    {
        //Find these scripts and updates their public variables
        gameController = GameObject.Find("Game Controller").GetComponent<GameController>();
        movementController = GameObject.Find("Guardian Controller").GetComponent<MovementController>();
        spawner = GameObject.Find("Spawners").GetComponent<Spawner>();
        //Find this audio source
        getUpgrade = GameObject.Find("GetUpgrade").GetComponent<AudioSource>();
    }


    //On every update, transform the weapon upgrade's position to move towards the origin, at a variable speed
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, weaponUpgradeSpeed * Time.deltaTime);
    }


    //Checks if the weapon upgrade collides with something
    void OnTriggerEnter(Collider other)
    {
        //Checks if the weapon upgrade collides with the player
        if (other.tag == "Player")
            GetUpgrade();

        //Checks if a weapon upgrade collides with the Black Hole
        if (other.tag == "BlackHole")
            DestroyUpgrade();
    }


    private void GetUpgrade()
    {
        //If the weapon upgrade is a Wide type
        if (gameObject.tag == "WeaponUpgrade_Wide")
        {
            //Set this boolean to true and set off these particles
            gameController.wideActivated = true;
            movementController.playUpgradePS = true;
        }
        //Or if the weapon upgrade is a Rapid type
        else if (gameObject.tag == "WeaponUpgrade_Rapid")
        {
            //Set this boolean to true and set off these particles
            gameController.rapidActivated = true;
            movementController.playUpgradePS = true;
        }
        //Or if the weapon upgrade is a Large type
        else if (gameObject.tag == "WeaponUpgrade_Large")
        {
            //Set this boolean to true and set off these particles
            gameController.largeActivated = true;
            movementController.playUpgradePS = true;
        }

        //Play sound effect
        getUpgrade.Play();

        DestroyUpgrade();
    }


    private void DestroyUpgrade()
    {
        //When a weapon upgrade is destroyed, set this boolean to false
        spawner.weaponUpgradeInScene = false;

        //Destroy the weapon upgrade
        Destroy(gameObject);
    }
}
