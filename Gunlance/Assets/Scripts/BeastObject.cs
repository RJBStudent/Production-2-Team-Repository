using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Beast")]
public class BeastObject : ScriptableObject
{    
    public Vector3[] movementNodes;    //Each position the Beast can move to 
    [SerializeField] public float nodeRadius;   //When the beast can move to the next node
    [SerializeField] public float nodeRotateRadius;     // When the beast should start rotation towards the next node
    [SerializeField] public float speed;    // The speed it moves
    [SerializeField] public float rotationSpeed;    //The speed of the rotation
    [SerializeField] public EtherianEvent[] etheriansEvents;
}


[System.Serializable]
public struct EtherianEvent
{
    [SerializeField]
    public int nodeLocation;
    [SerializeField]
    public float lengthOfEvent;
    

};