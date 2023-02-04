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
                speechBubbleText.text = GameManager.instance.activeJoke.punchlineText;
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
        yield return new WaitForSecondsRealtime(2);
        winText.SetActive(true);
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
