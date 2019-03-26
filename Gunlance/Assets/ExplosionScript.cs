using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        transform.SetParent(transform, false);
        StartCoroutine(KillObject());
    }

    // Update is called once per frame
    IEnumerator KillObject()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(this.gameObject);
    }
}
