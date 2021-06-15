using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    //The rate at which the spawner should spawn game objects
    [HideInInspector]
    public float spawnDelay;
    //Checks whether a weapon upgrade is in the scene
    [HideInInspector]
    public bool weaponUpgradeInScene = false;

    //The rate at which the spawner should spawn game objects
    public List<float> spawnDelayList;
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

    //At the start of the game, the enemy spawner cannot spawn enemies
    void Start()
    {
        canSpawn = false;

        //Finds the Game Controller and updates its public variables
        gameController = GameObject.Find("Game Controller").GetComponent<GameController>();
    }

    void Update()
    {
        if (gameController.score < 5000)
            spawnDelay = spawnDelayList[0];
        else if (gameController.score >= 5000 && gameController.score < 10000)
            spawnDelay = spawnDelayList[1];
        else if (gameController.score >= 10000 && gameController.score < 15000)
            spawnDelay = spawnDelayList[2];
        else if (gameController.score >= 15000 && gameController.score < 20000)
            spawnDelay = spawnDelayList[3];
        else if (gameController.score >= 20000 && gameController.score < 25000)
            spawnDelay = spawnDelayList[4];
        else if (gameController.score >= 25000 && gameController.score < 30000)
            spawnDelay = spawnDelayList[5];
        else if (gameController.score >= 30000 && gameController.score < 35000)
            spawnDelay = spawnDelayList[6];
        else if (gameController.score >= 40000 && gameController.score < 45000)
            spawnDelay = spawnDelayList[7];
        else if (gameController.score >= 45000 && gameController.score < 50000)
            spawnDelay = spawnDelayList[8];
        else if (gameController.score >= 50000)
            spawnDelay = spawnDelayList[9];

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
        int spawnChance = Random.Range(0, 10);

        //If the player's score is 1500 or higher, there's a 20% chance of spawning enemy_type01_variety02
        if (spawnChance < 2 && gameController.score >= 1500)
            Instantiate(enemyToSpawn[1], spawnPos, spawnRot);
        //If the player's score is 5000 or higher, there's a 10% change of spawning enemy_type01_variety03
        else if (spawnChance == 3 && gameController.score >= 5000)
            Instantiate(enemyToSpawn[2], spawnPos, spawnRot);
        else if (spawnChance == 4 && gameController.score >= 10000)
            Instantiate(enemyToSpawn[1], spawnPos, spawnRot);
        else if (spawnChance == 5 && gameController.score >= 20000)
            Instantiate(enemyToSpawn[2], spawnPos, spawnRot);
        else if (spawnChance == 6 && gameController.score >= 30000)
            Instantiate(enemyToSpawn[1], spawnPos, spawnRot);
        else if (spawnChance == 7 && gameController.score >= 40000)
            Instantiate(enemyToSpawn[2], spawnPos, spawnRot);
        else if (spawnChance == 8 && gameController.score >= 50000)
            Instantiate(enemyToSpawn[2], spawnPos, spawnRot);
        //If the player's score is 1500 or higher, a weapon upgrade is not currently in the scene and the player's weapons haven't already been activated
        //There's a 10% chance of spawning a Wide weapon upgrade
        else if (spawnChance == 9 && weaponUpgradeInScene == false && gameController.score >= 1500)
        {
            int spawnChanceWU = Random.Range(0, 3);

            if (spawnChanceWU == 0 && gameController.wideActivated == false)
                Instantiate(weaponUpgradeToSpawn[0], spawnPos, spawnRot);
            else if (spawnChanceWU == 1 && gameController.rapidActivated == false)
                Instantiate(weaponUpgradeToSpawn[1], spawnPos, spawnRot);
            else if (spawnChanceWU == 2 && gameController.largeActivated == false)
                Instantiate(weaponUpgradeToSpawn[2], spawnPos, spawnRot);
            else
                Instantiate(enemyToSpawn[1], spawnPos, spawnRot);

            //When a weapon upgrade is spawned, set this boolean to true
            weaponUpgradeInScene = true;
        }
        //If none of the above conditions are met, spawn enemy_type01_variety01
        else
            Instantiate(enemyToSpawn[0], spawnPos, spawnRot);
    }
}
