using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    [SerializeField] Transform playerPosition;
    [SerializeField] float yOffset;
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
	void Update ()
    {
        UpdatePosition();
	}

    void UpdatePosition()
    {
         newPos = new Vector3(0, playerPosition.position.y + yOffset,
             playerPosition.position.z + distance);
        pos = Vector3.Lerp(pos, newPos, Time.deltaTime * lerpSpeed);
        transform.position = pos;
    }
}
