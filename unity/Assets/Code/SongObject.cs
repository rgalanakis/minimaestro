using UnityEngine;
using System.Collections;

public class SongObject : MonoBehaviour
{
    public AudioClip xylophone;
    public AudioClip violin;
    public AudioClip tuba;
    public AudioClip cymbals;
    public AudioClip harp;
    public AudioClip flute;
    public AudioClip trumpet;
    public AudioClip piano;
    public AudioClip clarinet;
    public AudioClip drums;
    public AudioClip horn;
    public DemoSongObject demo;
    public string displayName;
    public string songId;
        
    /// <summary>
    /// Dead time to add between loops of this song.
    /// You will need to iterate in the build,
    /// because workstation and build audio loop timing can vary.
    /// </summary>
    public float loopSilence = 0.7f;

    public float length
    {
        get { return xylophone.length + loopSilence;}
    }
}
