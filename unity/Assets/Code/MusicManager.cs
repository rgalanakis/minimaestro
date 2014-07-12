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
        UIEventListener.Get(playButton).onClick += OnButtonPlay;
        UIEventListener.Get(demoButton).onClick += OnButtonDemo;
        songEnumerator = EnumerateSongs().GetEnumerator();
        SwitchSong(false);
        EventManager.Highlight += OnHighlightOn;
        EventManager.NoHighlight += OnHighlightOff;
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
        songNameLabel.text = songName;
        songNameLabel.alpha = 1.0f;
        TweenAlpha.Begin(songNameLabel.gameObject, 3.0f, 0.0f);
    }
    void SwitchSong(SongObject newSong, bool demo)
    {
        DisplayAndFadeSongName(newSong.displayName);

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

        EventManager.TriggerSongSwitch(newSong);

        if (demo && newSong.demo != null)
        {
            EventManager.TriggerDemoSongStart(newSong.demo.instruments, newSong.demo.stageCointainers);
        }
    }
}
