using UnityEngine;
using System.Collections;
using System;

public class HighlightEventManager : MonoBehaviour
{
    public delegate void HighlightEvent(HighlightManager.InstrumentType type);
    public static event HighlightEvent Highlight;

    public static event Action NoHighlight;
    
    public static void TriggerInstrumentHighlight(HighlightManager.InstrumentType type)
    {
        if (Highlight != null)
        {
            Highlight(type);
        }
    }
    
    public static void TriggerInstrumentNoHighlight()
    {
        if (NoHighlight != null)
        {
            NoHighlight();
        }
    }
}
