using System.Collections;
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

    bool inEvent = false;
    float eventTimer = 0f;

	float groanTimer;
	float targetGroanTime;
	public float minGroanTime;
	public float maxGroanTime;

	// Use this for initialization
	void Start ()
    {
        targetNode = 0;
        targetRotateNode = 0;
        //Set the start node and directions
        targetPosition = beastValues.movementNodes[targetNode];
        targetLookDirection = beastValues.movementNodes[targetRotateNode];

		targetGroanTime = minGroanTime;
	}

	void Update()
	{
		groanTimer += Time.deltaTime;
		if (groanTimer > targetGroanTime)
		{
			Mann_AudioManagerScript.instance.PlaySound("Etherean_Passive_Groan");
			targetGroanTime = Random.Range(minGroanTime, maxGroanTime);
			groanTimer = 0;
		}
	}

	// Update is called once per frame
	void FixedUpdate ()
    {
        if (!inEvent)
        {
            MoveBeast();
        }
        else
        {
            EtherianEventFunction();
        }
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
            CheckEventNode();
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

    void EtherianEventFunction()
    {

        if (eventTimer >= beastValues.etheriansEvents[targetNode-1].lengthOfEvent)
        { inEvent = false; eventTimer = 0; return; }

        eventTimer += Time.deltaTime;

    }

    //Check whether the current target node and compare it with target node 
    void CheckEventNode()
    {
        for (int i = 0; i < beastValues.etheriansEvents.Length; i++)
        {
            if(beastValues.etheriansEvents[i].nodeLocation == targetNode)
            {
                inEvent = true;
                return;
            }
        }
    }

	
	
}
