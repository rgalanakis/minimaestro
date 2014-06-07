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
		glowSparkle.gameObject.SetActive(true);
		StopSparkle();
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
			StopSparkle();
        }
    }
	
	private void StartSparkle(InstrumentDragAndDrop instrument, float mult)
	{
		Color col = instrument.instrumentColor;
		glowSparkle.startColor = new Color(col.r, col.g, col.b, mult);
		//glowSparkle.gameObject.SetActive(true);
		glowSparkle.gameObject.renderer.enabled = true;
	}

	private void StopSparkle()
	{
		glowSparkle.startColor = defaultColor;
		glowSparkle.gameObject.renderer.enabled = false;
		//glowSparkle.gameObject.SetActive(false);
	}
}
