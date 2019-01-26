using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "BossObject")]
public class BossObject : ScriptableObject
{

    [SerializeField] public Vector3 SpawnDistance;
    [SerializeField] public BulletPatternObject[] bulletPattern;
    [SerializeField] public int currentPattern = 0;
	
}
