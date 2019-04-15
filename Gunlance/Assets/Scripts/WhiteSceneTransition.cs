using System.Collections;
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
		if(transitioning)
		{
			Color transitionCol = overlay.color;
			transitionCol.a = Mathf.Lerp(transitionCol.a, endOpacity, transitionSpeed);
			overlay.color = transitionCol;
			if(switchScenes && overlay.color.a > endOpacity-endOffSet)
			{
				SceneManager.LoadScene(transitionScene);
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
