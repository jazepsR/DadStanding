using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;
public class MainMenuUI : MonoBehaviour
{
    public GameObject tutorial;
    public GameObject fade;
    public GameObject clickToStart;
    public GameObject mainMenu;
    public GameObject levelMenu;
    public TMP_Text levelSelectTitle;
    public TMP_Text levelSelectDescription;
    public static MainMenuUI instance;
    public static bool openLevel = false;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        mainMenu.SetActive(true);
        fade.SetActive(false);
        levelMenu.SetActive(false);
        tutorial.SetActive(false);
        clickToStart.SetActive(true);
        if(openLevel)
        {
            mainMenu.SetActive(false);
            levelMenu.SetActive(true);
        }
    }
    private void Update()
    {
     
        if(Keyboard.current.escapeKey.wasPressedThisFrame )
        {
            Application.Quit();
        }else if (Keyboard.current.anyKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame)
            ShowTutorial();
    }

    public void SetLevelSelect(string title, string description)
    {
        levelSelectTitle.text = title;
        levelSelectDescription.text = description;
    }

    public void ClearLevelSelect()
    {
        levelSelectDescription.text = "";
        levelSelectTitle.text = "";
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void ShowTutorial()
    {
        tutorial.SetActive(true);
        clickToStart.SetActive(false);
    }
}
