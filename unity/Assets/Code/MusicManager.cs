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

    public AudioSource[] sources;

    public SongObject[] songs;
    private IEnumerator songEnumerator;

    void Start()
    {
        GoogleAnalytics.SafeLogScreen("game-start");

        UIEventListener.Get(playButton).onClick += OnButtonPlay;
        UIEventListener.Get(demoButton).onClick += OnButtonDemo;
        EventManager.Highlight += OnHighlightOn;
        EventManager.NoHighlight += OnHighlightOff;
        EventManager.Pause += PauseInstruments;
        EventManager.Resume += PlayInstruments;

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
//        foreach (var source in sources)
//        {
//                source.loop = false;
//        }
        songEnumerator = EnumerateSongs().GetEnumerator();
        SwitchSong(false);
    }

    void OnDestroy()
    {
        NGUIHelper.RemoveClickEventListener(playButton, OnButtonPlay);
        NGUIHelper.RemoveClickEventListener(demoButton, OnButtonDemo);
        EventManager.Highlight -= OnHighlightOn;
        EventManager.NoHighlight -= OnHighlightOff;
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

        string gaPrefix;
        if (demo)
        {
            gaPrefix = "song-demo-";
        }
        else
        {
            gaPrefix = "song-switch-";
        }
        GoogleAnalytics.SafeLogScreen(gaPrefix + newSong.songId);

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

        PlayInstruments();

        EventManager.TriggerSongSwitch(newSong);

        if (demo && newSong.demo != null)
        {
            EventManager.TriggerDemoSongStart(newSong.demo.instruments, newSong.demo.stageCointainers);
        }
    }

    void PauseInstruments()
    {
        foreach (AudioSource source in sources)
        {
            source.Pause();
        }
    }

    void PlayInstruments()
    {
        foreach (AudioSource source in sources)
        {
            source.Play();
        }
    }
}
