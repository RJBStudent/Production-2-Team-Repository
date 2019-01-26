using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Projectile")]
public class Projectile : ScriptableObject
{
    
    [SerializeField] public float acceleration;
    [SerializeField] public float angle;
    [SerializeField] public ProjectileType projectileType;
    [SerializeField] public Material materialType;
    [SerializeField] public GameObject bulletGameObject;

    public enum ProjectileType
    {
        RED = 0,
        BLUE = 1,
        NULL = 2
    };

    public void SpawnBullet(Vector3 position)
    {
        GameObject newBullet = (GameObject)Instantiate(bulletGameObject, position, Quaternion.identity);
        ProjectileScript newScript = newBullet.AddComponent<ProjectileScript>();
        

    }

}
