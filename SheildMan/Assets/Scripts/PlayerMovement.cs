using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] float playerSpeed;
    [SerializeField] float acceleration;
    [SerializeField] Transform redShield;
    [SerializeField] Transform blueShield;

    CharacterController thisPlayerControl;

    //Movement Variables
    float xDirection, zDirection;
    Vector3 velocity;

    //Sheild Variables
    float redShieldInput = 0, blueShieldInput = 0;
    [SerializeField] float redShieldXDistance, blueShieldXDistance;
    [SerializeField] float shieldInFrontDistance;
    Transform currentShieldInFront, shieldOnSide;
    [SerializeField] float shieldSize;
    [SerializeField] float shieldTransformSpeed;
    bool switchingShield;
    bool switchingShieldToSide;
    Vector3 inFrontPosition;
    Vector3 currentInitialPosition, otherInitialPosition;
    bool shieldUp;
    [SerializeField] float shieldScaleSize;
    Vector3 orignalScale, newScale;

    Vector3 shieldFrontTargetPosition, shieldSideTargetPosition;
    float switchingRotateDegreesOld = 0, switchingRotateDegreesNew = 0;


    // Use this for initialization
    void Start()
    {

        thisPlayerControl = GetComponent<CharacterController>();
        orignalScale = new Vector3(0.5f, 1.0f, 1.0f);
        newScale = new Vector3(0.5f, shieldScaleSize, shieldScaleSize);
        redShield.localScale = orignalScale;
        blueShield.localScale = orignalScale;

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        GetInput();
        MovePlayer();
        ShieldUpdate();

    }

    void GetInput()
    {
        //Movement
        xDirection = Input.GetAxisRaw("Horizontal");
        zDirection = Input.GetAxisRaw("Vertical");

        //Sheild Input
        if (Input.GetAxisRaw("Right Shield") == 1 && redShieldInput == 0) redShieldInput = Input.GetAxisRaw("Right Shield"); else redShieldInput = 0.0f;
        if (Input.GetAxisRaw("Left Shield") == 1 && blueShieldInput == 0) blueShieldInput = Input.GetAxisRaw("Left Shield"); else blueShieldInput = 0.0f;
    }

    void MovePlayer()
    {
        Vector3 dir = new Vector3(xDirection, 0, zDirection);

        velocity = Vector3.Lerp(velocity, dir * playerSpeed, acceleration * Time.deltaTime);

        thisPlayerControl.Move(velocity * Time.deltaTime);

    }

    void ShieldUpdate()
    {
        /* 1 - check Input
         * 2 - check if a sheild is already up
         * 3 - set current shield equiped
         * 4 - Apply transformations over time to sheilds (
         *      *Use boolean to show switch
         * 5 - Apply Collision while switching
         *  
         */


        

        if (switchingShield)
        {

            if (currentShieldInFront)
            {
                switchingRotateDegreesOld += shieldTransformSpeed * Time.deltaTime;
                shieldFrontTargetPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + shieldInFrontDistance);

                inFrontPosition = currentShieldInFront.position - shieldFrontTargetPosition;
                if (inFrontPosition.magnitude < 1)
                {

                    Debug.Log(inFrontPosition.magnitude);
                    switchingShield = false;
                    switchingRotateDegreesOld = 0;
                    //currentShieldInFront = null;
                }

                inFrontPosition.y = 0;
                float angleBetweenOld = Vector3.Angle(currentInitialPosition, inFrontPosition) * (Vector3.Cross(currentInitialPosition, inFrontPosition).y > 0 ? 1 : -1);
                float newAngleOld = Mathf.Clamp(angleBetweenOld + switchingRotateDegreesOld, -90, 90);
                switchingRotateDegreesOld = newAngleOld - angleBetweenOld;


                currentShieldInFront.RotateAround(transform.position, Vector3.up, switchingRotateDegreesOld);

            }

        }
        
        if (switchingShieldToSide)
        {
            if (shieldOnSide)
            {
                switchingRotateDegreesNew += shieldTransformSpeed * Time.deltaTime;

                if (shieldOnSide == redShield)
                {
                    shieldSideTargetPosition = new Vector3(transform.position.x + redShieldXDistance, transform.position.y, transform.position.z);
                }
                else
                {
                    shieldSideTargetPosition = new Vector3(transform.position.x + blueShieldXDistance, transform.position.y, transform.position.z);
                }

                inFrontPosition = shieldOnSide.position - shieldSideTargetPosition;

                if (inFrontPosition.magnitude < 1)

                {
                    Debug.Log(inFrontPosition.magnitude);
                    switchingShieldToSide = false;
                    switchingRotateDegreesNew = 0;
                    //ShieldOneSide = null;
                }

                inFrontPosition.y = 0;
                float angleBetweenNew = Vector3.Angle(otherInitialPosition, inFrontPosition) * (Vector3.Cross(otherInitialPosition, inFrontPosition).y > 0 ? 1 : -1);
                float newAngleNew = Mathf.Clamp(angleBetweenNew + switchingRotateDegreesNew, -90, 90);
                switchingRotateDegreesNew = newAngleNew - angleBetweenNew;


                shieldOnSide.RotateAround(transform.position, Vector3.up, switchingRotateDegreesNew);
            }
        }
        

        if ((redShieldInput == 1 || blueShieldInput == 1))
        {
            if(currentShieldInFront)
            {
                if(currentShieldInFront == redShield)
                {
                    shieldOnSide = redShield;
                    switchingShieldToSide = true;
                    if(blueShieldInput == 1)
                    {
                        currentShieldInFront = blueShield;
                        switchingShield = true;
                    }
                    else
                    {
                        currentShieldInFront = null;
                    }
                }
                else if(currentShieldInFront == blueShield)
                {
                    shieldOnSide = blueShield;
                    switchingShieldToSide = true;
                    currentShieldInFront = null;
                    if (redShieldInput == 1)
                    {
                        currentShieldInFront = redShield;
                        switchingShield = true;
                    }
                    else
                    {
                        currentShieldInFront = null;
                    }
                }
            }
            else if(!currentShieldInFront)
            {
                if(redShieldInput == 1)
                {
                    currentShieldInFront = redShield;
                    switchingShield = true;
                }
                else if(blueShieldInput == 1)
                {
                    currentShieldInFront = blueShield;
                    switchingShield = true;
                }
            }
        }




    //    if ((redShieldInput == 1 || blueShieldInput == 1) && (!switchingShield || !switchingShieldToSide))
    //    {
    //        if (redShieldInput == 1 && currentShieldInFront != blueShield)
    //        {
    //            if (currentShieldInFront == redShield)
    //            {
    //                shieldOnSide = redShield;
    //                otherInitialPosition = redShield.position;
    //                switchingShieldToSide = true;
    //                shieldOnSide.localScale = orignalScale;
    //                return;
    //            }
    //            currentShieldInFront = redShield;
    //            shieldOnSide = blueShield;
    //            currentInitialPosition = currentShieldInFront.position;
    //            otherInitialPosition = blueShield.position;
    //            switchingShield = true;
    //            currentShieldInFront.localScale = newScale;
    //        }
    //        else if (blueShieldInput == 1 && currentShieldInFront != redShield)
    //        {

            //            if (currentShieldInFront == blueShield)
            //            {
            //                shieldOnSide = blueShield;
            //                otherInitialPosition = blueShield.position;
            //                switchingShieldToSide = true;
            //                shieldOnSide.localScale = orignalScale;
            //                return;
            //            }
            //            currentShieldInFront = blueShield;
            //            shieldOnSide = redShield;
            //            currentInitialPosition = currentShieldInFront.position;
            //            otherInitialPosition = redShield.position;
            //            switchingShield = true;
            //            currentShieldInFront.localScale = newScale;
            //        }
            //        else if (redShieldInput == 1 && currentShieldInFront == blueShield)
            //        {
            //            currentShieldInFront = redShield;
            //            shieldOnSide = blueShield;
            //            currentInitialPosition = currentShieldInFront.position;
            //            otherInitialPosition = blueShield.position;
            //            switchingShield = true;
            //            switchingShieldToSide = true;
            //            currentShieldInFront.localScale = newScale;
            //        }
            //        else if (blueShieldInput == 1 && currentShieldInFront == redShield)
            //        {
            //            currentShieldInFront = blueShield;
            //            shieldOnSide = redShield;
            //            currentInitialPosition = currentShieldInFront.position;
            //            otherInitialPosition = redShield.position;
            //            switchingShield = true;
            //            switchingShieldToSide = true;
            //            currentShieldInFront.localScale = newScale;
            //        }
            //    }

            }
    }
