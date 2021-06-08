using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{

    //The axis at which the Player rotates around the Black Hole
    public Vector3 rotationAxis = Vector3.up;
    //The speed at which that rotation occurs
    public float rotationSpeed;

    private GameController gameController;

    void Start()
    {
        gameController = GameObject.Find("Game Controller").GetComponent<GameController>();
    }

    void Update()
    {
        //If the player is alive, they can use the D and A keys to rotate around the Black Hole
        if (!gameController.isDead)
        {
            if (Input.GetKey(KeyCode.D))
            {
                transform.Rotate(rotationAxis * Time.deltaTime * rotationSpeed);
            }

            if (Input.GetKey(KeyCode.A))
            {
                transform.Rotate(-rotationAxis * Time.deltaTime * rotationSpeed);
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                transform.Rotate(0, 180, 0);
            }
        }
    }
}
