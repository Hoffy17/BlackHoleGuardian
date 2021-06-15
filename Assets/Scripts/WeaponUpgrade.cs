using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUpgrade : MonoBehaviour
{
    //Sets a weapon upgrade's speed
    public float weaponUpgradeSpeed;

    //Calls the GameController.cs script
    private GameController gameController;
    //Calls the Spawner.cs script
    private Spawner spawner;

    void Start()
    {
        //Finds the Game Controller and updates its public variables
        gameController = GameObject.Find("Game Controller").GetComponent<GameController>();
        //Finds the Spawners group and updates its public variables
        spawner = GameObject.Find("Spawners").GetComponent<Spawner>();
    }

    //On every update, transform the weapon upgrade's position to move towards the origin, at the delegated speed
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, weaponUpgradeSpeed * Time.deltaTime);
    }

    //Checks if the weapon upgrade colliders with something
    void OnTriggerEnter(Collider other)
    {
        //If the a weapon upgrade collides with the player
        if (other.tag == "Player")
        {
            //If the weapon upgrade is a Wide type
            if (gameObject.tag == "WeaponUpgrade_Wide")
            {
                //Set Guardina's extra weapons to active
                gameController.wideActivated = true;
            }
            else if (gameObject.tag == "WeaponUpgrade_Rapid")
            {
                gameController.rapidActivated = true;
            }
            else if (gameObject.tag == "WeaponUpgrade_Large")
            {
                gameController.largeActivated = true;
            }
            //Destroy the weapon upgrade
            Destroy(gameObject);
            //When a weapon upgrade is destroyed, set this boolean to false
            spawner.weaponUpgradeInScene = false;
        }

        //If a weapon upgrade collides with the Black Hole, destroy the weapon upgrade
        if (other.tag == "BlackHole")
        {
            Destroy(gameObject);
            //When a weapon upgrade is destroyed, set this boolean to false
            spawner.weaponUpgradeInScene = false;
        }
    }
}
