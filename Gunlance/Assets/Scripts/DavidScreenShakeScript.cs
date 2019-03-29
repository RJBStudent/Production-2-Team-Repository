using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DavidScreenShakeScript : MonoBehaviour {

    Camera cam;
    [SerializeField] float duration;
    [SerializeField] float strength;
    [SerializeField] int vibrato;
    [SerializeField] float randomness;

	// Use this for initialization
	void Start () {
        cam = GetComponent<Camera>();
        
	}
	
	// Update is called once per frame
	public void ScreenShake() {
        cam.DOShakePosition(duration,strength,vibrato,randomness);
        GetComponent<CareyCameraController>().CanShake = false;
        Debug.Log("Screen SHOOK");
	}
}
