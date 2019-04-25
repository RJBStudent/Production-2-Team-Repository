using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartControls : MonoBehaviour
{

	public bool gameStarted = false;
    // Start is called before the first frame update
    void Start()
    {
		Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameStarted)
		{
			Time.timeScale = 1;
			this.gameObject.SetActive(false);
		}
		else
		{

		}

		if(Input.GetButtonDown("Jump"))
		{
			gameStarted = true;
		}
	}
}
