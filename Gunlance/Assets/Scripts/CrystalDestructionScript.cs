using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalDestructionScript : MonoBehaviour
{
    [SerializeField] GameObject crystalShards;
    [SerializeField] GameObject particleBurst;
	 [SerializeField] float shardLifetime;

	 [SerializeField] float exPower;
	 [SerializeField] float exRadius;
	 Vector3 exPos;

	 public void OnExplode()
    {
        GameObject shards = Instantiate(crystalShards);
		  shards.transform.position = transform.position;
		  shards.transform.rotation = transform.rotation;
		  GameObject burst = Instantiate(particleBurst);
		  burst.transform.position = transform.position;
		  burst.transform.rotation = transform.rotation;

		  foreach (Transform child in crystalShards.transform)
		  {
				Rigidbody rb = child.gameObject.GetComponent<Rigidbody>();

				if (rb != null)
				{

					rb.AddExplosionForce(exPower, transform.position, exRadius, 1, ForceMode.VelocityChange);

					rb.useGravity = true;
				}
		  }

		  Destroy(gameObject);
		  Destroy(burst, .4f);
		  Destroy(shards, shardLifetime);
    }
}
