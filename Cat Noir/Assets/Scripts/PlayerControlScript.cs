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
    bool jumpInput = false;
    bool justJumped = false;

    bool pouncing = false;
    private float currenHitDistance;


    // Use this for initialization
    void Start()
    {
        thisTransform = gameObject.transform;
        thisRigidbody = gameObject.GetComponent<Rigidbody>();
        thisCollider = gameObject.GetComponent<BoxCollider>();
        colliderTransform = thisCollider.transform;
    }

    // Update is called once per frame
    void Update()
    {

        UpdateInput();
        UpdatePosition();
        Crouch();
        Jump();

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

        if (Input.GetAxisRaw("Jump") == 1)
        {
            jumpInput = true;
        }
        else
        {
            jumpInput = false;
        }

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
        Gizmos.DrawSphere(groundCheck.position, 0.2f);

    }

    void Jump()
    {

        if (jumpInput && !jumping)
        {
            thisRigidbody.velocity = thisRigidbody.velocity + (transform.up * 5);

            jumping = true;
        }
        else if (jumping)
        {
            if (Physics.SphereCast(groundCheck.position, .2f, transform.forward, out ceilingTest,0f, groundLayer))
            {
                jumping = false;
            }
        }
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

        //thisRigidbody.velocity = new Vector3(playerX * playerSpeed, 0, playerZ* playerSpeed);
        thisRigidbody.velocity = camForward * playerZ * playerSpeed + camRight * playerX * playerSpeed;

    }
}
