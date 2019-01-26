﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour {

    public Projectile proj;

    Renderer render;
    Vector3 dir;
    Transform playerTransform;

    // Use this for initialization
    void Start ()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        render = GetComponent<Renderer>();
        render.material = proj.materialType;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        Vector3 dir = new Vector3( transform.position.x + Mathf.Cos(Mathf.Deg2Rad * proj.angle), transform.position.y, transform.position.z + Mathf.Sin(Mathf.Deg2Rad * proj.angle) );        
        transform.position = Vector3.Lerp(transform.position, dir, proj.acceleration * Time.deltaTime);
        checkBounds();
		
	}

    void checkBounds()
    {

        if ((transform.position.x < playerTransform.position.x - 10 || transform.position.x > playerTransform.position.x + 10)
            || (transform.position.z < playerTransform.position.z - 20 || transform.position.z > playerTransform.position.z + 100))
        {
            Destroy(gameObject);
        }

    }
}
