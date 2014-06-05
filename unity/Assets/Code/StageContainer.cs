using UnityEngine;
using System.Collections;

public class StageContainer : UIDragDropContainer
{
    public ParticleSystem glowSparkle;
    private GameObject instrumentOnStage;
    private Color dragColor = new Color(0.2f, 0.2f, 0.2f, 0.435f);
    private Color hasInstrumentColor = new Color(1.0f, 1.0f, 1.0f, 0.435f);
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

    void DragGlow(GameObject instrument)
    {
        if (instrumentOnStage == null || instrument == instrumentOnStage)
        {
            instrumentOnStage = null;
            glowSparkle.gameObject.SetActive(true);
            glowSparkle.startColor = dragColor;
        }
    }

    public void DropInstrument(GameObject container, GameObject instrument)
    {
        if (container == this.gameObject)
        {
            glowSparkle.gameObject.SetActive(true);
            glowSparkle.startColor = hasInstrumentColor;
            instrumentOnStage = instrument;
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
