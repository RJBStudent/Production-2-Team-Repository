using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour {

    [SerializeField] Transform cameraTransform;
    [SerializeField] float playerSpeed;
    [SerializeField] float lerpSpeed;
    [SerializeField] float FOVSpeed;
    [SerializeField] float minFOV, maxFOV;
    [SerializeField] Transform backExplosionPosition, frontExplosionPosition;
    [SerializeField] float force;
    [SerializeField] float explodeRadius;    
    [SerializeField] float yRotationSpeed;
    [SerializeField] float explodeTime;
    

    Camera thisCamera;
    Rigidbody thisRB;
    Vector3 lastVelocity;

    float explodeForwardInput = 0, explodeBackInput = 0;
    float xDirection, zDirection;
    Vector3 camForward, camRight;
    bool addedForce;
    Vector3 originalExplodePosition;

	// Use this for initialization
	void Start ()
    {
        thisRB = GetComponent<Rigidbody>();
        thisCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        GetInput();
        MovePlayer();
        ExplodeDirection();
	}

    void GetInput ()
    {
        xDirection = Input.GetAxis("Horizontal");
        zDirection = Input.GetAxis("Vertical");

        explodeForwardInput = Input.GetAxisRaw("FrontExplosion");
        explodeBackInput = Input.GetAxisRaw("BackExplosion");
    }

    void MovePlayer()
    {
        camForward = cameraTransform.forward;
        camRight = cameraTransform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();
        //Vector3 jumpVelocity = new Vector3(0, thisRB.velocity.y, 0);
        thisRB.velocity = camForward * zDirection * playerSpeed + camRight * xDirection * playerSpeed;

        Vector3 targetDirection;
        if (thisRB.velocity.magnitude != 0)
        {

            targetDirection = new Vector3(transform.position.x + thisRB.velocity.normalized.x, transform.position.y, transform.position.z + thisRB.velocity.normalized.z);
            
        }
        else
        {
            targetDirection = lastVelocity;
        }
       

        targetDirection = Vector3.Lerp(lastVelocity, targetDirection, Time.deltaTime * yRotationSpeed);

        transform.LookAt(targetDirection);
        
        lastVelocity = new Vector3(targetDirection.x, transform.position.y, targetDirection.z);

        thisCamera.fieldOfView = (thisRB.velocity.magnitude > 5) ?Mathf.Lerp(thisCamera.fieldOfView, thisCamera.fieldOfView + FOVSpeed, Time.deltaTime) : Mathf.Lerp(thisCamera.fieldOfView, thisCamera.fieldOfView - FOVSpeed, Time.deltaTime);
        thisCamera.fieldOfView = Mathf.Clamp(thisCamera.fieldOfView, minFOV, maxFOV);
    }

    void ExplodeDirection()
    {
        if(explodeForwardInput == 1.0 && !addedForce)
        {
            thisRB.AddExplosionForce(force, frontExplosionPosition.position, explodeRadius, 1.0f, ForceMode.Impulse);
            originalExplodePosition = frontExplosionPosition.position;
            StartCoroutine( ExplodeTime(explodeTime));
        }
        else if(explodeBackInput == 1.0 && !addedForce)
        {
            thisRB.AddExplosionForce(force, backExplosionPosition.position, explodeRadius, 1.0f, ForceMode.Impulse);
            originalExplodePosition = backExplosionPosition.position;
            StartCoroutine(ExplodeTime(explodeTime));
        }

        if(addedForce)
        {
            thisRB.AddExplosionForce(force, originalExplodePosition, explodeRadius, 1.0f, ForceMode.Impulse);
            lastVelocity = new Vector3(thisRB.velocity.x, 0, thisRB.velocity.z); 
        }
    }

    IEnumerator ExplodeTime(float time)
    {
        addedForce = true;
        yield return new WaitForSecondsRealtime(time);
        addedForce = false;
    }
}
