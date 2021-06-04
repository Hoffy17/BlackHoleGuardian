using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    //The axis at which the Player rotates around the Black Hole
    public Vector3 rotationAxis = Vector3.up;
    //The speed at which that rotation occurs
    public float rotationSpeed;

    //A boolean for checking whether the game is over
    [HideInInspector]
    public bool isDead;

    //At the start of the game, the player is alive
    void Start()
    {
        isDead = false;
    }

    void Update()
    {
        //If the player is alive, they can use the D and A keys to rotate around the Black Hole
        if (!isDead)
        {
            if (Input.GetKey(KeyCode.D))
            {
                transform.Rotate(rotationAxis * Time.deltaTime * rotationSpeed);
            }

            if (Input.GetKey(KeyCode.A))
            {
                transform.Rotate(-rotationAxis * Time.deltaTime * rotationSpeed);
            }
        }
    }
}
