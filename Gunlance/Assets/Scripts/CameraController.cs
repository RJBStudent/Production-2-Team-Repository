using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour {

    [SerializeField] Transform playerTransform;
    [SerializeField] float heightRestrictMax, heightRestrictMin;
    [SerializeField] float aimMin, aimMax;
    [SerializeField] float xSpeed, ySpeed;
    [SerializeField] float distance;
    [SerializeField] float lerpSpeed;
    [SerializeField] Transform cannonRotate;
    [SerializeField] Image targetUI;
    [SerializeField] float aimOffset;

    float xMovement = 0, yMovement = 0;

    Vector3 newPos, pos;

    Vector3 aimPosition;
    float cannonAngle = 0;

    // Use this for initialization
    void Start ()
    {
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        GetInput();
        UpdatePosition();
        AimControl();
	}

    void GetInput()
    {
        xMovement += Input.GetAxis("Mouse X") * xSpeed;
        

        cannonAngle -= Input.GetAxis("Mouse Y") * ySpeed;

        //Only move when the cannon angle is within these bounds
       // if (-cannonAngle >= heightRestrictMin && -cannonAngle <= heightRestrictMax)
        //{
            yMovement -= Input.GetAxis("Mouse Y") * ySpeed;
        //}

        yMovement = Mathf.Clamp(yMovement, heightRestrictMin, heightRestrictMax);

    }


    void UpdatePosition()
    {
        Vector3 dir = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(yMovement, xMovement, 0);

        newPos = playerTransform.position + rotation * dir;
        pos = Vector3.Lerp(pos, newPos, Time.deltaTime * lerpSpeed);

        

        //transform.position = playerTransform.position + rotation * dir;
        transform.position = pos;
        transform.LookAt(playerTransform.position);

    }

    void AimControl()
    {
        
        cannonAngle = Mathf.Clamp(cannonAngle, aimMin, aimMax);

        //cannonRotate.rotation = Quaternion.Euler(cannonAngle, playerTransform.eulerAngles.y, playerTransform.eulerAngles.z);
    }
}
