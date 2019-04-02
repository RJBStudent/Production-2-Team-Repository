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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause") && transitionedToMenu == false)
        {
            TransitionToMenu();
        }

    }

    public void TransitionToMenu()
    {
        transitionedToMenu = true;
        startUI.SetActive(false);
        mainMenuUI.SetActive(true);
        EventSystem es = GameObject.Find("EventSystem").GetComponent<EventSystem>();
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
