using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour {

    [SerializeField] Projectile proj;
    [SerializeField] Transform playerTransform;

    Vector3 dir;

    // Use this for initialization
    void Start ()
    {
	
        
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 dir = new Vector3(transform.position.x+Mathf.Cos(Mathf.Deg2Rad * proj.angle), transform.position.y, transform.position.z+Mathf.Sin(Mathf.Deg2Rad * proj.angle) );
        transform.position = Vector3.Lerp(transform.position, dir * proj.speed, proj.acceleration * Time.deltaTime);
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
