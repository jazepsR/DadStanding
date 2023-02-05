using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public static UiManager instance;

    public GameObject speechBubble;
    public TMP_Text speechBubbleText;

    [Header("win menu")]
    public GameObject winMenu;
    public GameObject winText;
    public GameObject winMenuButtons;


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
                speechBubble.SetActive(true);
                speechBubbleText.text = GameManager.instance.activeJoke.setupText;
                break;
            case GameState.Playing:
                gameplayMenu.SetActive(true);
                speechBubble.SetActive(false);

                break;
            case GameState.Win:
                winMenu.SetActive(true);
                speechBubble.SetActive(true);
                winText.SetActive(false);
                StartCoroutine(EnableWinText());
                speechBubbleText.text = "As I was saying.. \n" +GameManager.instance.activeJoke.setupText;
                break;

            case GameState.punchLine:
                winMenuButtons.SetActive(true);
                break;
            case GameState.Fail:
                StartCoroutine(EnableLoseMenu());
                speechBubble.SetActive(false);
                break;
        }
    }
    private IEnumerator EnableLoseMenu()
    {
        yield return new WaitForSecondsRealtime(1);
        loseMenu.SetActive(true);
        loseTime.text = "ONLY NEEDED " + GameManager.instance.GetLevelTimeString() + " MORE SECONDS";
    }

    private IEnumerator EnableWinText()
    {
        yield return new WaitForSecondsRealtime(4);
        speechBubbleText.text = GameManager.instance.activeJoke.punchlineText;
        winText.SetActive(true);
    }

    void DisableUI()
    {
        gameplayMenu.SetActive(false);
        winMenu.SetActive(false);
        loseMenu.SetActive(false);
        startMenu.SetActive(false);
        winMenuButtons.SetActive(false);
    }

    public void ResetLevel()
    {
        GameManager.instance.ResetLevel();
    }

    public void NextLevel()
    {
        GameManager.instance.NextLevel();
    }
}
