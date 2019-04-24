using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AmmoCounterScript : MonoBehaviour
{
    Material cubeMat;
    [SerializeField] Gradient crystalColors;
    [SerializeField] float rotSpeed;
    [SerializeField] GameObject crystalFlash;
	 [SerializeField] GameObject flashNewCharge;

	 PlayerMovementScript movementScript;
	 float lastCharge;

	private void Start()
    {
		  cubeMat = GetComponent<Renderer>().material;

		  movementScript = GetComponentInParent<PlayerMovementScript>();
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

		  if (lastCharge < movementScript.charge % 1 && movementScript.charge >= movementScript.charge % 1)
		  {
				NewShot();
		  }
		  lastCharge = movementScript.charge;
    }

	 //flash crystal when reaching new shot charge
	 void NewShot()
	 {
		  GameObject flash = Instantiate(flashNewCharge, transform);
		  Destroy(flash, .7f);
	 }

	 //flash crystal on firing
	 public void Shoot()
    {
		  GameObject flash = Instantiate(crystalFlash, transform);
        Destroy(flash, .7f);
    }
}
