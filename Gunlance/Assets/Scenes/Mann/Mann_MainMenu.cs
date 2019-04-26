using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Mann_MainMenu : MonoBehaviour
{
    public GameObject startUI;

    public GameObject mainMenuUI;

    public bool transitionedToMenu = false;

	 EventSystem es;

	 [SerializeField] Transform selector;
	 [Range(0, 1)]
	 [SerializeField] float moveTime;

	 // Start is called before the first frame update
	 void Start()
	 {
	     es = GameObject.Find("EventSystem").GetComponent<EventSystem>();
	 }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause") && transitionedToMenu == false)
        {
            TransitionToMenu();
        }

		  if (transitionedToMenu)
		  {
				var moveTo = new Vector3(selector.position.x, es.currentSelectedGameObject.transform.position.y, selector.position.z);
				selector.position = Vector3.Lerp(selector.position, moveTo, moveTime);
		  }
	}

    public void TransitionToMenu()
    {
        transitionedToMenu = true;
        startUI.SetActive(false);
        mainMenuUI.SetActive(true);
        es.SetSelectedGameObject(null);
        es.SetSelectedGameObject(es.firstSelectedGameObject);
    }

    public void NewGame()
    {
        SceneManager.LoadScene("Roberge_SandboxV2");
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
