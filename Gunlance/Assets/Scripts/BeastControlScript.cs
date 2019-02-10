using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeastControlScript : MonoBehaviour {

    [SerializeField] BeastObject beastValues;
    

    int targetNode;
    Vector3 targetPosition;
    Vector3 targetDirection;

	// Use this for initialization
	void Start ()
    {
        targetNode = 0;
        targetDirection = beastValues.movementNodes[targetNode];
	}
	
	// Update is called once per frame
	void Update ()
    {
        MoveBeast();
	}

    void MoveBeast()
    {

        targetDirection = targetPosition - transform.position;
        if(targetDirection.magnitude < beastValues.nodeRadius)
        {
            targetNode = (targetNode + 1) % beastValues.movementNodes.Length;
            targetPosition = beastValues.movementNodes[targetNode];
        }
        else
        {
            targetDirection /= beastValues.speed;
            gameObject.transform.position = new Vector3(transform.position.x + targetDirection.x, transform.position.y + targetDirection.y, transform.position.z+ targetDirection.z);

        }

    }
}
