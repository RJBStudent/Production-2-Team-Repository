using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using DG.Tweening;


public class Mann_GUImanager : MonoBehaviour
{
    public static bool gamePaused = false;

    public GameObject pauseMenuUI;
	 EventSystem es;

	 [SerializeField] Transform selector;
	 [Range(0, 1)]
	 [SerializeField] float moveTime;

	 // Start is called before the first frame update
	 void Start()
    {

    }
    private void Awake()
    {
        Resume();
		  es = GameObject.Find("EventSystem").GetComponent<EventSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (gamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
                es.SetSelectedGameObject(null);
                es.SetSelectedGameObject(es.firstSelectedGameObject);
            }
        }

		  if (gamePaused)
		  {
				var moveTo = new Vector3(selector.position.x, es.currentSelectedGameObject.transform.position.y, selector.position.z);
				selector.position = Vector3.Lerp(selector.position, moveTo, moveTime);
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
        SceneManager.LoadScene("Mann_MainMenu");
    }
}
