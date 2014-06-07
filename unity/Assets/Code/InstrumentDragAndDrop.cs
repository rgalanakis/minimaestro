using UnityEngine;
using System.Collections;

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
	public int notePulseCount = 4;
	public float notePulseThreshold = 2.2f;

    protected override void Start()
    {
        base.Start();
        startingContainer = NGUITools.FindInParents<UIDragDropContainer>(this.gameObject).gameObject;
        currentContainer = startingContainer;
        audio = GetComponent<AudioSource>();
        samples = new float[qSamples];
        collider = GetComponent<BoxCollider>();

        musicNotes.gameObject.SetActive(false);
        SetContainerAndUpdate(currentContainer);
        HighlightEventManager.Highlight += HighlightWithContainer;
        HighlightEventManager.NoHighlight += NoHightlight;
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

    void UpdateNotes()
    {
        if (audio.volume == 0.0f)
        {
            return;
        }
        if (dbValue <= -160 && musicNotes.gameObject.activeSelf)
        {
            musicNotes.gameObject.SetActive(false);
        }
        else
        {
			musicNotes.gameObject.SetActive(true);
			if (dbValue > notePulseThreshold)
			{
				musicNotes.Emit(notePulseCount);
			}
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
        InstrumentEventManager.TriggerInstrumentDrag(this.gameObject);
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
        else
        {
            SetContainerAndUpdate(targetContainer);
        }

    }

    public void SetContainerAndUpdate(GameObject targetContainer)
    {
        currentContainer = targetContainer;
        base.OnDragDropRelease(targetContainer);
        InstrumentEventManager.TriggerInstrumentDrop(targetContainer.gameObject, this.gameObject);
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
                sprite.color = noHighlightColor;
            }
            currentContainer.GetComponent<UISprite>().depth = normalDepth - 1;
            startingContainer.GetComponent<UISprite>().depth = normalDepth - 1;
        }

    }

}
