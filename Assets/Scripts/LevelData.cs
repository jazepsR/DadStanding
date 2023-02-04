using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public enum ControlType { Tap, Hold}
[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/Create Level Data", order = 1)]
public class LevelData : ScriptableObject
{
    public string levelName;
    public AnimationCurve moveCurve;
    [Range(0,600)]
    public float levelLength;
    [Range(0, 1)]
    public float randomnessStrength;
    public ControlType controlType = ControlType.Tap;
    public Sprite gameplayBackground, winBackground, loseBackground;


    public float GetSpeedAdjustment(float completionPercentage)
    {
        return moveCurve.Evaluate(completionPercentage) + Random.Range(-randomnessStrength, randomnessStrength);
    }

}
