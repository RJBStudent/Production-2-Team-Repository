using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DavidScreenShakeScript : MonoBehaviour {

    Camera cam;


	// Use this for initialization
	void Start () {
        cam = GetComponent<Camera>();
        StartCoroutine(ShakeScreen());
	}
	
	// Update is called once per frame
	IEnumerator ShakeScreen() {

        yield return new WaitForSeconds(1);
        cam.DOShakePosition(1,3,10,90);

	}
}
