using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour {

    [SerializeField] Transform cameraTransform;
    [SerializeField] float playerSpeed;
    [SerializeField] float airMovementSpeed;
    [SerializeField] float lerpSpeed;
    [SerializeField] float FOVSpeed;
    [SerializeField] float minFOV, maxFOV;
    [SerializeField] Transform explosionPosition, sideExplosion;
    [SerializeField] float force, sideForce;
    [SerializeField] float explodeRadius;    
    [SerializeField] float yRotationSpeed;
    [SerializeField] float explodeTime;
    [SerializeField] float upwardsForce, upwardsSideForce;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] int maxShots;

    Collider[] hitCollide;

    Camera thisCamera;
    Rigidbody thisRB;
    Vector3 lastVelocity;

    float explodeInput = 0;
    float xDirection, zDirection;
    Vector3 camForward, camRight;
    bool addedForce;
    Vector3 originalExplodePosition;
    int currentShot = 0;
    bool inAir = false;
    float distanceBelow;

    //Glide Variables
    float glideInput = 0;
    bool gliding = false;
    [SerializeField] Vector3 newGravity;
    Vector3 origGravity;
    Vector3 velocityInfluence;
    [SerializeField] float basedGlideSpeed, incrementedGlideSpeed;

    //Jump Variables
    [SerializeField] float jumpForce;
    float jumpInput = 0.0f;
    float lastJumpInput = 0.0f;

	// Use this for initialization
	void Start ()
    {
        thisRB = GetComponent<Rigidbody>();
        thisCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        origGravity = Physics.gravity;
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        GroundCheck();
        GetInput();
        Jump();
        MovePlayer();
        ExplodeDirection();
        Glide();
	}

    void GetInput ()
    {
        xDirection = Input.GetAxis("Horizontal");
        zDirection = Input.GetAxis("Vertical");

        explodeInput = Input.GetAxisRaw("ExplodeInput");
        glideInput = Input.GetAxisRaw("GlideInput");

        if (Input.GetAxisRaw("Jump") != lastJumpInput)
        { jumpInput = Input.GetAxisRaw("Jump"); lastJumpInput = jumpInput; }
        else { jumpInput = 0; }
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
        if (!addedForce && !inAir)
        {
            thisRB.velocity = camForward * zDirection * playerSpeed + camRight * xDirection * playerSpeed + jumpVelocity;
        }
        else if(addedForce || inAir)
        {
            thisRB.velocity = Vector3.Lerp(thisRB.velocity, thisRB.velocity.normalized + camForward * zDirection * airMovementSpeed + camRight * xDirection * airMovementSpeed, Time.deltaTime);
            thisRB.velocity = thisRB.velocity;
        }
        else if(gliding)
        {
            thisRB.velocity = Vector3.Lerp(thisRB.velocity, thisRB.velocity.normalized + camForward * ((zDirection < 0f) ? basedGlideSpeed : zDirection + incrementedGlideSpeed) * airMovementSpeed + jumpVelocity, Time.deltaTime);
        }


        if (Mathf.Abs(zDirection) > 0.65 || Mathf.Abs( xDirection ) > 0.65)
        {
            float zValue = zDirection, xValue = xDirection;
            if (zDirection < -0.5)
            {
                zValue = Mathf.Clamp(zValue*2f, -0.9f, zValue * 2f);
            }
            else if(zDirection > 0.5)
            {
                zValue = Mathf.Clamp(zValue * 2f, zValue * 2f, 0.9f);
            }
            if(xDirection < -0.5)
            {
                xValue = Mathf.Clamp(xValue * 2f, -0.9f, xValue * 2f);
            }
            else if(xDirection > 0.5)
            {
                xValue = Mathf.Clamp(xValue * 2f, xValue * 2f, 0.9f);
            }

            if(Mathf.Abs(xValue) > 0.65f && Mathf.Abs(zValue) > 0.65f)
            {
                if (zDirection < -0.5)
                {
                    zValue = Mathf.Clamp(zValue * 2f, -0.75f, zValue * 2f);
                }
                else if (zDirection > 0.5)
                {
                    zValue = Mathf.Clamp(zValue * 2f, zValue * 2f, 0.75f);
                }
                if (xDirection < -0.5)
                {
                    xValue = Mathf.Clamp(xValue * 2f, -0.75f, xValue * 2f);
                }
                else if (xDirection > 0.5)
                {
                    xValue = Mathf.Clamp(xValue * 2f, xValue * 2f, 0.75f);
                }

            }

            sideExplosion.position = camForward * (-zValue) + camRight * (-xValue) + transform.position;
            sideExplosion.position = new Vector3(sideExplosion.position.x, transform.position.y - 1f, sideExplosion.position.z);

        }
        else
        {
           // sideExplosion.position = camForward   + camRight + transform.position + sideExplosion.position; //So this never increments..??
            sideExplosion.position = new Vector3(transform.position.x, transform.position.y - 1.5f, transform.position.z);

        }

        Vector3 targetDirection;
        
        targetDirection = transform.position + (camForward * 5);
        
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
        if(hitCollide.Length > 0)
        {
            inAir = false;
            currentShot = 0;
        }
        else
        {
            inAir = true;
        }

    }

    void ExplodeDirection()
    {

        if(currentShot > maxShots)
        {
            return;
        }

        
        if(explodeInput == 1.0 && !addedForce && !gliding)
        {
            thisRB.velocity = Vector3.zero;
            thisRB.AddExplosionForce(sideForce, sideExplosion.position, explodeRadius, upwardsSideForce, ForceMode.Impulse);
            originalExplodePosition = sideExplosion.position;
            currentShot++;
            StartCoroutine(ExplodeTime(explodeTime));
        }
        
    }

    void Jump()
    {
        if(jumpInput != 0.0f && !inAir)
        {
            thisRB.AddForce(new Vector3(0, jumpForce, 0));
        }
    }

    void Glide()
    {

        //Always in direction of camera forward, left stick controlls left and right movement
        if(glideInput == 1.0f && !addedForce && inAir)
        {
            thisRB.velocity = new Vector3(thisRB.velocity.x, thisRB.velocity.y / 2, thisRB.velocity.z);
           
            gliding = true;
        }
        else
        {
            gliding = false;
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
