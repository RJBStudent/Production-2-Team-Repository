using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Beast")]
public class BeastObject : ScriptableObject
{

    [SerializeField] public GameObject beastObject;
    [SerializeField] public Vector3[] MovementNodes; 

}
