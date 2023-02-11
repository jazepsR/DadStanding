using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public GameObject tutorial;
    public GameObject clickToStart;

    private void Start()
    {
        tutorial.SetActive(false);
        clickToStart.SetActive(true);
    }
    private void Update()
    {
     
        if(Keyboard.current.escapeKey.wasPressedThisFrame )
        {
            Application.Quit();
        }else if (Keyboard.current.anyKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame)
            ShowTutorial();
    }
    public void ShowTutorial()
    {
        tutorial.SetActive(true);
        clickToStart.SetActive(false);
    }
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
