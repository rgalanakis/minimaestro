using UnityEngine;
using System.Collections;

public class StageContainer : UIDragDropContainer
{
    public ParticleSystem glowSparkle;
    private GameObject instrumentOnStage;
	private float dragColorAlpha = 0.1f;
    private float hasInstrumentColorAlpha = 0.2f;
    // Use this for initialization
    void Start()
    {
        glowSparkle.gameObject.SetActive(false);
        InstrumentEventManager.Drag += DragGlow;
        InstrumentEventManager.Drop += DropInstrument;
    }
    void OnDestroy()
    {
        InstrumentEventManager.Drag -= DragGlow;
    }

	private void SetStartColor(InstrumentDragAndDrop instrument, float mult)
	{
		glowSparkle.gameObject.SetActive(true);
		Color col = instrument.instrumentColor;
		glowSparkle.startColor = new Color(col.r, col.g, col.b, mult);
	}

    void DragGlow(GameObject instrumentObj)
    {
		var instrument = instrumentObj.GetComponent<InstrumentDragAndDrop>();
        if (instrumentOnStage == null || instrument == instrumentOnStage)
        {
            instrumentOnStage = null;
			SetStartColor(instrument, dragColorAlpha);
        }
    }

    public void DropInstrument(GameObject container, GameObject instrumentObj)
    {
		var instrument = instrumentObj.GetComponent<InstrumentDragAndDrop>();
        if (container == this.gameObject)
        {
			SetStartColor(instrument, hasInstrumentColorAlpha);
            instrumentOnStage = instrumentObj;
        }
        else if (instrumentOnStage != null)
        {
            return;
        }
        else
        {
            instrumentOnStage = null;
            glowSparkle.gameObject.SetActive(false);
        }
    }
}
