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
    public AnimationCurve moveCurve;
    [Range(0,20)]
    public float levelLength;
    [Range(0, 1)]
    public float randomnessStrength;
    public float randomnessMaxSpeed = 0.1f;
    public float sliderMoveSpeed=1;
    public float dampingTime = 0.25f;
    public ControlType controlType = ControlType.Tap;
    public Sprite gameplayBackground, winBackground, loseBackground;
    public RuntimeAnimatorController dadAnimationController;
    public projectile projectile;
    public float projectileStrenght = 0.5f;
    public List<ProjectileData> projectileData;


    public float GetSpeedAdjustment(float completionPercentage)
    {
        return moveCurve.Evaluate(completionPercentage) + Random.Range(-randomnessStrength, randomnessStrength);
    }

}

[System.Serializable]
public class ProjectileData
    {
    public bool isLeft = true;
    public float launchTime;
    public bool launched = false;

}