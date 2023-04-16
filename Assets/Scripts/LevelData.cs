using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public enum ControlType { Tap, Hold}
[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/Create Level Data", order = 1)]
public class LevelData : ScriptableObject
{
    public string levelName;
    public JokeScriptable[] jokes;
   // [HideInInspector]
    public float levelLength;
    [Range(0, 1)]
    public float randomnessMaxSpeed = 0.1f;
    public float sliderMoveSpeed=1;
    public float dampingTime = 0.25f;
    public ControlType controlType = ControlType.Tap;
    public Sprite gameplayBackground, winBackground, loseBackground;
    public RuntimeAnimatorController dadAnimationController;
    public projectile projectile;
    public float projectileStrenght = 0.5f;
    public List<MoveData> moveData;
    public List<ProjectileData> projectileData;


    public float GetSpeedAdjustment(float completionPercentage)
    {
        float currentCompletionPercentage = 0;

        foreach (MoveData moveData in moveData)
        {
            if((currentCompletionPercentage + moveData.time/levelLength)>= completionPercentage)
            {
                return moveData.isLeft ? moveData.strenght/10 : -moveData.strenght/10;
            }
            else
            {
                currentCompletionPercentage += moveData.time / levelLength;
            }
        }



        return 0;// moveCurve.Evaluate(completionPercentage) + Random.Range(-randomnessStrength, randomnessStrength);
    }

    private void OnValidate()
    {
        levelLength = 0;
        foreach(MoveData moveData in moveData)
        {
            levelLength += moveData.time;
        }
    }

}

[System.Serializable]
public class ProjectileData
{
    public bool isLeft = true;
    public float launchTime;
    public bool launched = false;
}

[System.Serializable]
public class MoveData
{
    public bool isLeft;
    public float time = 2;
    public float strenght = 2;
}