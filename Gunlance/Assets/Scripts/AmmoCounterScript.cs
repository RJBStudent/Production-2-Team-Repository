using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoCounterScript : MonoBehaviour
{
    [SerializeField] Gradient crystalColors;
    Material cubeMat;
    [SerializeField] int maxAmmo;
    int currentAmmo;
    [SerializeField] float charge;
    [SerializeField] float rechargeRate;

    private void Start()
    {
        cubeMat = GetComponent<Renderer>().material;

        charge = currentAmmo = 3;
    }

    void Update()
    {
        Recharge();
    }

    void Recharge()
    {
        if (charge < maxAmmo)
            charge += rechargeRate;

        charge = Mathf.Clamp(charge, 0, maxAmmo);

        cubeMat.color = crystalColors.Evaluate(charge / maxAmmo);

        currentAmmo = Mathf.FloorToInt(charge);
    }

    public void Shoot()
    {
        if (currentAmmo > 0)
        {
            currentAmmo--;
            charge--;
        }
    }
}
