using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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
    public Button anotherJokeButton;
    public GameObject winText;
    public GameObject winMenuButtons;
    public GameObject nextLevelButton;
    public GameObject allJokesSeen;
    public TMP_Text winScore;
    public Text nextLevelText;


    [Header("lose menu")]
    public GameObject loseMenu;
    public TMP_Text loseTime;
    public TMP_Text loseScore;
    public Button tryAgainButton;

    [Header("start menu")]
    public GameObject startMenu;


    [Header("gameplay menu")]
    public GameObject gameplayMenu;
    public TMP_Text scoreText;
    public GameObject wind;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        wind.gameObject.SetActive(false);
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

    public void GoToLevelSelect()
    {
        MainMenuUI.openLevel = true;
        SceneManager.LoadScene(0);
    }
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
                wind.gameObject.SetActive(false);
                break;
            case GameState.windGust:
                speechBubble.SetActive(false);
                startMenu.SetActive(false);
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
                //nextLevelButton.SetActive(GameManager.levelIndex+1 < GameManager.instance.levels.Length);
                if(GameManager.levelIndex + 1 < GameManager.instance.levels.Length)
                {
                    nextLevelText.text = "Next Level >";
                }
                else
                {
                    nextLevelText.text = "Finish Game >";
                }
                allJokesSeen.SetActive(GameManager.instance.activeLevel.GetJokesTold() >= GameManager.instance.activeLevel.jokes.Length);
                anotherJokeButton.Select();
                break;

            case GameState.punchLine:
                winMenu.SetActive(true);
                StartCoroutine(EnableWinButtons());
                speechBubblePunchline.SetActive(true);
                speechBubblePunchlineText.text = GameManager.instance.activeJoke.punchlineText;
                break;
            case GameState.Fail:
                tryAgainButton.Select();
                StartCoroutine(EnableLoseMenu());
                speechBubble.SetActive(false);
                loseScore.text = string.Format("Score: {0}", GameManager.instance.score);
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
        if (GameManager.levelIndex + 1 < GameManager.instance.levels.Length)
        {
            GameManager.instance.NextLevel();
        }
        else
        {
            MainMenuUI.openHighScore = true;
            SceneManager.LoadScene(0);
        }
    }
}
