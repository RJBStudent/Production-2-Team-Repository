using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Gruner_AmmoCounterScript : MonoBehaviour
{
	Material cubeMat;
	[SerializeField] Gradient crystalColors;
	[SerializeField] float rotSpeed;
	[SerializeField] GameObject crystalFlash;

	Gruner_PlayerMovement movementScript;

	private void Start()
	{
		cubeMat = GetComponent<Renderer>().material;

		movementScript = GetComponentInParent<Gruner_PlayerMovement>();
	}

	void Update()
	{
		CrystalUpdate();
	}

	void CrystalUpdate()
	{
		//update crystal color to charge level
		cubeMat.color = crystalColors.Evaluate(movementScript.charge / movementScript.maxShots);
		cubeMat.SetColor("_EmissionColor", crystalColors.Evaluate(movementScript.charge / movementScript.maxShots));

		transform.Rotate(0, -movementScript.charge * rotSpeed, 0);
	}

	public void Shoot()
	{
		//flash crystal
		GameObject flash = Instantiate(crystalFlash);
		flash.transform.position = transform.position;
		Destroy(flash, .45f);
	}
}
