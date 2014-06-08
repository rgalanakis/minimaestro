﻿using UnityEngine;
using System.Collections;
using System;

public class EventManager
{
    #region Highlight Events

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

    #endregion

    #region Instrument Events
    
    public delegate void DragEvent(GameObject instrument);
    public static event DragEvent Drag;
    
    public delegate void DropEvent(GameObject container,GameObject instrument);
    public static event DropEvent Drop;
    
    public static void TriggerInstrumentDrag(GameObject instrument)
    {
        if (Drag != null)
        {
            Drag(instrument);
        }
    }
    
    public static void TriggerInstrumentDrop(GameObject container, GameObject instrument)
    {
        if (Drop != null)
        {
            Drop(container, instrument);
        }
    }

    #endregion
}
