﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WhiteSceneTransition : MonoBehaviour
{
	public float endOpacity;
	public float transitionSpeed;
	public bool switchScenes;
	public string transitionScene;
	public float endOffSet;
	public bool mainmenu;
	public AudioSource theme;

	public bool transitioning = false;
	Image overlay;

    // Start is called before the first frame update
    void Start()
    {
		overlay = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
		if(transitioning && Time.timeScale != 0)
		{
			Color transitionCol = overlay.color;
			transitionCol.a = Mathf.Lerp(transitionCol.a, endOpacity, transitionSpeed);
			overlay.color = transitionCol;
			if(mainmenu)
			{
				theme.volume = Mathf.Lerp(theme.volume, 0, transitionSpeed);
			}
			if(switchScenes && overlay.color.a > endOpacity-endOffSet)
			{
				SceneManager.LoadScene(transitionScene);
			}
			if (!switchScenes && overlay.color.a < endOffSet)
			{
				transitioning = false;
			}
		}
    }

	public void startTransition()
	{
		transitioning = true;
	}

	public void setTransitionScene(string sceneName)
	{
		transitionScene = sceneName;
	}
}
