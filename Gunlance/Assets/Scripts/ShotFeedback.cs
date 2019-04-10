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

    [Header("Smoke")]
    [SerializeField] GameObject smokeFX;
    [SerializeField] float smokeDestroy;

    [Header("Screen Shake")]
    [Range(0, 1)]
    [SerializeField] float duration;
    [Range(0, 5)]
    [SerializeField] float strength;
    [Range(4, 15)]
    [SerializeField] int vibrato;
    [Range(0, 90)]
    [SerializeField] float randomness;

    Camera mainCamera;

	 AmmoCounterScript ammoScript;

	 private void Start()
	 {
		  mainCamera = Camera.main;		  
		  ammoScript = GetComponentInChildren<AmmoCounterScript>();
	 }

	 public void Explode()
    {
        mainCamera.DOShakePosition(duration, strength, vibrato, randomness, true);

        GameObject smoke = Instantiate(smokeFX, gameObject.transform);
        Destroy(smoke, smokeDestroy);

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
