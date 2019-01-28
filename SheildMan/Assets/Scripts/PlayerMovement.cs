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
    float redShieldInput = 0, blueShieldInput = 0, lastRedInput = 0, lastBlueInput = 0;
    [SerializeField] float redShieldXDistance, blueShieldXDistance;
    [SerializeField] float shieldInFrontDistance;
    Transform currentShieldInFront, shieldOnSide;
    [SerializeField] float shieldSize;
    [SerializeField] float shieldTransformSpeed;
    bool switchingShield;
    bool switchingShieldToSide;
    Vector3 inFrontPosition, onSidePosition;
    Vector3 currentInitialPosition, otherInitialPosition;

    float blueRotateAmount = 90, redRotateAmount = -90;
    bool shieldUp;
    [SerializeField] float shieldScaleSize;
    Vector3 orignalScale, newScale;

    bool initFront = false;
    bool initSide = false;

    Vector3 shieldFrontTargetPosition, shieldSideTargetPosition;
    float switchingRotateDegreesOld = 0, switchingRotateDegreesNew = 0;

    bool stopPlayer = false;
    bool finishShieldRotate = false;

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

        ShieldUpdate();
        if (stopPlayer)
        { return; }

        GetInput();
        MovePlayer();

    }

    void GetInput()
    {
        //Movement
        xDirection = Input.GetAxisRaw("Horizontal");
        zDirection = Input.GetAxisRaw("Vertical");

        //Sheild Input
        if (Input.GetAxisRaw("Right Shield") != lastRedInput)
        { redShieldInput = Input.GetAxisRaw("Right Shield"); lastRedInput = redShieldInput; }
        else { redShieldInput = 0; }

        if (Input.GetAxisRaw("Left Shield") != lastBlueInput)
        { blueShieldInput = Input.GetAxisRaw("Left Shield"); lastBlueInput = blueShieldInput; }
        else { blueShieldInput = 0; }
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


                if (initFront)
                {
                    currentInitialPosition = currentShieldInFront.position - transform.position;
                    currentInitialPosition.y = 0;
                    initFront = false;
                }

                inFrontPosition = currentShieldInFront.position - transform.position;



                inFrontPosition.y = 0;
                float angleBetweenOld = Vector3.Angle(currentInitialPosition, inFrontPosition) * (Vector3.Cross(currentInitialPosition, inFrontPosition).y > 0 ? 1 : -1);
                
                float newAngleOld;
                if (currentShieldInFront == redShield)
                {
                    switchingRotateDegreesOld -= shieldTransformSpeed * Time.deltaTime;
                    newAngleOld = Mathf.Clamp(angleBetweenOld + switchingRotateDegreesOld, redRotateAmount, 0);

                    if (newAngleOld == redRotateAmount)
                    {

                        switchingShield = false;
                        switchingRotateDegreesOld = 0;
                    }

                }
                else
                {
                    switchingRotateDegreesOld += shieldTransformSpeed * Time.deltaTime;
                    newAngleOld = Mathf.Clamp(angleBetweenOld + switchingRotateDegreesOld, 0, blueRotateAmount);

                    if (newAngleOld == blueRotateAmount)
                    {
                        switchingShield = false;
                        switchingRotateDegreesOld = 0;
                    }

                }


                switchingRotateDegreesOld = newAngleOld - angleBetweenOld;


                currentShieldInFront.RotateAround(transform.position, Vector3.up, switchingRotateDegreesOld);
            }

        }

        if (switchingShieldToSide)
        {

            if (shieldOnSide)
            {


                if (initSide)
                {

                    otherInitialPosition = shieldOnSide.position - transform.position;
                    otherInitialPosition.y = 0;
                    initSide = false;
                }

                onSidePosition = shieldOnSide.position - transform.position;


                onSidePosition.y = 0;
                float angleBetweenNew = Vector3.Angle(otherInitialPosition, onSidePosition) * (Vector3.Cross(otherInitialPosition, onSidePosition).y > 0 ? 1 : -1);

                float newAngleNew;
                if (shieldOnSide == redShield)
                {
                    switchingRotateDegreesNew += shieldTransformSpeed * Time.deltaTime;
                    newAngleNew = Mathf.Clamp(angleBetweenNew + switchingRotateDegreesNew, 0, -redRotateAmount);

                    if (newAngleNew == -redRotateAmount)
                    {

                        switchingShieldToSide = false;
                        switchingRotateDegreesNew = 0;

                    }

                }
                else
                {
                    switchingRotateDegreesNew -= shieldTransformSpeed * Time.deltaTime;
                    newAngleNew = Mathf.Clamp(angleBetweenNew + switchingRotateDegreesNew, -blueRotateAmount, 0);

                    if (newAngleNew == -blueRotateAmount)
                    {

                        switchingShieldToSide = false;
                        switchingRotateDegreesNew = 0;
                    }
                }


                switchingRotateDegreesNew = newAngleNew - angleBetweenNew;


                shieldOnSide.RotateAround(transform.position, Vector3.up, switchingRotateDegreesNew);

            }
        }


        if ((redShieldInput == 1 || blueShieldInput == 1) && (!switchingShield && !switchingShieldToSide))
        {
            if (currentShieldInFront)
            {
                if (currentShieldInFront == redShield)
                {
                    shieldOnSide = redShield;
                    switchingShieldToSide = true;
                    initSide = true;
                    currentShieldInFront.localScale = orignalScale;
                    currentShieldInFront = null;
                    if (blueShieldInput == 1)
                    {
                        currentShieldInFront = blueShield;
                        switchingShield = true;
                        initFront = true;
                        currentShieldInFront.localScale = newScale;
                    }
                }
                else if (currentShieldInFront == blueShield)
                {
                    shieldOnSide = blueShield;
                    switchingShieldToSide = true;
                    initSide = true;
                    currentShieldInFront.localScale = orignalScale;
                    currentShieldInFront = null;
                    if (redShieldInput == 1)
                    {
                        currentShieldInFront = redShield;
                        switchingShield = true;
                        initFront = true;
                        currentShieldInFront.localScale = newScale;
                    }
                }
            }
            else if (!currentShieldInFront)
            {
                if (redShieldInput == 1)
                {
                    currentShieldInFront = redShield;
                    switchingShield = true;
                    initFront = true;
                    currentShieldInFront.localScale = newScale;
                }
                else if (blueShieldInput == 1)
                {
                    currentShieldInFront = blueShield;
                    switchingShield = true;
                    initFront = true;
                    currentShieldInFront.localScale = newScale;
                }
            }
            redShieldInput = 0;
            blueShieldInput = 0;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Red"))
        {
            if (currentShieldInFront != redShield)
            {
                ResetPlayerTransform();
            }
            Destroy(other.gameObject);
            return;
        }
        if (other.gameObject.CompareTag("Blue"))
        {
            if (currentShieldInFront != blueShield)
            {
                ResetPlayerTransform();
            }
            Destroy(other.gameObject);
            return;
        }
    }

    void ResetPlayerTransform()
    {
        transform.position = new Vector3(0.0f, 0.0f, 0.0f);

        if(currentShieldInFront)
        {
            if(currentShieldInFront == redShield)
            {
                redShieldInput = 1;
            }
            else if(currentShieldInFront == blueShield)
            {
                blueShieldInput = 1;
            }
        }
        

        StartCoroutine(FreezePlayer(1f));
    }

    IEnumerator FreezePlayer(float time)
    {
        stopPlayer = true;
        yield return new WaitForSecondsRealtime(time);
        stopPlayer = false;
    }

}
