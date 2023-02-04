using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "JokeData", menuName = "ScriptableObjects/Create Joke", order = 2)]
public class JokeScriptable : ScriptableObject
{
    public string setupText;
    public AudioClip setupAudio;
    public string punchlineText;
    public AudioClip punchlineAudio;

}
