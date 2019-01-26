using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    [SerializeField] float playerSpeed;
    [SerializeField] float acceleration;

    CharacterController thisPlayerControl;

    float xDirection, zDirection;
    Vector3 velocity;

    // Use this for initialization
    void Start()
    {

        thisPlayerControl = GetComponent<CharacterController>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        GetInput();
        MovePlayer();

    }

    void GetInput()
    {
        xDirection = Input.GetAxisRaw("Horizontal");
        zDirection = Input.GetAxisRaw("Vertical");
    }

    void MovePlayer()
    {
        Vector3 dir = new Vector3(xDirection, 0, zDirection);

        velocity = Vector3.Lerp(velocity, dir*playerSpeed, acceleration*Time.deltaTime);

        thisPlayerControl.Move(velocity* Time.deltaTime);

    }
}
