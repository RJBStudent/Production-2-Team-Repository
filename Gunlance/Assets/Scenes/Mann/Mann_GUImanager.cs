using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mann_GUImanager : MonoBehaviour
{
   public static bool gamePaused = false;

   public GameObject pauseMenuUI;

   // Start is called before the first frame update
   void Start()
   {

   }

   // Update is called once per frame
   void Update()
   {
      if(Input.GetButtonDown("Pause"))
      {
         if(gamePaused)
         {
            Resume();
         }
         else
         {
            Pause();
         }
      }
   }

   public void Resume()
   {
      pauseMenuUI.SetActive(false);
      gamePaused = false;
      Time.timeScale = 1;
   }

   public void Pause()
   {
      pauseMenuUI.SetActive(true);
      gamePaused = true;
      Time.timeScale = 0;
   }
   
   public void Options()
   {
      Debug.Log("Options... ");
   }

   public void Quit()
   {
      Debug.Log("Quitting... "); 
   }
}
