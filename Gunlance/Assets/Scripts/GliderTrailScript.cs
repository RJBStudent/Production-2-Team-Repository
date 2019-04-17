using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GliderTrailScript : MonoBehaviour
{
	[SerializeField] TrailRenderer leftTrail;
	[SerializeField] TrailRenderer rightTrail;

    void Update()
    {
		if (gameObject.activeSelf)
		{
			leftTrail.emitting = true;
			rightTrail.emitting = true;
		}
		else
		{
			leftTrail.emitting = false;
			rightTrail.emitting = false;
		}
    }
}
