using UnityEngine;
using System.Collections;

public class StageContainer : UIDragDropContainer
{
    public ParticleSystem glowSparkle;
    private GameObject instrumentOnStage;
	private float dragColorAlpha = 0.1f;
    private float hasInstrumentColorAlpha = 0.2f;

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

    void DragGlow(GameObject instrumentObj)
    {
		var instrument = instrumentObj.GetComponent<InstrumentDragAndDrop>();
        if (instrumentOnStage == null || instrumentObj == instrumentOnStage)
        {
            instrumentOnStage = null;
			StartSparkle(instrument, dragColorAlpha);
        }
    }

    public void DropInstrument(GameObject container, GameObject instrumentObj)
    {
		var instrument = instrumentObj.GetComponent<InstrumentDragAndDrop>();
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
            glowSparkle.gameObject.SetActive(false);
        }
    }
	
	private void StartSparkle(InstrumentDragAndDrop instrument, float mult)
	{
		Color col = instrument.instrumentColor;
		glowSparkle.startColor = new Color(col.r, col.g, col.b, mult);
		glowSparkle.gameObject.SetActive(true);
	}

	private static string TS(object o)
	{
		if (o == null)
		{
			return "<null>";
		}
		return o.ToString();
	}
}
