using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectButton : MonoBehaviour
{
    public string levelName;
    [TextArea(2,5)]
    public string levelDescription;
    public int levelID;
    public Sprite normalIcon, goldIcon;
    public GameObject lockObj;
    public GameObject unlockObj;
    private bool locked = false;
    public LevelData levelData;
    // Start is called before the first frame update
    void Start()
    {
        locked = levelID> PlayerPrefs.GetInt(GameManager.saveKey, 0);
        lockObj.SetActive(locked);
        unlockObj.SetActive(!locked);
        if(levelData)
            levelData.LoadTimesTold();
        unlockObj.GetComponent<Image>().sprite = levelData.jokes.Length == levelData.GetJokesTold() ? goldIcon : normalIcon;
        //GetComponent<Button>().interactable = !locked;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartLevel()
    {
        if (locked)
        {
            SoundController.instance.PlayError();
        }
            else
        {
            Invoke(nameof(LoadLevel), 3f);
            MainMenuUI.instance.fade.SetActive(true);
            SoundController.instance.LevelStartClick();
        }
    }

    private void LoadLevel()
    {
        GameManager.levelIndex = levelID;
        SceneManager.LoadScene(1);
    }
    public void SetupLvlSelectScreen()
    {
        string jokesSeenString = "";
        if(levelData)
            jokesSeenString = "Jokes told " + levelData.GetJokesTold() + "/" + levelData.jokes.Length; 
        MainMenuUI.instance.SetLevelSelect(levelName, levelDescription + "\n\n" + jokesSeenString);
    }
}
