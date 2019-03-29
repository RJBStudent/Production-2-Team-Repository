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
    [SerializeField] Light crystalLight;

    [SerializeField] int maxAmmo;
    [SerializeField] int currentAmmo;
    [SerializeField] float charge;
    [SerializeField] float rechargeRate;
    [SerializeField] float chargePause;
    bool pause = false;

    private void Start()
    {
        cubeMat = GetComponent<Renderer>().material;

        charge = currentAmmo = 3;
    }

    void Update()
    {
        CrystalUpdate();

        if (!pause)
            Recharge();
    }

    void CrystalUpdate()
    {
        //update crystal color to charge level
        cubeMat.color = crystalColors.Evaluate(charge / maxAmmo);
        cubeMat.SetColor("_EmissionColor", crystalColors.Evaluate(charge / maxAmmo));

        //update crystal light intensity to charge level
        crystalLight.intensity = (charge / maxAmmo);

        transform.Rotate(0, -charge * rotSpeed, 0);
    }

    void Recharge()
    {
        if (charge < maxAmmo)
            charge += rechargeRate;

        //make sure charge doesn't "overcharge"
        charge = Mathf.Clamp(charge, 0, maxAmmo);

        currentAmmo = Mathf.FloorToInt(charge);
    }

    void UnPause()
    {
        pause = false;
    }

    public void Shoot()
    {
        if (currentAmmo > 0)
        {
            currentAmmo--;
            charge--;

            //flash crystal
            GameObject flash = Instantiate(crystalFlash);
            flash.transform.position = transform.position;
            Destroy(flash, .45f);

            pause = true;
            Invoke("UnPause", chargePause);
        }
    }
}
