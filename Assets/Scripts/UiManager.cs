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
    public TMP_Text winScore;


    [Header("lose menu")]
    public GameObject loseMenu;
    public TMP_Text loseTime;

    [Header("start menu")]
    public GameObject startMenu;


    [Header("gameplay menu")]
    public GameObject gameplayMenu;
    public TMP_Text scoreText;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }
    public void Update()
    {
        switch (GameManager.gameState)
        {
            case GameState.Starting:
                break;
            case GameState.Playing:
                scoreText.text = string.Format("Score: {0}", GameManager.instance.score);
                break;
            case GameState.Win:

                break;

            case GameState.punchLine:
                break;
            case GameState.Fail:
                break;
        }
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
                winScore.text = string.Format("Score: {0}", GameManager.instance.score);
                break;

            case GameState.punchLine:
                winMenu.SetActive(true);
                StartCoroutine(EnableWinButtons());
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
        yield return new WaitForSecondsRealtime(1);
        winScore.gameObject.SetActive(true);
        winText.SetActive(true);
        yield return new WaitForSecondsRealtime(1.5f);
        winMenuButtons.SetActive(true);
    }

    void DisableUI()
    {
        gameplayMenu.SetActive(false);
        winMenu.SetActive(false);
        loseMenu.SetActive(false);
        startMenu.SetActive(false);
        winMenuButtons.SetActive(false);
        winScore.gameObject.SetActive(false);
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
