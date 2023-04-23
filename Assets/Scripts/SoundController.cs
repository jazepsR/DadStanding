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
    [SerializeField] private AudioClip chuckle;
    [SerializeField] private AudioClip error;
    [SerializeField] private AudioClip woosh;
    [SerializeField] private List<AudioClip> noBalance;
    private AudioSource musicSource;
    private List<AudioClip> winClips = new List<AudioClip>();
    private float musicVolMax =0.60f;
    private float musicVolTarget;
    private float lastNoBalance;
    private float noBalanceTimeout = 1f;
    private void Awake()
    {
        source = GetComponent<AudioSource>();
        instance = this;
        winClips.Add(sigh); winClips.Add(sigh2); winClips.Add(cricket); winClips.Add(grumble);
    }

    private void Start()
    {
        musicSource = GameObject.FindGameObjectWithTag("music").GetComponent<AudioSource>();
        if (musicSource == null)
        {
            musicVolTarget = musicVolMax;
        }
        FadeInMusic();
    }
    public void PlayNoBalance()
    {
        if (Time.time > lastNoBalance + noBalanceTimeout)
        {
            source.PlayOneShot(noBalance[Random.Range(0, noBalance.Count)]);
            lastNoBalance = Time.time;
        }
    }

    public void PlayWoosh()
    {
        source.PlayOneShot(woosh, 0.3f);
    }
    public void FadeInMusic()
    {
        if (musicSource != null)
            musicVolTarget = musicVolMax;
    }

    public void FadeOutMusic()
    {
        if (musicSource != null)
            musicVolTarget = 0;
    }

    private void Update()
    {
        if(musicSource)
        {
            musicSource.volume = Mathf.Lerp(musicSource.volume, musicVolTarget, Time.deltaTime*2.5f);
        }
    }
    // Start is called before the first frame update
    public void PlayJoke(JokeScriptable joke)
    {
        if (joke.setupAudio && joke.punchlineAudio)
            StartCoroutine(PlayJokeCoroutine(joke));
    }
    public void PlayError()
    {
        source.PlayOneShot(error);
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
        source.PlayOneShot(badumTss,1.6f);
        yield return new WaitForSecondsRealtime(0.35f);
        if (GameManager.levelIndex == 2)
            source.PlayOneShot(chuckle);
        else
            source.PlayOneShot(winClips[Random.Range(0,winClips.Count)]);
    }

    public void PlayLose()
    {
        source.PlayOneShot(oof);
        source.PlayOneShot(badDad);
        FadeOutMusic();
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
        source.PlayOneShot(levelStartClick, 0.75f);
    }
    // Update is called once per frame
    /*public void PlayBadumTss()
     {
         source.PlayOneShot(badumTss);
     }*/
}
