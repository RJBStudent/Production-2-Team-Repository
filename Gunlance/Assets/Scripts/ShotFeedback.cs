using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShotFeedback : MonoBehaviour
{
    [SerializeField] GameObject smokeFX;
    [SerializeField] float smokeDestroy;

    [Header("Screen Shake Values")]
    [Range(0, 1)]
    [SerializeField] float duration;
    [Range(0, 5)]
    [SerializeField] float strength;
    [Range(4, 15)]
    [SerializeField] int vibrato;
    [Range(0, 90)]
    [SerializeField] float randomness;

    public void Explode()
    {
        Camera.main.DOShakePosition(duration, strength, vibrato, randomness, true);

        GameObject smoke = Instantiate(smokeFX, gameObject.transform);
        Destroy(smoke, smokeDestroy);
    }
}
