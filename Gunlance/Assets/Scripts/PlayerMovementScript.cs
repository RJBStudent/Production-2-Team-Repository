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
    [SerializeField] float upwardsForce;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    Collider[] hitCollide;

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
        GroundCheck();
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
        Vector3 jumpVelocity = new Vector3(0, thisRB.velocity.y, 0);
        if (!addedForce)
        {
            thisRB.velocity = camForward * zDirection * playerSpeed + camRight * xDirection * playerSpeed + jumpVelocity;
        }
       

        Vector3 targetDirection;
        //if (thisRB.velocity.magnitude != 0  || hitCollide.Length > 0)
        //{

        // targetDirection = new Vector3(transform.position.x, transform.position.y, transform.position.z + camRight.z * 5);
        targetDirection = transform.position + (camForward * 5);
        //}
        //else
        //{
        //    targetDirection = lastVelocity;

        //}


       // targetDirection = Vector3.Lerp(lastVelocity, targetDirection, Time.deltaTime * yRotationSpeed);
        transform.LookAt(targetDirection);
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        
        lastVelocity = new Vector3(targetDirection.x, transform.position.y, targetDirection.z);


        thisCamera.fieldOfView = (thisRB.velocity.magnitude > 10) ?Mathf.Lerp(thisCamera.fieldOfView, thisCamera.fieldOfView + FOVSpeed, Time.deltaTime) : Mathf.Lerp(thisCamera.fieldOfView, thisCamera.fieldOfView - FOVSpeed, Time.deltaTime);
        thisCamera.fieldOfView = Mathf.Clamp(thisCamera.fieldOfView, minFOV, maxFOV);
    }

    void GroundCheck()
    {
        hitCollide = null;
        hitCollide = Physics.OverlapSphere(groundCheck.position, 0.21f, groundLayer);
    }

    void ExplodeDirection()
    {
        if(explodeForwardInput == 1.0 && !addedForce)
        {
            thisRB.AddExplosionForce(force, frontExplosionPosition.position, explodeRadius, upwardsForce, ForceMode.Impulse);
            originalExplodePosition = frontExplosionPosition.position;

            lastVelocity = new Vector3(transform.position.x + thisRB.velocity.x, transform.position.y, transform.position.z + thisRB.velocity.z);
            StartCoroutine( ExplodeTime(explodeTime));
        }
        else if(explodeBackInput == 1.0 && !addedForce)
        {
            thisRB.AddExplosionForce(force, backExplosionPosition.position, explodeRadius, upwardsForce, ForceMode.Impulse);
            originalExplodePosition = backExplosionPosition.position;

            lastVelocity = new Vector3(transform.position.x + thisRB.velocity.x, transform.position.y, transform.position.z + thisRB.velocity.z);
            StartCoroutine(ExplodeTime(explodeTime));
        }

        if(addedForce)
        {
           // thisRB.AddExplosionForce(force, originalExplodePosition, explodeRadius, upwardsForce, ForceMode.Impulse);
           // lastVelocity = new Vector3(transform.position.x + thisRB.velocity.x, transform.position.y, transform.position.z + thisRB.velocity.z); 
        }
    }

    IEnumerator ExplodeTime(float time)
    {
        addedForce = true;
        yield return new WaitForSecondsRealtime(time);
        addedForce = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (addedForce)
        {
            Gizmos.DrawSphere(originalExplodePosition, explodeRadius);
    }
    }
}
