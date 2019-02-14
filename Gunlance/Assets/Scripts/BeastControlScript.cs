﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeastControlScript : MonoBehaviour {

    [SerializeField] BeastObject beastValues;
    

    int targetNode;
    int targetRotateNode;
    Vector3 targetPosition;
    Vector3 targetDirection;
    Vector3 targetLookDirection;
    Vector3 lookAheadDirection;

	// Use this for initialization
	void Start ()
    {
        targetNode = 0;
        targetRotateNode = 0;
        //Set the start node and directions
        targetPosition = beastValues.movementNodes[targetNode];
        targetLookDirection = beastValues.movementNodes[targetRotateNode];
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        MoveBeast();
	}

    void MoveBeast()
    {
        //What is it moving to and what is it looking at
        targetDirection = targetPosition - transform.position;
        targetLookDirection = lookAheadDirection - transform.position;

        if(targetLookDirection.magnitude < beastValues.nodeRotateRadius)
        {
            //Change rotation within range
            targetRotateNode = (targetRotateNode + 1) % beastValues.movementNodes.Length;
            lookAheadDirection = beastValues.movementNodes[targetRotateNode];
        }
        else
        {
            //Look at node
            Quaternion toDirection = Quaternion.LookRotation(lookAheadDirection);
        }

        if(targetDirection.magnitude < beastValues.nodeRadius)
        {
            //Change target position within range
            targetNode = (targetNode + 1) % beastValues.movementNodes.Length;
            targetPosition = beastValues.movementNodes[targetNode];
        }
        else
        {
            //Update position and current direction
            Vector3 newPosition = new Vector3(transform.position.x + targetDirection.x, transform.position.y + targetDirection.y, transform.position.z+ targetDirection.z);
            transform.position = Vector3.MoveTowards(transform.position, newPosition, beastValues.speed * Time.deltaTime);

            Quaternion toDirection = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, toDirection, Time.deltaTime * beastValues.rotationSpeed);
        }

    }
}
