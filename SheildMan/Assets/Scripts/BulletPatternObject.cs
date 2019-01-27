using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "BulletPattern")]
public class BulletPatternObject : ScriptableObject
{

    [SerializeField] public Vector3[] SpawnPositionRelativeToBoss;
    [SerializeField] private int[] duplicatePositions;
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
    float switchPosTime = 0.0f;
    float switchTypeTime = 0.0f;

    int switchPosCount = 0;
    int switchAngleCount = 0;
    int switchType = 0;
    int currentIncrement = 1;
    int[] currentIncrementList;
    int[] copiedDuplicateArray;

    public void Setup()
    {
        currentIncrementList = new int[duplicatePositions.Length];
        copiedDuplicateArray = new int[duplicatePositions.Length];
        for (int i = 0; i < currentIncrementList.Length; i++)

        {
            currentIncrementList[i] = 1;

            copiedDuplicateArray[i] = duplicatePositions[i];
        }
    }


    public void FireBullet(Vector3 bossPosition)
    {
        if(currentTime > spawnInterval)
        {
            if(switchTypeTime > whenToSwitchBulletType && switchType >= repeatSwitchType)
            {
               
                whichBullet = (whichBullet + 1) % projectileTypes.Length;                

                switchType = 0;

                switchTypeTime = 0.0f;
            }

            if(switchPosTime > positionInterval && switchPosCount >= repeatPosition)
            {

                //FIGURE OUT A WAY TO INCREMENT BACKWARDS FOR INDIVIDUALS
                for (int i = 0; i < copiedDuplicateArray.Length; i++)
                {

                    if (((copiedDuplicateArray[i] + currentIncrementList[i]) % SpawnPositionRelativeToBoss.Length == 0) && traverseBackwards)
                    {
                        currentIncrementList[i] = -currentIncrementList[i];

                    }
                    copiedDuplicateArray[i] = (copiedDuplicateArray[i] + currentIncrementList[i]) % SpawnPositionRelativeToBoss.Length;
                    
                }

                if (((whichPosition + currentIncrement) % SpawnPositionRelativeToBoss.Length == 0) && traverseBackwards)
                {
                    currentIncrement = -currentIncrement;
                    
                }

                whichPosition = (whichPosition + currentIncrement) % SpawnPositionRelativeToBoss.Length;



                switchPosCount = 0;
                switchPosTime = 0.0f;
            }


            if (copiedDuplicateArray.Length > 0)
            {
                for (int i = 0; i < copiedDuplicateArray.Length; i++)
                {
                    GameObject Bullet = (GameObject)Instantiate(bulletPrefab, bossPosition + SpawnPositionRelativeToBoss[copiedDuplicateArray[i]], Quaternion.identity);
                    bulletPrefab.GetComponent<ProjectileScript>().proj = projectileTypes[whichBullet];

                    switchType++;
                    switchPosCount++;
                }
            }
            else
            {
                GameObject Bullet = (GameObject)Instantiate(bulletPrefab, bossPosition + SpawnPositionRelativeToBoss[whichPosition], Quaternion.identity);
                bulletPrefab.GetComponent<ProjectileScript>().proj = projectileTypes[whichBullet];

                switchType++;
                switchPosCount++;
            }
                
            currentTime = 0.0f;
        }
        currentTime += Time.deltaTime;
        switchPosTime += Time.deltaTime;
        switchTypeTime += Time.deltaTime;
    }
}
