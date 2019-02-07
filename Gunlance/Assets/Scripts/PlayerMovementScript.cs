using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour {

    [SerializeField] Transform cameraTransform;
    [SerializeField] float playerSpeed;
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

    //Glide Variables
    float glideInput = 0;
    bool gliding = false;
    [SerializeField] Vector3 newGravity;
    Vector3 origGravity;

	// Use this for initialization
	void Start ()
    {
        thisRB = GetComponent<Rigidbody>();
        thisCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        origGravity = Physics.gravity;
    }
	
	// Update is called once per frame
	void Update ()
    {
        GroundCheck();
        GetInput();
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

        sideExplosion.position = camForward * (-zDirection/1) + camRight * (-xDirection/1) +transform.position;
        sideExplosion.position = new Vector3(sideExplosion.position.x, sideExplosion.position.y - 1f, sideExplosion.position.z);

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

        if(explodeInput == 1.0 && !inAir && currentShot == 0 && !addedForce && !gliding)
        {
            thisRB.velocity = Vector3.zero;
            thisRB.AddExplosionForce(force, explosionPosition.position, explodeRadius, upwardsForce, ForceMode.Impulse);
            currentShot++;
            originalExplodePosition = explosionPosition.position;
            StartCoroutine(ExplodeTime(explodeTime));
        }

        if(explodeInput == 1.0 && inAir && currentShot > 0 && !addedForce && !gliding)
        {
            thisRB.velocity = Vector3.zero;
            thisRB.AddExplosionForce(sideForce, sideExplosion.position, explodeRadius, upwardsSideForce, ForceMode.Impulse);
            originalExplodePosition = sideExplosion.position;
            currentShot++;
            StartCoroutine(ExplodeTime(explodeTime));
        }
        
    }


    void Glide()
    {
        if(glideInput == 1.0f && !addedForce && inAir)
        {
            thisRB.velocity = new Vector3(thisRB.velocity.x, thisRB.velocity.y / 2, thisRB.velocity.z);
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
