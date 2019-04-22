using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CrystalDestructionScript : MonoBehaviour
{
    [SerializeField] GameObject crystalShards;
    [SerializeField] GameObject particleBurst;
	 [SerializeField] Light exLight;
	 [SerializeField] float shardLifetime;

	 [SerializeField] float exPower;
	 [SerializeField] Transform exPoint;
	 [SerializeField] float exRadius;
	 Vector3 exPos;

	 [SerializeField] float lightFade;

	 public void OnExplode()
    {
        GameObject shards = Instantiate(crystalShards);
		  shards.transform.position = transform.position;
		  shards.transform.rotation = transform.rotation;

		  GameObject burst = Instantiate(particleBurst);
		  burst.transform.position = transform.position;
		  burst.transform.rotation = transform.rotation;

		  Light light = Instantiate(exLight);
		  light.transform.position = exPoint.position;
		  light.DOIntensity(0, lightFade);

		  foreach (Transform child in shards.transform)
		  {
				Rigidbody rb = child.gameObject.GetComponent<Rigidbody>();

				if (rb != null)
				{
					rb.AddExplosionForce(exPower, exPoint.transform.position, exRadius, 0, ForceMode.VelocityChange);
				}
		  }

		  Destroy(gameObject);
		  Destroy(burst, shardLifetime);
		  Destroy(shards, shardLifetime);
		  Destroy(light, lightFade + .2f);
    }
}
