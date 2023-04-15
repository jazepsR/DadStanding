using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectButton : MonoBehaviour
{
    public string levelName;
    [TextArea(2,5)]
    public string levelDescription;
    public int levelID;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartLevel()
    {
        GameManager.levelIndex = levelID;
        SceneManager.LoadScene(1);
    }
    public void SetupLvlSelectScreen()
    {
        MainMenuUI.instance.SetLevelSelect(levelName, levelDescription);
    }
}
