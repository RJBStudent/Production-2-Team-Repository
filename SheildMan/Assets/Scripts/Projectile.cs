using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Projectile")]
public class Projectile : ScriptableObject {

    [SerializeField] public float speed;
    [SerializeField] public float acceleration;
    [SerializeField] public float angle;



}
