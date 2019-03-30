using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mann_DayNight : MonoBehaviour
{
   // Use this for initialization
   void Start()
   {

   }

   // Update is called once per frame
   void Update()
   {

      //Rotate Sun with Speed
      transform.RotateAround(Vector3.zero, Vector3.right, 5f * Time.deltaTime);
      transform.LookAt(Vector3.zero);

   }
}
