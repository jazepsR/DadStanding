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
    public GameObject lockObj;
    public GameObject unlockObj;
    private bool locked = false;
    // Start is called before the first frame update
    void Start()
    {
        locked = levelID> PlayerPrefs.GetInt(GameManager.saveKey, 0);
        lockObj.SetActive(locked);
        unlockObj.SetActive(!locked);
        //GetComponent<Button>().interactable = !locked;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartLevel()
    {
        if (!locked)
        {
            Invoke(nameof(LoadLevel), 0.15f);
        }
    }

    private void LoadLevel()
    {
        GameManager.levelIndex = levelID;
        SceneManager.LoadScene(1);
    }
    public void SetupLvlSelectScreen()
    {
        MainMenuUI.instance.SetLevelSelect(levelName, levelDescription);
    }
}
