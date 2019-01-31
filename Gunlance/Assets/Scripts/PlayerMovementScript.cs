using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour {

    [SerializeField] Transform cameraTransform;
    [SerializeField] float playerSpeed;

    Rigidbody thisRB;

    float xDirection, zDirection;
    Vector3 camForward, camRight;
    

	// Use this for initialization
	void Start ()
    {
        thisRB = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
    {

        GetInput();
        MovePlayer();
	}

    void GetInput ()
    {
        xDirection = Input.GetAxis("Horizontal");
        zDirection = Input.GetAxis("Vertical");
    }

    void MovePlayer()
    {
        camForward = cameraTransform.forward;
        camRight = cameraTransform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();
        Vector3 jumpVelocity = new Vector3(0, thisRB.velocity.y, 0);
                thisRB.velocity = camForward * zDirection * playerSpeed + camRight * xDirection * playerSpeed + jumpVelocity;
    }
}
