using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demonstration_UI_Script : MonoBehaviour
{
	Material cubeMat;
	[SerializeField] Gradient crystalColors;
	[SerializeField] float rotSpeed;
	[SerializeField] GameObject crystalFlash;

	float charge;
	int shots;
	[SerializeField] float chargeRate;
	bool pause;

	PlayerMovementScript movementScript;

	private void Start()
	{
		cubeMat = GetComponent<Renderer>().material;

		movementScript = GetComponentInParent<PlayerMovementScript>();
	}

	void Update()
	{
		if (!pause)
		{
			charge += chargeRate;

			charge = Mathf.Clamp(charge, 0, 3);
		}

		CrystalUpdate();
	}

	void CrystalUpdate()
	{
		//update crystal color to charge level
		cubeMat.color = crystalColors.Evaluate(charge / 3);
		cubeMat.SetColor("_EmissionColor", crystalColors.Evaluate(charge / 3));

		transform.Rotate(0, -charge * rotSpeed, 0);

		shots = Mathf.FloorToInt(charge);
	}

	public void Shoot()
	{
		if (shots > 0)
		{
			charge--;

			//flash crystal
			GameObject flash = Instantiate(crystalFlash);
			flash.transform.position = transform.position;
			Destroy(flash, .45f);

			pause = true;

			Invoke("UnPause", 2.5f);
		}
	}

	void UnPause()
	{
		pause = false;
	}
}
