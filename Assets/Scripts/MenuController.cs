using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject ControlsMenu;
    public GameObject PauseMenu;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ExitGame()
    {
        SceneManager.LoadScene(0);
    }

    public void DisplayControlsMenu()
    {
        PauseMenu.SetActive(false);
        ControlsMenu.SetActive(true);
    }

    public void DisplayPauseMenu()
    {
        PauseMenu.SetActive(true);
        ControlsMenu.SetActive(false);
    }
}
