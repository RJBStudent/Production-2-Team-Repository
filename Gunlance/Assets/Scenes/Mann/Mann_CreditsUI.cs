using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Mann_CreditsUI : MonoBehaviour
{

   RectTransform mRectTransform;
   public float scrollSpeed;

   public float endYPos;

   // Start is called before the first frame update
   void Start()
   {
      mRectTransform = GetComponent<RectTransform>();
   }

   // Update is called once per frame
   void Update()
   {
      
      if(mRectTransform.position.y > endYPos)
      {
            if(Input.GetButtonDown("Pause"))
            {
                SceneManager.LoadScene("Mann_MainMenu");
            }
      }
      else
      {
         transform.Translate(Vector3.up * Time.deltaTime * scrollSpeed);
      }
   }
}
