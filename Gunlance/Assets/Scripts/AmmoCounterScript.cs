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

	 PlayerMovementScript movementScript;

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
    }

    public void Shoot()
    {
		  //flash crystal
		  GameObject flash = Instantiate(crystalFlash, transform);
        Destroy(flash, .45f);
    }
}
