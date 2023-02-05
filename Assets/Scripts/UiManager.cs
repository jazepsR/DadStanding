using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public static UiManager instance;

    public GameObject speechBubble;
    public GameObject speechBubblePunchline;
    public GameObject speechBubbleNext;
    public TMP_Text speechBubbleText;
    public TMP_Text speechBubblePunchlineText;

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
                speechBubbleNext.SetActive(true);
                break;
            case GameState.Playing:
                gameplayMenu.SetActive(true);
                speechBubble.SetActive(false);

                break;
            case GameState.Win:
                speechBubbleNext.SetActive(false);
                winMenu.SetActive(true);
                speechBubble.SetActive(true);
                winText.SetActive(false);
                speechBubbleText.text = GameManager.instance.activeJoke.punchlineText;
                speechBubbleText.text = "As I was saying.. \n" +GameManager.instance.activeJoke.setupText;
                break;

            case GameState.punchLine:
                winMenu.SetActive(true);
                StartCoroutine(EnableWinButtons());
                winText.SetActive(true);
                speechBubblePunchline.SetActive(true);
                speechBubblePunchlineText.text = GameManager.instance.activeJoke.punchlineText;
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

    private IEnumerator EnableWinButtons()
    {
        yield return new WaitForSecondsRealtime(2);
        winMenuButtons.SetActive(true);
    }

    void DisableUI()
    {
        gameplayMenu.SetActive(false);
        winMenu.SetActive(false);
        loseMenu.SetActive(false);
        startMenu.SetActive(false);
        winMenuButtons.SetActive(false); 
        speechBubblePunchline.SetActive(false);
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
