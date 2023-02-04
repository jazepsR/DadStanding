using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public static UiManager instance;
    [Header("win menu")]
    public GameObject winMenu;


    [Header("lose menu")]
    public GameObject loseMenu;
    public TMP_Text loseTime;

    [Header("start menu")]
    public GameObject startMenu;


    [Header("gameplay menu")]
    public GameObject gameplayMenu;
   
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    public void SetupUI()
    {
        DisableUI();
        switch(GameManager.gameState)
        {
            case GameState.Starting:
                startMenu.SetActive(true);
                break;
            case GameState.Playing:
                gameplayMenu.SetActive(true);

                break;
            case GameState.Win:
                winMenu.SetActive(true);   
                break;
            case GameState.Fail:
                StartCoroutine(EnableLoseMenu());
                break;
        }
    }
    private IEnumerator EnableLoseMenu()
    {
        yield return new WaitForSecondsRealtime(1);
        loseMenu.SetActive(true);
        loseTime.text = "ONLY NEEDED " + GameManager.instance.GetLevelTimeString() + " MORE SECONDS";
    }

    void DisableUI()
    {
        gameplayMenu.SetActive(false);
        winMenu.SetActive(false);
        loseMenu.SetActive(false);
        startMenu.SetActive(false);
    }

    public void ResetLevel()
    {
        GameManager.instance.ResetLevel();
    }
}
