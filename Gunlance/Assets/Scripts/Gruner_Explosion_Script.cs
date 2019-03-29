using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Gruner_Explosion_Script : MonoBehaviour
{
    [SerializeField] float destroyWait;
    [SerializeField] Light explodeLight;

    void Start()
    {
        Destroy(gameObject, destroyWait);
        explodeLight.DOIntensity(0, destroyWait - .1f);
    }
}
