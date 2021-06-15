using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public Vector3 wallRotationAxis = Vector3.up;

    private MovementController movementController;

    void Start()
    {
        movementController = GameObject.Find("Guardian Controller").GetComponent<MovementController>();
    }

    void Update()
    {
    }
}
