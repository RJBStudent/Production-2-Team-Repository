using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlScript : MonoBehaviour
{

    [SerializeField] private float playerSpeed;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private CapsuleCollider thisCollider;
    [SerializeField] private float standingHeight;
    [SerializeField] private float crouchingHeight;
    [SerializeField] private LayerMask ignorePlayer;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float gravity;
    [SerializeField] private float jumpForce;
    [SerializeField] private float shortHopForce;
    [SerializeField] private GameObject pounceTarget;
    [SerializeField] private LayerMask targetRaycast;
    [SerializeField] private float pounceDistance; // greater than 10
    [SerializeField] private float yTargetDistance;
    [SerializeField] private float initialAngle;
    [SerializeField] private float pounceForce;
    [SerializeField] private float ignoreGroundFrames;
    float playerX;
    float playerZ;
    Transform thisTransform;
    Rigidbody thisRigidbody;
    Vector3 camForward;
    Vector3 camRight;


    Transform colliderTransform;
    bool crouching = false;
    bool crouchInput = false;
    RaycastHit ceilingTest;

    Collider[] hitCollide;

    bool jumping = false;
    bool falling = false;
    bool jumpInput = false;
    bool justJumped = false;
    float currentJumpTime = 0;

    bool pouncing = false;
    bool inAirPounce = false;
    bool pounceReady = false;
    bool targetExists = false;
    float currentIgnore = 0;
    RaycastHit surfaceHit;
    Transform pounceTargetTransform;

    Animator playerAnimator;

    // Use this for initialization
    void Start()
    {
        thisTransform = gameObject.transform;
        thisRigidbody = gameObject.GetComponent<Rigidbody>();
        thisCollider = gameObject.GetComponent<CapsuleCollider>();
        colliderTransform = thisCollider.transform;
        pounceTarget.layer = LayerMask.NameToLayer("DontRender");
        pounceTargetTransform = pounceTarget.transform;
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        UpdateInput();
        Pounce();
        GroundCheck();
        Jump();
        UpdatePosition();
        Crouch();

    }

    void UpdateInput()
    {
        playerX = Input.GetAxis("Horizontal");
        playerZ = Input.GetAxis("Vertical");

        if (Input.GetAxisRaw("Crouch") == 1)
        {
            crouchInput = true;
        }
        else
        {
            crouchInput = false;
        }

        jumpInput = Input.GetButton("Jump");       

    }

    void Crouch()
    {
        if (crouchInput && !jumping)
        {
            crouching = true;
            playerAnimator.SetTrigger("Crouching");
            //set collidor to be shorter and change color(for now)
            thisCollider.height = crouchingHeight;
            thisCollider.center = new Vector3(0, -0.5f, 0);
            playerAnimator.ResetTrigger("Idle");
        }
        else if (!crouchInput)
        {
            //check if there is anything above and dont stand up if there is
            //otherwise standup if just crouched

            if (crouching /*&& is something above me */)
            {
                if (Physics.SphereCast(transform.position, 0.5f, transform.up, out ceilingTest, 2f, ignorePlayer))
                {
                    return;

                }
                thisCollider.height =standingHeight;
                thisCollider.center = new Vector3(0, 0, 0);
                playerAnimator.SetTrigger("Idle");
            }

            crouching = false;
            playerAnimator.ResetTrigger("Crouching");

        }


    }
    

    void Jump()
    {
        //***WORKING ITERATION *****
        
        if (jumpInput && hitCollide.Length > 0 && !crouching)
        {
            thisRigidbody.AddForce(new Vector3(0, jumpForce, 0));
            
        }
        
    }

    void GroundCheck()
    {
        hitCollide = null;
        hitCollide = Physics.OverlapSphere(groundCheck.position, 0.21f, groundLayer);
    }

    void Pounce()
    { 
       if(pouncing)
        {
            currentIgnore++;
        }
        if(pouncing && hitCollide.Length == 0)
        {
            inAirPounce = true;
        }
        else if (pouncing && hitCollide.Length > 0 && inAirPounce)
        {
            if (currentIgnore > ignoreGroundFrames)
            {
                pouncing = false;
                playerAnimator.SetTrigger("Idle");
                playerAnimator.ResetTrigger("Pouncing");
            }
        }

        if (crouchInput && jumpInput)
        {
            pounceReady = true;
            playerAnimator.SetTrigger("PounceReady");
            playerAnimator.ResetTrigger("Crouching");
        }
        else
        {

            playerAnimator.ResetTrigger("PounceReady");
        }
        if(pounceReady)
        {
            Vector3 targetDirection = new Vector3(cameraTransform.forward.x, cameraTransform.forward.y + yTargetDistance, cameraTransform.forward.z);
            if(Physics.Raycast(transform.position, targetDirection, out surfaceHit, pounceDistance, targetRaycast))
            {
                pounceTarget.layer = LayerMask.NameToLayer("Default");
                pounceTargetTransform.position = surfaceHit.point;
                //pounceTargetTransform.rotation = Quaternion.Euler(Mathf.Rad2Deg * surfaceHit.normal.x, Mathf.Rad2Deg * surfaceHit.normal.y, Mathf.Rad2Deg * surfaceHit.normal.z);
                // pounceTargetTransform.rotation = Quaternion.LookRotation(surfaceHit.normal);
                pounceTargetTransform.rotation = Quaternion.FromToRotation(transform.up, surfaceHit.normal);
                targetExists = true;
            }
            else
            {
                targetExists = false;
                pounceReady = false;
                pounceTarget.layer = LayerMask.NameToLayer("DontRender");
            }
        }
        if(pounceReady && (!jumpInput || !crouchInput) && targetExists)
        {
            pounceReady = false;
            pouncing = true;

            float angle = initialAngle * Mathf.Deg2Rad;

            Vector3 planarTarget = new Vector3(pounceTargetTransform.position.x, 0, pounceTargetTransform.position.z);
            Vector3 planarPosition = new Vector3(transform.position.x, 0, transform.position.z);

            float distance = Vector3.Distance(planarTarget, planarPosition);
            float yOffset = transform.position.y - pounceTargetTransform.position.y;

            float initialVelocity = (1 / Mathf.Cos(angle)) 
                * Mathf.Sqrt((0.5f * Physics.gravity.magnitude * Mathf.Pow(distance, 2)) 
                / (distance * Mathf.Tan(angle) + yOffset));
            Debug.Log(initialVelocity);

            Vector3 vel = new Vector3(0, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));


            float angleBetweenObjects = Vector3.Angle(transform.forward, planarTarget - planarPosition) 
                * (pounceTargetTransform.position.x > transform.position.x ? 1 : -1);
            Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, transform.up) * vel;

            thisRigidbody.velocity = Vector3.zero;
            thisRigidbody.AddForce(finalVelocity*thisRigidbody.mass * pounceForce, ForceMode.Impulse);
            pounceTarget.layer = LayerMask.NameToLayer("DontRender");
            pouncing = true;
            currentIgnore = 0;
            playerAnimator.SetTrigger("Pouncing");
        }
    }

    void UpdatePosition()
    {
        camForward = cameraTransform.forward;
        camRight = cameraTransform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();
        Vector3 jumpVelocity = new Vector3(0, thisRigidbody.velocity.y, 0);

        //thisRigidbody.velocity = new Vector3(playerX * playerSpeed, 0, playerZ* playerSpeed);
        if (!pouncing)
        {
            thisRigidbody.velocity = camForward * playerZ * playerSpeed + camRight * playerX * playerSpeed + jumpVelocity;
        }
    }
}
