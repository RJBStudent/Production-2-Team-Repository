using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour {

    [SerializeField] BossObject bossType;
    Transform PlayerPosition;

	// Use this for initialization
	void Start ()
    {
        bossType.bulletPattern[bossType.currentPattern].Setup();
        PlayerPosition = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void Update ()    
    {
        if (Vector3.Magnitude(PlayerPosition.position - transform.position) > 10)
        {
            bossType.bulletPattern[bossType.currentPattern].FireBullet(transform.position);
        }
	}
}
