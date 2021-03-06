﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Gruner_CameraController : MonoBehaviour 
{
    //store OS info
    string os;

    [SerializeField] Transform playerTransform;
    [SerializeField] float heightRestrictMax, heightRestrictMin;
    [SerializeField] float xSpeed, ySpeed;
    [SerializeField] float lerpSpeed;
    [SerializeField] float rotateAround = 70f;
    [SerializeField] float rotateAroundVertical = 45f;
    [Header("Camera Clipping Values")]
    [SerializeField] float minimumClipDistance;
    [SerializeField] LayerMask cameraCollideMask;
    [SerializeField] float maxDistance = 3, minDistance = 1;
    [SerializeField] float distanceAway = 3f;

    float xMovement = 0, yMovement = 0;

    Vector3 newPos, pos;

    public bool CanShake;

    //Collision Values
    Vector3 camMask;
    RaycastHit wallHit;

    //Temporary Debug Values
    bool removeCameraControl = false;
    float xPos, zPos;

    // Use this for initialization
    void Start()
    {
        rotateAround = playerTransform.eulerAngles.y - 45f;
        wallHit = new RaycastHit();

        GetOS();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        GetInput();
        //Dont move the camera with the player if the control is removed
        if (!removeCameraControl)
        {
            UpdatePosition();
        }
        else
        {
            UpdateCamera();
        }
    }

    void GetInput()
    {
        //Which direction to move in
        xMovement = Input.GetAxis("Mouse X" + os) * xSpeed; //add os to string to allow win & mac control

        yMovement = -Input.GetAxis("Mouse Y" + os) * ySpeed;


        if (xMovement / xSpeed < 0.3f && xMovement / xSpeed > -0.3f)
        {
            xMovement = 0.0f;
        }


        if (yMovement / ySpeed < 0.3f && yMovement / ySpeed > -0.3f)
        {
            yMovement = 0.0f;
        }

        //Temporary Movement
        xPos = Input.GetAxis("Horizontal");
        zPos = Input.GetAxis("Vertical");


        //TEMPORARY FILM CONTROL
        if (Input.GetKey(KeyCode.C))
        {
            removeCameraControl = true;
        }

        //Clamp how high and low the camera can go
        if (!removeCameraControl)
        {
            rotateAroundVertical = Mathf.Clamp(rotateAroundVertical, heightRestrictMin, heightRestrictMax);
        }

        //If shot is fired
        if (CanShake)
        {
            GetComponent<DavidScreenShakeScript>().ScreenShake();
        }

    }

    void UpdatePosition()
    {
        Vector3 dir = new Vector3(0, 0, -distanceAway);
        //Quaternion rotation = Quaternion.Euler(yMovement, xMovement, 0);
        Quaternion rotation = Quaternion.Euler(rotateAroundVertical, rotateAround, 0);
        Vector3 rotateVector = rotation * Vector3.one;

        //Move the position to the players position plus the offset
        // newPos = playerTransform.position + Vector3.up - rotateVector;
        //camMask = playerTransform.position + Vector3.up - rotateVector;
        newPos = playerTransform.position + rotation * dir;
        camMask = (playerTransform.position + rotation * dir);

        CameraRayCollide(playerTransform.position);

        pos = Vector3.Lerp(pos, newPos, Time.deltaTime * lerpSpeed);


        transform.position = pos;

        transform.LookAt(playerTransform.position);

        if (rotateAround > 360)
        {
            rotateAround = 0f;
        }
        else if (rotateAround < 0f)
        {
            rotateAround = (rotateAround + 360f);
        }

        //if (rotateAroundVertical > 360)
        //{
        //    rotateAroundVertical = 0f;
        //}
        //else if (rotateAroundVertical < 0f)
        //{
        //    rotateAroundVertical = (rotateAroundVertical + 360f);
        //}

        rotateAround += xMovement * lerpSpeed * Time.deltaTime;
        rotateAroundVertical += yMovement * lerpSpeed * Time.deltaTime;
        //distanceAway = Mathf.Clamp(distanceAway += yMovement, minDistance, maxDistance);
        //distanceUp = Mathf.Clamp(distanceUp + yMovement, minDistance, maxDistance);

    }

    //Camera FILM TEMPORARY
    void UpdateCamera()
    {

        Quaternion rotation = Quaternion.Euler(yMovement, xMovement, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * lerpSpeed);

        transform.Translate(Vector3.right * xPos / lerpSpeed);
        transform.Translate(transform.up * zPos / lerpSpeed, Space.World);


    }

    void CameraRayCollide(Vector3 offset)
    {

        if (Physics.Linecast(offset, camMask, out wallHit, cameraCollideMask))
        {
            newPos = new Vector3(wallHit.point.x + wallHit.normal.x * 0.5f, newPos.y, wallHit.point.z + wallHit.normal.z * 0.5f);
        }
    }

    //check operating system and define string os to be added to axis name for win/mac support
    void GetOS()
    {
        os = SystemInfo.operatingSystem;

        if(os.Contains("Mac"))
        {
            os = "_Mac";
        }
        else
        {
            os = null;
        }
    }
}
