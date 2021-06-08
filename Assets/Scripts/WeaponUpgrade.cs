using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUpgrade : MonoBehaviour
{
    public float weaponUpgradeSpeed;

    private GameController gameController;
    private Weapon weapon;
    private Spawner spawner;

    void Start()
    {
        gameController = GameObject.Find("Game Controller").GetComponent<GameController>();
        weapon = GameObject.Find("Weapon_M").GetComponent<Weapon>();
        spawner = GameObject.Find("Spawners").GetComponent<Spawner>();
    }

    //On every update, transform the enemy's position to move towards the origin, at the delegated speed
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, weaponUpgradeSpeed * Time.deltaTime);
        //Set the enemy to face the origin
        transform.LookAt(Vector3.zero);
    }

    //Checks if the enemy colliders with something
    void OnTriggerEnter(Collider other)
    {
        //If an enemy collides with the BlackHole, destroy the enemy and decrease health
        if (other.tag == "Player")
        {
            if (gameObject.tag == "WeaponUpgrade_Wide")
            {
                weapon.allWeaponsActivated = true;
            }
            Destroy(gameObject);
            spawner.weaponUpgradeInScene = false;
        }

        if (other.tag == "BlackHole")
        {
            Destroy(gameObject);
            spawner.weaponUpgradeInScene = false;
        }
    }
}
