using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlScript : MonoBehaviour {

    [SerializeField]
    private float playerSpeed;
    float playerX;
    float playerZ;
    Transform thisTransform;
    Rigidbody thisRigidbody;

    
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
        thisRigidbody.velocity = new Vector3(playerX * playerSpeed, 0, playerZ* playerSpeed);
    }
}
