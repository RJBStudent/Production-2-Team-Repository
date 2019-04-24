using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CrystalDestructionScript : MonoBehaviour
{
    [SerializeField] GameObject crystalShards;
    [SerializeField] GameObject particleBurst;
	 [SerializeField] Light burstLight;
	 [SerializeField] float lightLifetime;
	 [SerializeField] float lifetime;

	 [SerializeField] Transform exPos;
	 [SerializeField] float exForce;
	 [SerializeField] float exRadius;
	 [Range(0, 1)]
	 [SerializeField] float upMod;

	 public void OnExplode()
    {
		  GameObject shards = Instantiate(crystalShards, transform.position, transform.rotation);
		  Destroy(shards, lifetime);

		  GameObject burst = Instantiate(particleBurst, transform.position, transform.rotation);
		  Destroy(burst, lifetime);

		  Light light = Instantiate(burstLight, transform.position, transform.rotation);
		  light.DOIntensity(0, lightLifetime);
		  Destroy(light.gameObject, lightLifetime);

		  foreach (Transform child in shards.transform)
		  {
				var rb = child.gameObject.GetComponent<Rigidbody>();

				rb.AddExplosionForce(exForce, exPos.position, exRadius, upMod, ForceMode.VelocityChange);
		  }

		  Destroy(gameObject);
    }
}
