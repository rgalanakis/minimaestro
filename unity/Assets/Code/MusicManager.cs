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

    public SongObject[] songs;
    private IEnumerator songEnumerator;

    void Start()
    {

        if (GoogleAnalytics.instance)
        {
            GoogleAnalytics.instance.LogScreen("Game Start");
        }
            

        UIEventListener.Get(playButton).onClick += OnButtonPlay;
        UIEventListener.Get(demoButton).onClick += OnButtonDemo;
        songEnumerator = EnumerateSongs().GetEnumerator();
        SwitchSong(false);
        EventManager.Highlight += OnHighlightOn;
        EventManager.NoHighlight += OnHighlightOff;
        EventManager.Pause += PauseInstruments;
        EventManager.Resume += PlayInstruments;
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
        DisplayAndFadeSongName(newSong.displayName);

        if (GoogleAnalytics.instance)
        {
            if (demo)
            {
                GoogleAnalytics.instance.LogScreen("Demo Song Name " + newSong.displayName);
            }
            else
            {
                GoogleAnalytics.instance.LogScreen("Switch to Song " + newSong.displayName);
            }

        }

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
        xylophone.Pause();
        violin.Pause();
        tuba.Pause();
        cymbals.Pause();
        harp.Pause();
        flute.Pause();
        trumpet.Pause();
        piano.Pause();
        clarinet.Pause();
        drums.Pause();
        horn.Pause();
    }

    void PlayInstruments()
    {
        xylophone.Play();
        violin.Play();
        tuba.Play();
        cymbals.Play();
        harp.Play();
        flute.Play();
        trumpet.Play();
        piano.Play();
        clarinet.Play();
        drums.Play();
        horn.Play();
    }
}
