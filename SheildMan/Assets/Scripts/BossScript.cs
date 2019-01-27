using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour {

    [SerializeField] BossObject bossType;

	// Use this for initialization
	void Start ()
    {
        bossType.bulletPattern[bossType.currentPattern].Setup();
	}
	
	// Update is called once per frame
	void Update ()    
    {
        bossType.bulletPattern[bossType.currentPattern].FireBullet(transform.position);
	}
}
