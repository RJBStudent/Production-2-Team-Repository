using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "BulletPattern")]
public class BulletPatternObject : ScriptableObject
{

    [SerializeField] public Vector3[] SpawnPositionRelativeToBoss;
    [SerializeField] public int[] duplicatePositions;
    [SerializeField] public int whichPosition;
    [SerializeField] public int repeatPosition;
    [SerializeField] public float positionInterval;
    [SerializeField] public bool traverseBackwards;


    [SerializeField] public float spawnInterval;

    [SerializeField] public bool isBossSpawn;

    [SerializeField] public float angleInterval;
    [SerializeField] public int repeatAngle;

    [SerializeField] public float patternLength;

    [SerializeField] public Projectile[] projectileTypes;
    [SerializeField] public int whichBullet;
    [SerializeField] public float whenToSwitchBulletType;
    [SerializeField] public int repeatSwitchType;

    [SerializeField] public GameObject bulletPrefab;
   

    float currentTime = 0.0f;

    int switchPosCount = 0;
    int switchAngleCount = 0;
    int switchType = 0;
    int currentIncrement = 1;

    public void FireBullet(Vector3 bossPosition)
    {
        if(currentTime > spawnInterval)
        {
            if(currentTime > whenToSwitchBulletType && switchType >= repeatSwitchType)
            {
               
                whichBullet = (whichBullet + 1) % projectileTypes.Length;                

                switchType = 0;
            }

            if(currentTime > positionInterval && switchPosCount >= repeatPosition)
            {

                //FIGURE OUT A WAY TO INCREMENT BACKWARDS FOR INDIVIDUALS
                for (int i = 0; i < duplicatePositions.Length; i++)
                {
                    duplicatePositions[i] = (duplicatePositions[i] + currentIncrement) % SpawnPositionRelativeToBoss.Length;
                }

                if (((whichPosition + currentIncrement) % SpawnPositionRelativeToBoss.Length == 0) && traverseBackwards)
                {
                    currentIncrement = -currentIncrement;
                    
                }

                whichPosition = (whichPosition + currentIncrement) % SpawnPositionRelativeToBoss.Length;



                switchPosCount = 0;
            }

            switchType++;
            switchPosCount++;
            if (duplicatePositions.Length > 0)
            {
                for (int i = 0; i < duplicatePositions.Length; i++)
                {
                    GameObject Bullet = (GameObject)Instantiate(bulletPrefab, bossPosition + SpawnPositionRelativeToBoss[duplicatePositions[i]], Quaternion.identity);
                    bulletPrefab.GetComponent<ProjectileScript>().proj = projectileTypes[whichBullet];
                }
            }
            else
            {
                GameObject Bullet = (GameObject)Instantiate(bulletPrefab, bossPosition + SpawnPositionRelativeToBoss[whichPosition], Quaternion.identity);
                bulletPrefab.GetComponent<ProjectileScript>().proj = projectileTypes[whichBullet];
            }
                
            currentTime = 0.0f;
        }
        currentTime += Time.deltaTime;
    }
}
