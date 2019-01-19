using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlScript : MonoBehaviour {

    [SerializeField]
    private float playerSpeed;
    [SerializeField]
    private Transform cameraTransform;
    float playerX;
    float playerZ;
    Transform thisTransform;
    Rigidbody thisRigidbody;
    Vector3 camForward;
    Vector3 camRight;

    
	// Use this for initialization
	void Start () {
        thisTransform = gameObject.transform;
        thisRigidbody = gameObject.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {

        UpdateInput();
        UpdatePosition();

	}

    void UpdateInput()
    {
        playerX = Input.GetAxis("Horizontal");
        playerZ = Input.GetAxis("Vertical");

    }
    
    void UpdatePosition()
    {
        camForward = cameraTransform.forward;
        camRight = cameraTransform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();

        //thisRigidbody.velocity = new Vector3(playerX * playerSpeed, 0, playerZ* playerSpeed);
        thisRigidbody.velocity = camForward*playerZ* playerSpeed+ camRight*playerX* playerSpeed;

    }
}
