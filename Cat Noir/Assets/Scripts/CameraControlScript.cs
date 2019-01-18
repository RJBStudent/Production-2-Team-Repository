using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControlScript : MonoBehaviour {

    

    [SerializeField]
    private float heightRestrictMax, heightRestrictMin;
    [SerializeField]
    private float xSpeed, ySpeed;
    [SerializeField]
    private Transform playerPosition;
    [SerializeField]
    private float distance;

    float xMovement = 0, yMovement = 0;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        GetInput();
        UpdatePosition();
	}

    void GetInput()
    {
        xMovement += Input.GetAxis("Mouse X") * xSpeed;
        yMovement += Input.GetAxis("Mouse Y") * ySpeed;

        yMovement = Mathf.Clamp(yMovement, heightRestrictMin, heightRestrictMax);
    }

    void UpdatePosition()
    {
        Vector3 dir = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(yMovement, xMovement, 0);
        transform.position = playerPosition.position + rotation * dir;
        transform.LookAt(playerPosition.position);

        transform.LookAt(playerPosition);
    }
}
