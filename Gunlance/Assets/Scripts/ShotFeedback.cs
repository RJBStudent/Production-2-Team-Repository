using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using XInputDotNetPure;

public class ShotFeedback : MonoBehaviour
{
    [Header("Vibration")]
    [Range(0, 1)]
    [SerializeField] float vibrationHitLength;
    [Range(0, 1)]
    [SerializeField] float vibrationHitStrength;
    [Range(0, 1)]
    [SerializeField] float vibrationFadeLength;
    [Range(0, 1)]
    [SerializeField] float vibrationFadeStrength;
    float vibration; //vibrate amount

	 [Header("Blast")]
	 [SerializeField] Transform shotPos;
	 [SerializeField] GameObject lanceShot;
	 [SerializeField] float shotDestroy;
	 [SerializeField] float kickUpDist;
	 [SerializeField] float closeDist;
	 [SerializeField] float kickUpRadScale;
	 [SerializeField] float kickUpVelScale;
	 [SerializeField] float maxVel;
	 [SerializeField] LayerMask ground;
	 [SerializeField] ParticleSystem dustKickUp;

	 [Header("Light Flash")]
	 [SerializeField] Light burstLight;
	 [SerializeField] float lightLifetime;

    [Header("Screen Shake")]
    [Range(0, 1)]
    [SerializeField] float duration;
    [Range(0, 5)]
    [SerializeField] float strength;
    [Range(4, 15)]
    [SerializeField] int vibrato;
    [Range(0, 90)]
    [SerializeField] float randomness;
    [SerializeField] Camera mainCamera;

	 AmmoCounterScript ammoScript;

	 private void Start()
	 {		  
		  ammoScript = GetComponentInChildren<AmmoCounterScript>();
	 }

	 public void TestingFire()
	 {
		  var dir = (transform.position - transform.parent.position).normalized;
		  var rot = Quaternion.LookRotation(dir);

		  Explode(rot);
	 }

	 public void Explode(Quaternion rot)
    {
        mainCamera.DOShakePosition(duration, strength, vibrato, randomness, true);

        GameObject shot = Instantiate(lanceShot, shotPos);
		  shot.transform.rotation = rot;
		  Destroy(shot, shotDestroy);

		  Light burst = Instantiate(burstLight, shotPos);
		  burst.DOIntensity(0, lightLifetime);
		  Destroy(burst.gameObject, lightLifetime);

           Vector3 offsetPosition = shotPos.position + (shotPos.TransformDirection(Vector3.forward) * kickUpDist);
        
       // Debug.Log("HEc k " + shotPos.TransformPoint(0, 0, 0));
        Debug.Log("HEc k " + offsetPosition + " " + shotPos.position);
		  //sand kick-up
		  RaycastHit hit;
		  //if (Physics.Raycast(shotPos.position, offset, out hit, kickUpDist, ground))
		 if (Physics.Linecast(shotPos.position, offsetPosition, out hit, ground))
		  {
				ParticleSystem dust = Instantiate(dustKickUp);
				dust.transform.position = hit.point;

				//get dust kickUp emission shape and set radius relative to hit distance of the ray
				var dustDonut = dust.shape;
				dustDonut.radius += kickUpRadScale * (Mathf.Clamp(hit.distance - closeDist, 0, kickUpDist) / kickUpDist);

				var dustVel = dust.velocityOverLifetime; //get velocity over lifetime module of dust kickUp
				var radialVel = maxVel - (kickUpVelScale * (Mathf.Clamp(hit.distance - closeDist, 0, kickUpDist) / kickUpDist)); //set velocity relative to hit distance
				dustVel.radial = new ParticleSystem.MinMaxCurve(radialVel);

			   Destroy(dust.gameObject, shotDestroy);
				Debug.Log("kickUp");
		  }

		  if (ammoScript != null)
			   ammoScript.Shoot();

        vibration = vibrationHitStrength;
        Invoke("VibrationFade", vibrationHitLength);
    }

    void VibrationFade()
    {
        vibration = vibrationFadeStrength;
        DOTween.To(() => vibration, x => vibration = x, 0, vibrationFadeLength);
    }

    private void FixedUpdate()
    {
        GamePad.SetVibration(0, vibration, vibration);
    }
}
