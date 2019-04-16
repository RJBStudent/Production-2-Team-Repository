using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mann_FootAudio : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void setAudioClip(AudioClip clip)
	{
		GetComponent<AudioSource>().clip = clip;
	}

	public void playAudioClip()
	{
		GetComponent<AudioSource>().Play();
	}
}
