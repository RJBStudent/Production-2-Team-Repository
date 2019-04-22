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

	 public void Explode()
    {
        mainCamera.DOShakePosition(duration, strength, vibrato, randomness, true);

        GameObject shot = Instantiate(lanceShot, shotPos);
		  Destroy(shot, shotDestroy);

		  //sand kick-up
		  RaycastHit hit;
		  if (Physics.Raycast(shotPos.position, shotPos.TransformDirection(Vector3.forward), out hit, kickUpDist, ground))
		  {
				ParticleSystem dust = Instantiate(dustKickUp);
				dust.transform.position = hit.point;

				//get dust kickUp emission shape and set radius relative to hit distance of the ray
				var dustDonut = dust.shape;
				dustDonut.radius += kickUpRadScale * (Mathf.Clamp(hit.distance - closeDist, 0, kickUpDist) / kickUpDist);

				var dustVel = dust.velocityOverLifetime; //get velocity over lifetime module of dust kickUp
				var radialVel = maxVel - (kickUpVelScale * (Mathf.Clamp(hit.distance - closeDist, 0, kickUpDist) / kickUpDist)); //set velocity relative to hit distance
				dustVel.radial = new ParticleSystem.MinMaxCurve(radialVel);

				Destroy(dust, shotDestroy);
		  }

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
