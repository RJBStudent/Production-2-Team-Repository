using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mann_MainMenu : MonoBehaviour
{
   public GameObject startUI;

   public GameObject mainMenuUI;

   public bool transitionedToMenu = false;

   // Start is called before the first frame update
   void Start()
   {

   }

   // Update is called once per frame
   void Update()
   {
      if(Input.GetButtonDown("Pause") && transitionedToMenu == false)
      {
         TransitionToMenu();
      }
      
   }
   
   public void TransitionToMenu()
   {
      transitionedToMenu = true;
      startUI.SetActive(false);
      mainMenuUI.SetActive(true);

   }

   public void NewGame()
   {
      Debug.Log("NewGame... ");
   }

   public void Controls()
   {
      Debug.Log("Controls... ");
   }

   public void Quit()
   {
      Application.Quit();
   }
}
