using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    [SerializeField] Transform playerPosition;
    [SerializeField] Transform bossPosition;
    [SerializeField] float yOffset;
    [SerializeField] float zLookOffset;
    [SerializeField] float distance;
    [SerializeField] float lookAngle;
    [SerializeField] float lerpSpeed;

    Vector3 pos, newPos;

	// Use this for initialization
	void Start ()
    {
        transform.rotation = Quaternion.Euler(lookAngle, 0, 0);

        pos = new Vector3(0, playerPosition.position.y + yOffset,
            playerPosition.position.z + distance);
        transform.position = pos;
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        UpdatePosition();
        UpdateRotation();
	}

    void UpdatePosition()
    {
         newPos = new Vector3(playerPosition.position.x,  + yOffset,
             playerPosition.position.z + distance);
        pos = Vector3.Lerp(pos, newPos, Time.deltaTime * lerpSpeed);
        transform.position = pos;
        
    }

    void UpdateRotation()
    {
        // Vector3 lookPosition =  new Vector3( ( bossPosition.position.x + playerPosition.position.x ) / 2, 0, ( bossPosition.position.z + playerPosition.position.z ) / 2);
        Vector3 lookPosition = new Vector3(playerPosition.position.x, bossPosition.position.y, playerPosition.position.z + zLookOffset);
        
       transform.LookAt(lookPosition);
    }
}
