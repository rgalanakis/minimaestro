using UnityEngine;
using System.Collections;
using System;

public static class InstrumentEventManager
{
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
}
