using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static SoundController instance;
    private AudioSource source;
    [SerializeField] private AudioClip asIWasSaying;
    [SerializeField] private AudioClip ouch;
    [SerializeField] private AudioClip oof;
    [SerializeField] private AudioClip badumTss;
    [SerializeField] private AudioClip sigh;
    [SerializeField] private AudioClip sigh2;
    [SerializeField] private AudioClip grumble;
    [SerializeField] private AudioClip badDad;
    [SerializeField] private AudioClip cricket;
    [SerializeField] private AudioClip getHit;
    [SerializeField] private AudioClip getHit2;
    [SerializeField] private AudioClip click;
    [SerializeField] private AudioClip levelStartClick;

    private List<AudioClip> winClips = new List<AudioClip>();
    private void Awake()
    {
        source = GetComponent<AudioSource>();
        instance = this;
        winClips.Add(sigh); winClips.Add(sigh2); winClips.Add(cricket); winClips.Add(grumble);
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
        GameManager.instance.OnPunchline();
        source.PlayOneShot(joke.punchlineAudio);
        yield return new WaitForSecondsRealtime(joke.punchlineAudio.length + 0.2f);
        source.PlayOneShot(badumTss);
        yield return new WaitForSecondsRealtime(0.35f);
        source.PlayOneShot(winClips[Random.Range(0,winClips.Count)]);
    }

    public void PlayLose()
    {
        source.PlayOneShot(oof);
        source.PlayOneShot(badDad);
    }

    public void PlayGetHit()
    {
        source.PlayOneShot(Random.value> 0.5f ? getHit: getHit2);
        source.PlayOneShot(ouch);
    }

    public void Click()
    {
        source.PlayOneShot(click);
    }

    public void LevelStartClick()
    {
        source.PlayOneShot(levelStartClick);
    }
    // Update is called once per frame
    /*public void PlayBadumTss()
     {
         source.PlayOneShot(badumTss);
     }*/
}
