using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveLightScript : MonoBehaviour {

    [SerializeField] float lifeSpan;
    [SerializeField] float dieSpan;
    [SerializeField] float rateOfIntensityPerFrame;
    [SerializeField] float rateOfDecayIntensityPerFrame;

    Light thisLight;

    bool intense = true;

    //gets light component and sets up necessary variables
    void Start()
    {
        thisLight = GetComponent<Light>();
    }

    // Use this for initialization
    void OnEnable ()
    {
        //When enabled start light intensity routine
        StartCoroutine(LifeLine());	
	}

    void FixedUpdate()
    {
        if (intense)
        {
            thisLight.intensity += rateOfIntensityPerFrame;
        }
        else
        {
            thisLight.intensity -= rateOfDecayIntensityPerFrame;
        }
    }

    IEnumerator LifeLine()
    {
        intense = true;
        yield return new WaitForSeconds(lifeSpan);
        StartCoroutine(DieSpeed());
    }

    IEnumerator DieSpeed()
    {
        intense = false;
        yield return new WaitForSeconds(dieSpan);
        thisLight.intensity = 1;
        gameObject.SetActive(false);
    }
}
