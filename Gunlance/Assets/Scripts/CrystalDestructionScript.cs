using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalDestructionScript : MonoBehaviour
{
    [SerializeField] GameObject crystalShards;
    [SerializeField] GameObject particleBurst;

    public void OnExplode()
    {
        GameObject shards = Instantiate(crystalShards);
    }
}
