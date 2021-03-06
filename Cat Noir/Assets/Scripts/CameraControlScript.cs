﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControlScript : MonoBehaviour {

    

    [SerializeField] private float heightRestrictMax, heightRestrictMin;
    [SerializeField] private float xSpeed, ySpeed;
    [SerializeField] private Transform playerPosition;
    [SerializeField] private float distance;
    [SerializeField] private Vector3 rayOffset;
    [SerializeField] private float rayDistance;
    [SerializeField] private float rayCameraHeight;

    float xMovement = 0, yMovement = 0;
   
    
    

    RaycastHit hitWall;
    Vector3 backOfCamera;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void FixedUpdate ()
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

        //backOfCamera = playerPosition.TransformDirection(-1 * Vector3.forward);

        //if (Physics.Raycast(playerPosition.TransformPoint(rayOffset), backOfCamera, out hitWall, rayDistance)
        //    && hitWall.transform != playerPosition)
        //{
        //    dir.x = hitWall.point.x;
        //    dir.z = hitWall.point.z;
        //    dir.y = Mathf.Lerp(hitWall.point.y, dir.y, Time.deltaTime * 5.0f);
        //}

        //transform.position = Vector3.Lerp(transform.position, playerPosition.position + rotation * dir, Time.deltaTime * 5.0f);
        
        transform.position = playerPosition.position + rotation * dir;
        transform.LookAt(playerPosition.position);

       // transform.LookAt(playerPosition);
    }
}
