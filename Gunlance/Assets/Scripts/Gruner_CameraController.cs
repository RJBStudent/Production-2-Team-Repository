using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gruner_CameraController : MonoBehaviour 
{

    string os; //check OS so that Mac users can test/play

    [SerializeField] Transform playerTransform;
    [SerializeField] float heightRestrictMax, heightRestrictMin;
    [SerializeField] float xSpeed, ySpeed;
    [SerializeField] float distance;
    [SerializeField] float lerpSpeed;

    float xMovement = 0, yMovement = 0;

    Vector3 newPos, pos;


    //Temporary Debug Values
    bool removeCameraControl = false;
    float xPos, zPos;


    // Use this for initialization
    void Start()
    {
        //Check operating system for controller input
        GetOS();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        //Dont move the camera with the player if the control is removed
        if (!removeCameraControl)
        {
            UpdatePosition();
        }
        else
        {
            UpdateCamera();
        }
    }

    void GetInput()
    {
        //Which direction to move in
        xMovement += Input.GetAxis("Mouse X" + os) * xSpeed;

        yMovement -= Input.GetAxis("Mouse Y" + os) * ySpeed;

        //Temporary Movement
        xPos = Input.GetAxis("Horizontal");
        zPos = Input.GetAxis("Vertical");


        //TEMPORARY FILM CONTROL
        if (Input.GetKey(KeyCode.C))
        {
            removeCameraControl = true;
        }

        //Clamp how high and low the camera can go
        if (!removeCameraControl)
        {
            yMovement = Mathf.Clamp(yMovement, heightRestrictMin, heightRestrictMax);
        }

    }


    void UpdatePosition()
    {
        Vector3 dir = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(yMovement, xMovement, 0);

        //Move the position to the players position plus the offset
        newPos = playerTransform.position + rotation * dir;
        pos = Vector3.Lerp(pos, newPos, Time.deltaTime * lerpSpeed);


        transform.position = pos;
        transform.LookAt(playerTransform.position);
    }

    //Camera FILM TEMPORARY
    void UpdateCamera()
    {

        Quaternion rotation = Quaternion.Euler(yMovement, xMovement, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * lerpSpeed);

        transform.Translate(Vector3.right * xPos / lerpSpeed);
        transform.Translate(transform.up * zPos / lerpSpeed, Space.World);


    }

    void GetOS()
    {
        os = SystemInfo.operatingSystem;

        if (os.Contains("Mac"))
        {
            os = "_Mac";
        }
        else
        {
            os = null;
        }
    }
}
