using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    //The rate at which the spawner should spawn game objects
    public float spawnDelay;
    //Checks whether a weapon upgrade is in the scene
    [HideInInspector]
    public bool weaponUpgradeInScene = false;

    //A list of spawner objects in the scene
    public List<GameObject> spawners;
    //A list of enemy prefabs to be spawned
    public List<GameObject> enemyToSpawn;
    //A list of weapon upgrade prefabs to be spawned
    public List<GameObject> weaponUpgradeToSpawn;

    //Measures time since the last enemy was spawned
    private float timePassed;
    //Checks whether a spawner can spawn
    private bool canSpawn;
    //Calls the Box Collider component for spawners in the scene
    private BoxCollider spawnBounds;
    //Calls the GameController.cs script
    private GameController gameController;
    //Calls the Weapon.cs script
    private Weapon weapon;

    //At the start of the game, the enemy spawner cannot spawn enemies
    void Start()
    {
        canSpawn = false;

        //Finds the Game Controller and updates its public variables
        gameController = GameObject.Find("Game Controller").GetComponent<GameController>();
        //Finds the main Weapon game object and updates its public variables
        weapon = GameObject.Find("Weapon_M").GetComponent<Weapon>();
    }

    void Update()
    {
        //Runs through each spawner in the scene and spawns enemies and weapon upgrades
        for (int i = 0; i < spawners.Count; i++)
        {
            //Calls the box collider component for a spawner and gets its size
            spawnBounds = spawners[i].GetComponent<BoxCollider>();

            //If the spawner can spawn
            if (canSpawn)
            {
                //Run the Spawn function and disable the spawner's ability to spawn
                Spawn();
                canSpawn = false;
                timePassed = 0.0f;
            }

            //Count time until the spawner can spawn again
            timePassed += Time.deltaTime;

            //If the amount of time passed is greater than or equal to the rate at which the spawner is able to spawn
            if (timePassed >= spawnDelay)
            {
                //Allow the spawner to spawn again
                canSpawn = true;
            }
        }
    }

    //A function that handles the enemy spawner's ability to spawn
    private void Spawn()
    {
        //Defines the random range within which game objects can spawn, as the bounds of the box collider
        float xPos = Random.Range((spawnBounds.size.x * -0.5f), (spawnBounds.size.x * 0.5f)) + spawnBounds.gameObject.transform.position.x;
        float zPos = Random.Range((spawnBounds.size.z * -0.5f), (spawnBounds.size.z * 0.5f)) + spawnBounds.gameObject.transform.position.z;
        //Create a variable for a game object's spawn position
        Vector3 spawnPos = new Vector3(xPos, 0.0f, zPos);

        //Define the game object's rotation variable as a Quaternion
        Quaternion spawnRot = new Quaternion();
        //Then use the Euler function to convert the game object's rotation into a Vector3
        spawnRot = Quaternion.Euler(0f, 0f, 0f);

        //Grab a random number with a range
        int spawnProbability = Random.Range(0, 10);

        //If the player's score is 1500 or higher, there's a 20% chance of spawning enemy_type01_variety02
        if (spawnProbability < 2 && gameController.score >= 1500)
            Instantiate(enemyToSpawn[1], spawnPos, spawnRot);
        //If the player's score is 5000 or higher, there's a 10% change of spawning enemy_type01_variety03
        else if (spawnProbability == 3 && gameController.score >= 5000)
            Instantiate(enemyToSpawn[2], spawnPos, spawnRot);
        //If the player's score is 1500 or higher, a weapon upgrade is not currently in the scene and the player's weapons haven't already been activated
        //There's a 10% chance of spawning a Wide weapon upgrade
        else if (spawnProbability == 9 && weapon.allWeaponsActivated == false && weaponUpgradeInScene == false && gameController.score >= 1500)
        {
            Instantiate(weaponUpgradeToSpawn[0], spawnPos, spawnRot);
            //When a weapon upgrade is spawned, set this boolean to true
            weaponUpgradeInScene = true;
        }
        //If none of the above conditions are met, spawn enemy_type01_variety01
        else
            Instantiate(enemyToSpawn[0], spawnPos, spawnRot);
    }
}
