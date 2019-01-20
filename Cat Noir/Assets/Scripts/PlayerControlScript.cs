using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlScript : MonoBehaviour
{

    [SerializeField] private float playerSpeed;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private BoxCollider thisCollider;
    [SerializeField] private float standingHeight;
    [SerializeField] private float crouchingHeight;
    [SerializeField] private LayerMask ignorePlayer;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float gravity;
    [SerializeField] private float jumpForce;
    [SerializeField] private float shortHopForce;
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


    bool jumping = false;
    bool falling = false;
    bool jumpInput = false;
    bool justJumped = false;
    float currentJumpTime = 0;

    bool pouncing = false;
    private float currenHitDistance;
    Vector3 down;

    // Use this for initialization
    void Start()
    {
        thisTransform = gameObject.transform;
        thisRigidbody = gameObject.GetComponent<Rigidbody>();
        thisCollider = gameObject.GetComponent<BoxCollider>();
        colliderTransform = thisCollider.transform;
        down = -1 * transform.up;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        UpdateInput();

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

        jumpInput = Input.GetButtonDown("Jump");       

    }

    void Crouch()
    {
        if (crouchInput && !jumping)
        {
            crouching = true;
            //set collidor to be shorter and change color(for now)
            thisCollider.size = new Vector3(colliderTransform.localScale.x, crouchingHeight, colliderTransform.localScale.z);
            thisCollider.center = new Vector3(0, -0.5f, 0);

        }
        else if (!crouchInput)
        {
            //check if there is anything above and dont stand up if there is
            //otherwise standup if just crouched

            if (crouching /*&& is something above me */)
            {
                if (Physics.SphereCast(transform.position, 0.5f, transform.up, out ceilingTest, 2f, ignorePlayer))
                {
                    currenHitDistance = ceilingTest.distance;
                    return;

                }
                thisCollider.size = new Vector3(colliderTransform.localScale.x, standingHeight, colliderTransform.localScale.z);
                thisCollider.center = new Vector3(0, 0, 0);

            }

            crouching = false;

        }


    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(groundCheck.position + down * 0f, 0.201f);

    }

    void Jump()
    {
        Collider[] hitCollide = Physics.OverlapSphere(groundCheck.position, 0.201f, groundLayer);
        if (jumpInput && hitCollide.Length > 0)
        {
            thisRigidbody.AddForce(new Vector3(0, jumpForce, 0));
        }
        
        //if (!jumping)
        //{

        //    Collider[] hitCollide = Physics.OverlapSphere(groundCheck.position, 0.201f, groundLayer);

        //    if (hitCollide.Length > 0)
        //    {
        //        thisRigidbody.velocity = new Vector3(thisRigidbody.velocity.x, 0, thisRigidbody.velocity.z);
                
        //    }
        //}
        //if (jumpInput)
        //{
        //    thisRigidbody.velocity = thisRigidbody.velocity + (Vector3.up *  jumpForce);
        //    //Debug.Log(thisRigidbody.velocity.y);
        //    jumping = true;

        //}

        //if (thisRigidbody.velocity.y < 0)
        //{
        //   // Debug.Log("Fall");
        //    thisRigidbody.velocity += Vector3.up * Physics.gravity.y * (gravity - 1) * Time.deltaTime;
        //    jumping = false;
           
        //}
        //else if(thisRigidbody.velocity.y > 0 && !jumpInput)
        //{
        //   // Debug.Log("ShortJump");
        //    thisRigidbody.velocity += Vector3.up * Physics.gravity.y * (shortHopForce - 1) * Time.deltaTime;
        //}

        //if(jumpInput && !jumping)
        //{
        //    thisRigidbody.velocity = thisRigidbody.velocity + (transform.up * jumpForce);
        //    jumping = true;
        //}
        //if(jumping)
        //{
        //    thisRigidbody.velocity = thisRigidbody.velocity + (Physics.gravity * gravity * Time.deltaTime);
        //   v

        //    if (hitCollide.Length > 0)
        //    {
        //        Debug.Log("Ground");
        //        jumping = false;
        //    }
        //}


        //if (jumpInput && !jumping)
        //{

        //    currentJumpTime = 0;
        //    currentJumpTime += Time.deltaTime;
        //    thisRigidbody.velocity = thisRigidbody.velocity + (transform.up * jumpTime -Mathf.Pow(jumpMultiplier*currentJumpTime, shortHopForce));

        //    jumping = true;            
        //}
        //else if (jumping)
        //{
        //    if (currentJumpTime < jumpTime && !falling)
        //    {
        //        thisRigidbody.velocity = thisRigidbody.velocity + (transform.up * Mathf.Pow(jumpMultiplier*currentJumpTime, shortHopForce));
        //        currentJumpTime += Time.deltaTime;
        //    }
        //    else if(currentJumpTime >= jumpTime)
        //    {
        //        currentJumpTime = 0;
        //        falling = true;
        //    }
        //    else
        //    {
        //        currentJumpTime += Time.deltaTime; ;
        //        thisRigidbody.velocity = thisRigidbody.velocity + (down * Mathf.Pow(jumpMultiplier * currentJumpTime, shortHopForce));
        //        Debug.Log(thisRigidbody.velocity);

        //        Collider[] hitCollide = Physics.OverlapSphere(groundCheck.position, 0.201f, groundLayer);

        //        if (hitCollide.Length > 0)
        //        {
        //            Debug.Log("Ground");
        //            jumping = false;
        //            falling = false;
        //        }
        //    }
            
        //}
    }

    void Pounce()
    {
        if (crouchInput && jumpInput)
        {
            if (true)
            { }
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
        thisRigidbody.velocity = camForward * playerZ * playerSpeed + camRight * playerX * playerSpeed + jumpVelocity;
        Debug.Log(thisRigidbody.velocity);
    }
}
