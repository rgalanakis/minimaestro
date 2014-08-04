using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MusicManager : MonoBehaviour
{
    public UILabel songNameLabel;
    public GameObject playButton;
    public GameObject demoButton;

    public AudioSource xylophone;
    public AudioSource violin;
    public AudioSource tuba;
    public AudioSource cymbals;
    public AudioSource harp;
    public AudioSource flute;
    public AudioSource trumpet;
    public AudioSource piano;
    public AudioSource clarinet;
    public AudioSource drums;
    public AudioSource horn;

    private AudioSource[] sources;

    public Dictionary<AudioSource, HighlightManager.InstrumentType> sourceTypes;
    public SongObject[] songs;
    private IEnumerator songEnumerator;

    // In order to coordinate looping between all tracks,
    // we keep track of whether the song is playing or paused,
    // how long its been playing, and when it should re-loop.
    // Each tick of the update loop, we:
    //   - do nothing if the song is paused.
    //   - add the delta from the last update to the time we've played.
    //   - if we have been playing longer than the loop duration,
    //     restart the song (and set the 'played time' back to 0).
    private bool isPlaying;
    private float restartAfter;
    private float timePlayed;
    private float startVolume = 0.01f;
    private float nonSectionButtonVolume = 0.5f; 

    void Start()
    {
        UIEventListener.Get(playButton).onClick += OnButtonPlay;
        UIEventListener.Get(demoButton).onClick += OnButtonDemo;
        EventManager.Highlight += OnHighlightOn;
        EventManager.NoHighlight += OnHighlightOff;
        EventManager.Pause += PauseInstruments;
        EventManager.Resume += PlayInstruments;
        EventManager.TutorialCompleted += OnTutorialCompleted;

        sources = new AudioSource[] {
                xylophone,
                violin,
                tuba,
                cymbals,
                harp,
                flute,
                trumpet,
                piano,
                clarinet,
                drums,
                horn
        };

        // We do not want to loop automatically.
        // See comments earlier in file.
        foreach (var source in sources)
        {
            source.loop = false;
            source.volume = startVolume;
        }
        songEnumerator = EnumerateSongs().GetEnumerator();
        SwitchSong(false);
    }

    void OnDestroy()
    {
        NGUIHelper.RemoveClickEventListener(playButton, OnButtonPlay);
        NGUIHelper.RemoveClickEventListener(demoButton, OnButtonDemo);
        EventManager.Highlight -= OnHighlightOn;
        EventManager.NoHighlight -= OnHighlightOff;
        EventManager.TutorialCompleted -= OnTutorialCompleted;
    }

    void Update()
    {
        if (!isPlaying)
        {
            return;
        }
        timePlayed += Time.deltaTime;
        if (timePlayed >= restartAfter)
        {
            timePlayed = 0.0f;
            EventManager.TriggerSongCompleted();
            PlayInstruments();
        }
    }

    private IEnumerable<SongObject> EnumerateSongs()
    {
        while (true)
        {
            foreach (var song in songs)
            {
                yield return song;
            }
        }
    }

    public void OnTutorialCompleted()
    {
        foreach (var source in sources)
        {
            if (source.volume != 1)
            {
                source.volume = 0;
            }
        }
    }
    private SongObject NextSong()
    {
        songEnumerator.MoveNext();
        return (SongObject)songEnumerator.Current;
    }

    void OnButtonPlay(GameObject go)
    {
        SwitchSong(false);
    }

    void OnButtonDemo(GameObject go)
    {
        SwitchSong(true);
    }

    void SwitchSong(bool demo)
    {
        SwitchSong(NextSong(), demo);
    }

    void OnHighlightOn(HighlightManager.InstrumentType instrumentType)
    {   
        playButton.collider.enabled = false;
        demoButton.collider.enabled = false;
    }

    void OnHighlightOff()
    {
        playButton.collider.enabled = true;
        demoButton.collider.enabled = true;
    }

    void DisplayAndFadeSongName(string songName)
    {
        TweenScale.Begin(songNameLabel.gameObject, 0.0f, Vector3.zero);
        TweenScale.Begin(songNameLabel.gameObject, 0.5f, Vector3.one);
        songNameLabel.text = songName;
        songNameLabel.alpha = 1.0f;
        Invoke("AlphaSongName", 2.5f);
    }
    void AlphaSongName()
    {
        //songNameLabel.effectStyle = UILabel.Effect.None;
        TweenScale.Begin(songNameLabel.gameObject, 0.5f, Vector3.zero);
    }
    void SwitchSong(SongObject newSong, bool demo)
    {
        CancelInvoke();
        DisplayAndFadeSongName(newSong.displayName.Replace("\\n", "\n"));

        xylophone.clip = newSong.xylophone;
        violin.clip = newSong.violin;
        tuba.clip = newSong.tuba;
        cymbals.clip = newSong.cymbals;
        harp.clip = newSong.harp;
        flute.clip = newSong.flute;
        trumpet.clip = newSong.trumpet;
        piano.clip = newSong.piano;
        clarinet.clip = newSong.clarinet;
        drums.clip = newSong.drums;
        horn.clip = newSong.horn;

        restartAfter = newSong.length;
        timePlayed = 0.0f;
        PlayInstruments();

        EventManager.TriggerSongSwitch(newSong);

        if (demo && newSong.demo != null)
        {
            EventManager.TriggerDemoSongStart(newSong.demo.instruments, newSong.demo.stageCointainers);
        }
    }

    void PauseInstruments()
    {
        isPlaying = false;
        foreach (AudioSource source in sources)
        {
            source.Pause();
        }
    }

    void PlayInstruments()
    {
        isPlaying = true;
        foreach (AudioSource source in sources)
        {
            source.Play();
        }
    }
}
