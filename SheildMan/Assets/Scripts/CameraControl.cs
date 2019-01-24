using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    [SerializeField] Transform playerPosition;
    [SerializeField] float yOffset;
    [SerializeField] float distance;
    [SerializeField] float lookAngle;

	// Use this for initialization
	void Start ()
    {
        transform.rotation = Quaternion.Euler(lookAngle, 0, 0);
	}
	
	// Update is called once per frame
	void Update ()
    {
        UpdatePosition();
	}

    void UpdatePosition()
    {
        transform.position = new Vector3(0,
            playerPosition.position.y + yOffset, playerPosition.position.z + distance);

        
    }
}
