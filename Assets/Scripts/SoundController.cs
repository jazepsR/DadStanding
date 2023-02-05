using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static SoundController instance;
    private AudioSource source;
    [SerializeField] private AudioClip asIWasSaying;
    private void Awake()
    {
        source = GetComponent<AudioSource>();
        instance = this;
    }
    // Start is called before the first frame update
   public void PlayJoke(JokeScriptable joke)
    {
        if (joke.setupAudio && joke.punchlineAudio)
            StartCoroutine(PlayJokeCoroutine(joke));
    }

    public void PlaySetup(JokeScriptable joke)
    {
        if(joke.setupAudio)
            source.PlayOneShot(joke.setupAudio);
    }

    IEnumerator PlayJokeCoroutine(JokeScriptable joke)
    {
        source.PlayOneShot(asIWasSaying);
        yield return new WaitForSecondsRealtime(asIWasSaying.length + 0.2f);
        source.PlayOneShot(joke.setupAudio);
        yield return new WaitForSecondsRealtime(joke.setupAudio.length + 0.2f);
        source.PlayOneShot(joke.punchlineAudio);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
