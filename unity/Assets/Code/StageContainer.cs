using UnityEngine;
using System.Collections;

public class StageContainer : UIDragDropContainer
{
    public ParticleSystem glowSparkle;
    private GameObject instrumentOnStage;
    private Color dragColorMult = new Color(1f, 1f, 1f, 0.2f);
    private Color hasInstrumentColorMult = new Color(1f, 1f, 1f, 0.3f);
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

    void DragGlow(GameObject instrumentObj)
    {
		var instrument = instrumentObj.GetComponent<InstrumentDragAndDrop>();
        if (instrumentOnStage == null || instrument == instrumentOnStage)
        {
            instrumentOnStage = null;
            glowSparkle.gameObject.SetActive(true);
            glowSparkle.startColor = instrument.instrumentColor * dragColorMult;
        }
    }

    public void DropInstrument(GameObject container, GameObject instrumentObj)
    {
		var instrument = instrumentObj.GetComponent<InstrumentDragAndDrop>();
        if (container == this.gameObject)
        {
            glowSparkle.gameObject.SetActive(true);
            glowSparkle.startColor = instrument.instrumentColor * hasInstrumentColorMult;
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
