using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Beast")]
public class BeastObject : ScriptableObject
{
    
    [SerializeField] public Vector3[] movementNodes;
    [SerializeField] public float nodeRadius;
    [SerializeField] public float nodeRotateRadius;
    [SerializeField] public float speed;
    [SerializeField] public float rotationSpeed;
}
