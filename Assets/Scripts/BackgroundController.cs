using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public SpriteRenderer bg;


    public void Update()
    {
        
    }


    public void SetBackground()
    {
        if (GameManager.instance.activeLevel == null)
            return;
        switch(GameManager.gameState)
        {
            case GameState.Starting:
                bg.sprite = GameManager.instance.activeLevel.gameplayBackground;
                break;
            case GameState.Playing:
                bg.sprite = GameManager.instance.activeLevel.gameplayBackground;
                break;
            case GameState.Fail:
                bg.sprite = GameManager.instance.activeLevel.loseBackground;
                break;
            case GameState.Win:
                bg.sprite = GameManager.instance.activeLevel.winBackground;
                break;
        }

    }
}
