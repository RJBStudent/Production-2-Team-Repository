using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mann_AudioManagerScript : MonoBehaviour {

   public static Mann_AudioManagerScript instance;

   AudioSource[] sounds;
   void Awake()
   {
      //Check if instance already exists
      if (instance == null)

         //if not, set instance to this
         instance = this;

      //If instance already exists and it's not this:
      else if (instance != this)

         //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
         Destroy(gameObject);

      //Sets this to not be destroyed when reloading scene
      DontDestroyOnLoad(gameObject);

      sounds = Resources.FindObjectsOfTypeAll<AudioSource>();
   }

   // Use this for initialization
   void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

   public void PlaySound(string clipName)
   {
      for(int i = 0; i < sounds.Length; i++)
      {
         if(sounds[i].clip.name == clipName)
         {
            sounds[i].Play();
         }
      }
   }

}
