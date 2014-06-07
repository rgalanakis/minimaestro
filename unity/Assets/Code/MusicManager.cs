using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
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

    public SongObject currentSongPlaying;

    public SongObject valkyrie;
    public SongObject spring;

    void Start()
    {
        UIEventListener.Get(playButton).onClick += OnButtonPlay;
        UIEventListener.Get(demoButton).onClick += OnButtonDemo;
    }

    void OnDestroy()
    {
        NGUIHelper.RemoveClickEventListener(playButton, OnButtonPlay);
        NGUIHelper.RemoveClickEventListener(demoButton, OnButtonDemo);
    }

    void OnButtonPlay(GameObject go)
    {
        if (currentSongPlaying == valkyrie)
        {
            SwitchSong(spring);
            currentSongPlaying = spring;
        } else
        {
            SwitchSong(valkyrie);
            currentSongPlaying = valkyrie;
        }
    }

    void OnButtonDemo(GameObject go)
    {
        
    }

    void SwitchSong(SongObject newSong)
    {
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

    }
}
