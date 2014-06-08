using UnityEngine;
using System.Collections;

public class StageContainer : UIDragDropContainer
{
    public ParticleSystem glowSparkle;
    private GameObject instrumentOnStage;
    private float dragColorAlpha = 0.1f;
    private float hasInstrumentColorAlpha = 0.2f;
    private Color defaultColor = new Color(1f, 1f, 1f, 0.02f);
    void Start()
    {
        // We have these disabled in the editor, enable them here.
        glowSparkle.gameObject.SetActive(true);
        StopSparkle();
        EventManager.Drag += DragGlow;
        EventManager.Drop += DropInstrument;
        EventManager.Highlight += HighlightWithContainer;
        EventManager.NoHighlight += NoHightlight;
    }

    void OnDestroy()
    {
        EventManager.Drag -= DragGlow;
    }

    void DragGlow(GameObject instrumentObj)
    {
        InstrumentDragAndDrop instrument = instrumentObj.GetComponent<InstrumentDragAndDrop>();
        if (instrumentOnStage == null || instrumentObj == instrumentOnStage)
        {
            instrumentOnStage = null;
            StartSparkle(instrument, dragColorAlpha);
        }
    }

    public void DropInstrument(GameObject container, GameObject instrumentObj)
    {
        InstrumentDragAndDrop instrument = instrumentObj.GetComponent<InstrumentDragAndDrop>();
        if (container == this.gameObject)
        {
            StartSparkle(instrument, hasInstrumentColorAlpha);
            instrumentOnStage = instrumentObj;
        }
        else if (instrumentOnStage != null)
        {
            return;
        }
        else
        {
            instrumentOnStage = null;
            StopSparkle();
        }
    }

    // Use renderer.enabled, so the particles can change color under the hood.
    // Using SetActive will not allow the particles to change color while invisible.
    private void StartSparkle(InstrumentDragAndDrop instrument, float mult)
    {
        Color col = instrument.instrumentColor;
        glowSparkle.startColor = new Color(col.r, col.g, col.b, mult);
        glowSparkle.Play();
    }

    private void StopSparkle()
    {
        glowSparkle.startColor = defaultColor;
        glowSparkle.Stop();
    }

    private void NoHightlight()
    {
        if (instrumentOnStage != null)
        {
            glowSparkle.gameObject.SetActive(true);
        }
    }

    public void HighlightWithContainer(HighlightManager.InstrumentType instrumentType)
    {
        if (instrumentOnStage != null)
        {
            InstrumentDragAndDrop instrument = instrumentOnStage.GetComponent<InstrumentDragAndDrop>();
            if (instrument.instrumentType != instrumentType)
            {
                glowSparkle.gameObject.SetActive(false);
            }
        }

    }
}
