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
        StartCoroutine(ShakeScreen());
	}
	
	// Update is called once per frame
	IEnumerator ShakeScreen() {

        yield return new WaitForSeconds(3);
        cam.DOShakePosition(duration,strength,vibrato,randomness);

	}
}
