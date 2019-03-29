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
            //Increase intensity if in alive coroutine
            thisLight.intensity += rateOfIntensityPerFrame;
        }
        else
        {
            //decrease intensity in death coroutine
            thisLight.intensity -= rateOfDecayIntensityPerFrame;
        }
    }

    IEnumerator LifeLine()
    {
        //Set boolean for whether it should increment intensity
        intense = true;
        yield return new WaitForSeconds(lifeSpan);
        StartCoroutine(DieSpeed());
    }

    IEnumerator DieSpeed()
    {
        //Set boolean for whether it should decrement intensity
        intense = false;
        yield return new WaitForSeconds(dieSpan);
        thisLight.intensity = 1;
        gameObject.SetActive(false);
    }
}
