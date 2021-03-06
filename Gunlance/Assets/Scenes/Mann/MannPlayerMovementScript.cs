﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MannPlayerMovementScript : MonoBehaviour
{


   [SerializeField] Transform cameraTransform;
   [SerializeField] float playerSpeed;
   [SerializeField] float airMovementSpeed;
   [SerializeField] float FOVSpeed;
   [SerializeField] float minFOV, maxFOV;
   [SerializeField] Transform sideExplosion;
   [SerializeField] float sideForce;
   [SerializeField] float explodeRadius;
   [SerializeField] float explodeTime;
   [SerializeField] float upwardsSideForce;
   [SerializeField] LayerMask groundLayer;
   [SerializeField] LayerMask beastLayer;
   [SerializeField] LayerMask crystalLayer;
   [SerializeField] Transform groundCheck;
   [SerializeField] int maxShots;

   Collider[] hitCollide;

   Camera thisCamera;
   Rigidbody thisRB;

   [SerializeField] Animator clip; //TEMPORARY FOR TRAILER
   bool removeControl = false; // TEMPORARY FOR TRAILER


   //Explode variables
   float explodeInput = 0;
   float lastExplodeInput = 0.0f;
   float xDirection, zDirection;
   Vector3 camForward, camRight;
   bool addedForce;
   Vector3 originalExplodePosition;
   int currentShot = 0;
   bool inAir = false;
   float distanceBelow;

   float crystalsOnScene = 0;

   //Glide Variables
   float glideInput = 0;
   bool gliding = false;
   Vector3 velocityInfluence;
   [SerializeField] float basedGlideSpeed, incrementedGlideSpeed;
   [SerializeField] GameObject gliderTemp;
   [SerializeField] float fallSpeedGliding;

   //Jump Variables
   [SerializeField] float jumpForce;
   float jumpInput = 0.0f;
   float lastJumpInput = 0.0f;

   //Light Variables
   [SerializeField] GameObject thisLight;
   Transform lightTransform;


   //Scale stuff
   Vector3 scale;

   //**************** TOMMY STUFF ***********************
   [SerializeField] bool sliding;
   [SerializeField] bool startingToSlide;
   [SerializeField] bool stoppingToSlide = false;
   [SerializeField] float slidingAngle;
   [SerializeField] float slidingFallOffAngle;
   [SerializeField] PhysicMaterial slippery;
   [SerializeField] CapsuleCollider collider;
   [SerializeField] float currentGroundAngle;
   [SerializeField] Vector3 groundNormal;
   public float slideStartUpTime;
   public float slideFallOffTime;
   public float slideFallOffVelocity;
   public float slideMovementSpeed;
   

   //******Crystal Destroy Event Variables******* RJ Bourdelais
   [Header("Crystal Destory Event")]
   [SerializeField]
   float cdeForce;
   [SerializeField] float cdeLength;
   bool cdeActive = false;
   Transform etherianInLevel;
   [SerializeField]
   float randomDistance;




   // Use this for initialization
   void Start()
   {
      thisRB = GetComponent<Rigidbody>();
      thisCamera = GameObject.Find("Main Camera").GetComponent<Camera>();

      //Turn off light and glider for temporary animation
      thisLight.SetActive(false);
      lightTransform = thisLight.transform;
      gliderTemp.SetActive(false);

      //Load crystals into scene
      LoadLevel();
   }

   // Update is called once per frame
   // *************************** TOMMY STUFF ****************************
   // *************************** SLIDE FUNCTION ADDED 
   // *************************** RETURNS IF SLIDING

   void FixedUpdate()
   {
      if (cdeActive)
      {
         return;
      }

      GroundCheck();

      GetInput();

      Slide();

      //Temporary camera control
      if (removeControl)
         return;

      Jump();
      MovePlayer();
      ExplodeDirection();
      Glide();

      //Temporary win condition
      if (crystalsOnScene <= 0)
      {
         Debug.Log("WIN");
      }

      // NNNNNNNNNNNNNNNEEEEEEEEEEEEEEEEEEEEEEEWWWWWWWWWWWWWWWWWWWWWWWWWWWWW
      if(!inAir && thisRB.velocity.magnitude > 1 && !sliding)
      {
         if(transform.parent.name == "Terrain")
         {
            Mann_AudioManagerScript.instance.PlaySound("Player_Sand_Run");
         }
         else
         {
            Mann_AudioManagerScript.instance.PlaySound("Player_Rock_Run");
         }
      }
      else
      {
         Mann_AudioManagerScript.instance.StopSound("Player_Sand_Run");
         Mann_AudioManagerScript.instance.StopSound("Player_Rock_Run");
      }
   }

   //Input for controller
   void GetInput()
   {
      //Left stick movment
      xDirection = Input.GetAxis("Horizontal");
      zDirection = Input.GetAxis("Vertical");

      //On press Explode
      if (Input.GetAxisRaw("ExplodeInput") != lastExplodeInput)
      { explodeInput = Input.GetAxisRaw("ExplodeInput"); lastExplodeInput = explodeInput; }
      else
      { explodeInput = 0.0f; }

      //On hold Glide
      glideInput = Input.GetAxisRaw("GlideInput");

      //On press jump
      if (Input.GetAxisRaw("Jump") != lastJumpInput)
      { jumpInput = Input.GetAxisRaw("Jump"); lastJumpInput = jumpInput; }
      else { jumpInput = 0; }

      //Temporary Turn off render for video
      if (Input.GetKey(KeyCode.Return))
      {
         GetComponent<MeshRenderer>().enabled = false; MeshRenderer[] meshs = gameObject.GetComponentsInChildren<MeshRenderer>();
         for (int i = 0; i < meshs.Length; i++)
         {
            meshs[i].enabled = false;
         }
      }
      else
      {
         GetComponent<MeshRenderer>().enabled = true;
         MeshRenderer[] meshs = gameObject.GetComponentsInChildren<MeshRenderer>();
         for (int i = 0; i < meshs.Length; i++)
         {
            meshs[i].enabled = true;
         }
      }

      //Temporary animation for pick up gun
      if (Input.GetKey(KeyCode.P))
      {
         Debug.Log("OUCH");
         clip.SetTrigger("Anim");
      }

      //Temporary Camera remove control from player
      if (Input.GetKey(KeyCode.C))
      {
         removeControl = true;
      }
   }

   //Update player position
   void MovePlayer()
   {
      //Camera forward and right directions
      camForward = cameraTransform.forward;
      camRight = cameraTransform.right;

      camForward.y = 0;
      camRight.y = 0;

      camForward.Normalize();
      camRight.Normalize();

      //Current Y velocity
      Vector3 jumpVelocity = new Vector3(0, thisRB.velocity.y, 0);

      if (gliding)
      {
         //If gliding always have velocity forward of camera and make it faster if the left stick is being pushed forward, slower if backwards
         thisRB.velocity = Vector3.Lerp(thisRB.velocity, thisRB.velocity.normalized + camForward * ((zDirection < 0f) ? basedGlideSpeed : zDirection + incrementedGlideSpeed) * airMovementSpeed + jumpVelocity, Time.deltaTime);
      }
      // ****************************** ADDED "!sliding" TOMMY STUFF ********************************
      else if (!addedForce && !inAir && !sliding)
      {
         //Ground Movement
         thisRB.velocity = camForward * zDirection * playerSpeed + camRight * xDirection * playerSpeed + jumpVelocity;
      }
      else if (addedForce || inAir)
      {
         //Falling velocity
         thisRB.velocity = Vector3.Lerp(thisRB.velocity, thisRB.velocity.normalized + camForward * zDirection * airMovementSpeed + camRight * xDirection * airMovementSpeed, Time.deltaTime);

      }
      // ***************************** TOMMY STUFF *********************************
      else if (sliding && !inAir && !addedForce)
      {
         //thisRB.velocity += camForward * zDirection * slideMovementSpeed + camRight * xDirection * slideMovementSpeed;
         thisRB.AddForce(camForward * zDirection * slideMovementSpeed + camRight * xDirection * slideMovementSpeed, ForceMode.Force);
      }


      //Spooky if statement That clamps movement of the explosion position. If it is too close to zero it will explode up too much
      if (Mathf.Abs(zDirection) > 0.65 || Mathf.Abs(xDirection) > 0.65)
      {
         float zValue = zDirection, xValue = xDirection;
         if (zDirection < -0.5)
         {
            zValue = Mathf.Clamp(zValue * 2f, -0.9f, zValue * 2f);
         }
         else if (zDirection > 0.5)
         {
            zValue = Mathf.Clamp(zValue * 2f, zValue * 2f, 0.9f);
         }
         if (xDirection < -0.5)
         {
            xValue = Mathf.Clamp(xValue * 2f, -0.9f, xValue * 2f);
         }
         else if (xDirection > 0.5)
         {
            xValue = Mathf.Clamp(xValue * 2f, xValue * 2f, 0.9f);
         }

         //Clamp again if it is both greater the than value, this fixes it still going to high if it is too close to the character
         if (Mathf.Abs(xValue) > 0.65f && Mathf.Abs(zValue) > 0.65f)
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

         //Set the position of the expolsion to the new clamped values
         sideExplosion.position = camForward * (-zValue) + camRight * (-xValue) + transform.position;
         sideExplosion.position = new Vector3(sideExplosion.position.x, transform.position.y - 1f, sideExplosion.position.z);

      }
      else
      {
         //Put it below the player directly otherwise
         sideExplosion.position = new Vector3(transform.position.x, transform.position.y - 1.5f, transform.position.z);

      }

      Vector3 targetDirection;

      //Where the player should look
      targetDirection = transform.position + (camForward * 5);

      //If the player isn't moving keep the rotation as it was
      if (thisRB.velocity.magnitude > 0.5f)
      {
         //Update new rotation if player is moving
         transform.LookAt(targetDirection);
         transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
      }

      //If the players velocity is grater than 10 increase the FOV or decrease it otherwise
      thisCamera.fieldOfView = (thisRB.velocity.magnitude > 10) ? Mathf.Lerp(thisCamera.fieldOfView, thisCamera.fieldOfView + FOVSpeed, Time.deltaTime) : Mathf.Lerp(thisCamera.fieldOfView, thisCamera.fieldOfView - FOVSpeed, Time.deltaTime);
      thisCamera.fieldOfView = Mathf.Clamp(thisCamera.fieldOfView, minFOV, maxFOV);

      //If the players position is too low respawn at 1 1 1 TEMPORARY
      if (transform.position.y < -5f)
      {
         transform.position = new Vector3(1, 1, 1);
         thisRB.velocity = Vector3.zero;
      }
   }

   //Check what is under the player
   void GroundCheck()
   {
      hitCollide = null;
      hitCollide = Physics.OverlapSphere(groundCheck.position, 0.5f, groundLayer);
      //If this collision is greater than 0 it is on a surface
      if (hitCollide.Length > 0)
      {
         inAir = false;
         if (!addedForce)
         {
            currentShot = 0;
         }
      }
      else
      {
         inAir = true;
      }

      //check if the player should attach itself to the ground or beast
      for (int i = 0; i < hitCollide.Length && !inAir; i++)
      {
         if ((groundLayer & 1 << hitCollide[i].gameObject.layer) != 0)
         {
            scale = transform.localScale;
            transform.parent = hitCollide[i].gameObject.transform;
            ResetScale();
            return;
         }
      }
      // *************************** TOMMY STUFF ****************************

   }

   // *************************** TOMMY STUFF ****************************
   void Slide()
   {
      RaycastHit hit;
      // Does the ray intersect any objects excluding the player layer
      if (Physics.Raycast(transform.position + new Vector3(0, 2, 0), Vector3.down, out hit, Mathf.Infinity, groundLayer))
      {
         groundNormal = hit.normal;
         currentGroundAngle = Vector3.Angle(Vector3.up, hit.normal);
         
            if (currentGroundAngle > slidingAngle && sliding == false)
            {
               collider.material = slippery;
               sliding = true;
               StartCoroutine(SlideTime(slideStartUpTime));
            }
            else if (sliding && currentGroundAngle < slidingAngle && thisRB.velocity.magnitude < slideFallOffVelocity && startingToSlide == false)
            {

               sliding = false;
               collider.material = null;
            }
         

         if(sliding && currentGroundAngle <= slidingFallOffAngle)
         {
            StartCoroutine(SlideFallOffTime(slideFallOffTime));
         }
      }

   }

   //When the input is received for explode propell with motion
   void ExplodeDirection()
   {
      //Dont do anything if the player is out of shots
      if (currentShot >= maxShots)
      {
         return;
      }

      //Create explosion where the sidePosition is
      if (explodeInput == 1.0 && !addedForce && !gliding)
      {
         thisRB.velocity = Vector3.zero;
         thisRB.AddExplosionForce(sideForce, sideExplosion.position, explodeRadius, upwardsSideForce, ForceMode.Impulse);
         originalExplodePosition = sideExplosion.position;
         currentShot++;
         StartCoroutine(ExplodeTime(explodeTime));

         // ***************************** TOMMMYMMYMYMYMYMYM **************************
         Mann_AudioManagerScript.instance.PlaySound("GunLance2");

      }

   }

   //Add jump force to velocity when input pressed
   void Jump()
   {
      if (jumpInput != 0.0f && !inAir)
      {
         thisRB.AddForce(new Vector3(0, jumpForce, 0));
      }
   }

   //Glide while button held
   void Glide()
   {

      //Always in direction of camera forward, left stick controlls left and right movement
      if (glideInput == 1.0f && !addedForce && inAir)
      {
         //cut the falling velocity by fallSpeedGliding
         thisRB.velocity = new Vector3(thisRB.velocity.x, thisRB.velocity.y / fallSpeedGliding, thisRB.velocity.z);

         gliding = true;
      }
      else
      {
         gliding = false;
      }

      //Render Glider
      if (gliding)
      {
         gliderTemp.SetActive(true);
      }
      else
      {
         gliderTemp.SetActive(false);
      }
   }

   //How long does the force get added
   IEnumerator ExplodeTime(float time)
   {
      addedForce = true;

      //Set flash animation for explosion
      thisLight.SetActive(true);
      lightTransform.position = transform.position;

      //Check if the explosion hits any crystals nearby
      hitCollide = null;
      hitCollide = Physics.OverlapSphere(originalExplodePosition, explodeRadius, crystalLayer);

      //Destroy any crystals hit
      for (int i = 0; i < hitCollide.Length; i++)
      {
         Destroy(hitCollide[i].gameObject);
         crystalsOnScene--;
         StartCoroutine(CrystalDestroyEvent(cdeLength));
      }
      yield return new WaitForSecondsRealtime(time);
      addedForce = false;
   }

   IEnumerator SlideTime(float time)
   {
      startingToSlide = true;
      yield return new WaitForSecondsRealtime(time);
      startingToSlide = false;

   }

   IEnumerator SlideFallOffTime(float time)
   {
      stoppingToSlide = true;
      yield return new WaitForSecondsRealtime(time);
      sliding = false;
      collider.material = null;
      stoppingToSlide = false;
   }

   IEnumerator CrystalDestroyEvent(float time)
   {
      cdeActive = true;
      Vector3 randomDir = RandomDirection();
      randomDir.Normalize();
      Vector3 forceDirection = transform.position - (etherianInLevel.position + randomDir * randomDistance);
      //forceDirection.Normalize();
      forceDirection *= cdeForce;
      thisRB.AddForce(forceDirection);

      yield return new WaitForSecondsRealtime(time);
      cdeActive = false;
   }

   //Debugging for where the explosion is 
   private void OnDrawGizmos()
   {
      Gizmos.color = Color.red;
      if (addedForce)
      {
         Gizmos.DrawSphere(originalExplodePosition, explodeRadius);
      }
   }

   //Loads all the crystals on new level
   void LoadLevel()
   {
      GameObject[] crystalList = GameObject.FindGameObjectsWithTag("Crystal");
      crystalsOnScene = crystalList.Length;
      etherianInLevel = GameObject.FindGameObjectWithTag("Beast").transform;

   }

   Vector3 RandomDirection()
   {
      float rand = Random.Range(0, 2 * Mathf.PI);
      float x = Mathf.Sin(rand);
      float y = Mathf.Cos(rand);
      Debug.Log("YEET " + x + " " + y);
      return new Vector3(x, 0, y);

   }

   void ResetScale()
   {
      //var worldMat = transform.worldToLocalMatrix;
      //worldMat.SetColumn(3, new Vector4(0.0f, 0.0f, 0.0f, 1.0f));
      //     transform.localScale = Vector3.one;
      transform.lossyScale.Set(1, 1, 1);
      //transform.localScale = worldMat.MultiplyPoint( SCALE);
      //transform.localScale = new Vector3(transform.parent.parent.lossyScale.x, transform.parent.parent.lossyScale.y, transform.parent.parent.lossyScale.z);
      // transform.localScale = new Vector3(transform.parent.parent.localScale.x, )
      //transform.localScale = 1/transform.parent.lossyScale;

      //transform.localScale = new Vector3(1,1,1);
      //transform.localScale.
   }
}