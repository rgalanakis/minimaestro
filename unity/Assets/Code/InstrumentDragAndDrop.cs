﻿using UnityEngine;
using System.Collections;
using System.Linq;

public class InstrumentDragAndDrop : UIDragDropItem
{
    public GameObject startingContainer;
    public GameObject currentContainer;
    public new AudioSource audio;
    public int qSamples = 1024;       // array size
    public float refValue = 0.1f;  // RMS value for 0 dB
    public float rmsValue;        // sound level - RMS
    public float dbValue;       // sound level - dB
    public float volume = 20;     // set how much the scale will vary
    private float[] samples;     // audio samples
    private Vector3 volumeScale = Vector3.one;
    public float tweenDuration;
    private new BoxCollider collider;
    private int highlightDepth = 15;
    private int normalDepth = 12;
    public Color instrumentColor;
    public ParticleSystem musicNotes;
    public HighlightManager.InstrumentType instrumentType;
    private Color normalColor = Color.white;
    private Color noHighlightColor = new Color(0.3f, 0.3f, 0.3f, 1.0f);
    public int notePulseCount = 3;
    public float notePulseThreshold = 2.2f;
    private const float nonHighlightedInstrumentVolume = 0.5f;

    protected override void Start()
    {
        Analytics.Initialize();
        base.Start();
        startingContainer = NGUITools.FindInParents<UIDragDropContainer>(this.gameObject).gameObject;
        currentContainer = startingContainer;
        audio = GetComponent<AudioSource>();
        samples = new float[qSamples];
        collider = GetComponent<BoxCollider>();

        musicNotes.gameObject.SetActive(false);
        SetContainerAndUpdate(currentContainer);
        EventManager.Highlight += HighlightWithContainer;
        EventManager.NoHighlight += NoHightlight;
        EventManager.SongSwitched += OnSongSwitched;
        EventManager.DemoSongStart += OnDemoStart;
        EventManager.Pause += OnPause;
        EventManager.Resume += OnResume;
    }

    void OnDestroy()
    {
        EventManager.Highlight -= HighlightWithContainer;
        EventManager.NoHighlight -= NoHightlight;
        EventManager.SongSwitched -= OnSongSwitched;
        EventManager.DemoSongStart -= OnDemoStart;
        EventManager.Pause -= OnPause;
        EventManager.Resume -= OnResume;
    }

    public void ScaleTweenFinished()
    {
        UpdateVolume();
        UpdateNotes();
        UpdateScale();
    }

    void UpdateVolume()
    {
        audio.GetOutputData(samples, 0);  // fill array with samples
        float sum = 0;
        for (int i = 0; i < qSamples; i++)
        {
            sum += samples [i] * samples [i];    // sum squared samples
        }
        rmsValue = Mathf.Sqrt(sum / qSamples);  // rms = square root of average
        dbValue = 20 * Mathf.Log10(rmsValue / refValue);    // calculate dB
        if (dbValue < -160)
        {
            dbValue = -160;   // clamp it to -160 dB min
        }
    }

    /// <summary>
    /// Updates notes.
    /// If volume is 0 (instrument is offstage), don't do anything.
    /// If volume is low and instrument is active,
    /// stop the particle system.
    /// Otherwise, music is coming from the instrument so
    /// 1) make sure it is playing and
    /// 2) emit a burse of particles if the volume is over notePulseThreshold.
    /// We use Play and Stop on the particle system instead of SetActive,
    /// because if particles are showing and the particle system is deactivated,
    /// the particles will flash.
    /// </summary>
    void UpdateNotes()
    {
        if (audio.volume == 0.0f)
        {
            return;
        }
        if (dbValue <= -160 && musicNotes.gameObject.activeSelf)
        {
            musicNotes.Stop();
        }
        else
        {
            if (!musicNotes.isPlaying)
            {
                musicNotes.Play();
            }
            if (dbValue > notePulseThreshold)
            {
                musicNotes.Emit(notePulseCount);
            }
        }
    }

    void OnPause()
    {
        musicNotes.gameObject.SetActive(false);
    }

    void OnResume()
    {
        if (currentContainer.tag == "stage_grid")
        {
            musicNotes.gameObject.SetActive(true);
        }
    }

    void UpdateScale()
    {
        float scaleValue = Mathf.Lerp(1.0f, 1.2f, rmsValue / 0.3f);
        volumeScale.Set(scaleValue, scaleValue, transform.localScale.z);
        TweenScale.Begin(this.gameObject, tweenDuration, volumeScale);
    }

    protected override void OnDragDropStart()
    {
        EventManager.TriggerInstrumentDrag(this.gameObject);
        base.OnDragDropStart();
    }

    protected override void OnDragDropRelease(GameObject target)
    {
        if (target == null)
        {
            SetContainerAndUpdate(startingContainer);
            return;
        }

        GameObject targetContainer = target;
        InstrumentDragAndDrop instrument = target.GetComponentInChildren<InstrumentDragAndDrop>();

        if (targetContainer.tag == "instrument_grid")
        {
            SetContainerAndUpdate(startingContainer);
        }
        else if (instrument != null && instrument.currentContainer.tag == "stage_grid")
        { 
        
            targetContainer = instrument.currentContainer;
        
            if (this.currentContainer.tag == "stage_grid" && targetContainer.tag == "stage_grid")
            {
                instrument.SetContainerAndUpdate(this.currentContainer);
                SetContainerAndUpdate(targetContainer);
                return;
            }
        
            instrument.SetContainerAndUpdate(instrument.startingContainer);
            SetContainerAndUpdate(targetContainer);
        }
        else if (targetContainer.tag == "instrument_item" && targetContainer != startingContainer)
        {
            SetContainerAndUpdate(startingContainer);  
        }
        else if (targetContainer.tag == "stage_grid")
        {
            SetContainerAndUpdate(targetContainer);
        }
        else
        {
            SetContainerAndUpdate(startingContainer);  
        }

    }

    public void SetContainerAndUpdate(GameObject targetContainer)
    {
        currentContainer = targetContainer;
        base.OnDragDropRelease(targetContainer);
        EventManager.TriggerInstrumentDrop(targetContainer.gameObject, this.gameObject);
        if (targetContainer != null)
        {
            if (targetContainer.tag == "stage_grid")
            {
                musicNotes.gameObject.SetActive(true);
                ScaleTweenFinished();
                audio.volume = 1.0f;
            }
            else
            {
                musicNotes.gameObject.SetActive(false);
                transform.localScale = Vector3.one;
                audio.volume = 0.0f;
            }
        }
    }

    private void NoHightlight()
    {
        collider.enabled = true;
        this.GetComponent<UISprite>().color = normalColor; 
        if (currentContainer.tag == "stage_grid")
        {
            audio.volume = 1.0f;
        }
    }

    public void HighlightWithContainer(HighlightManager.InstrumentType type)
    {
        collider.enabled = false;
        UISprite sprite = this.GetComponent<UISprite>();
        if (instrumentType == type)
        {
            sprite.depth = highlightDepth;
            currentContainer.GetComponent<UISprite>().depth = highlightDepth - 1;
            startingContainer.GetComponent<UISprite>().depth = highlightDepth - 1;
        }
        else
        {
            sprite.depth = normalDepth;
            if (currentContainer.tag == "stage_grid")
            {
                audio.volume = nonHighlightedInstrumentVolume;
                sprite.color = noHighlightColor;
            }
            currentContainer.GetComponent<UISprite>().depth = normalDepth - 1;
            startingContainer.GetComponent<UISprite>().depth = normalDepth - 1;
        }
    }

    void OnSongSwitched(SongObject song)
    {
        musicNotes.Clear();
    }

    void OnDemoStart(InstrumentDragAndDrop[] instruments, StageContainer[] stageContainers)
    {
        int ind = System.Array.IndexOf(instruments, this);
        if (ind >= 0)
        {
            SetContainerAndUpdate(stageContainers [ind].gameObject);
        }
        else
        {
            SetContainerAndUpdate(startingContainer);
        }
    }
}
