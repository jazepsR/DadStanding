using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "JokeData", menuName = "ScriptableObjects/Create Joke", order = 2)]
public class JokeScriptable : ScriptableObject
{
    [TextArea(4,10)]
    public string setupText;
    public AudioClip setupAudio;
    [TextArea(4, 10)]
    public string punchlineText;
    public AudioClip punchlineAudio;
    [HideInInspector] public int timesTold = 0;

}
