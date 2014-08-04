using UnityEngine;
using System.Collections;
using System;

public class EventManager : MonoBehaviour
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

    #region Song Events

    public delegate void SongSwitchEvent(SongObject song);
    public static event SongSwitchEvent SongSwitched;

    public static void TriggerSongSwitch(SongObject song)
    {
        if (SongSwitched != null)
        {
            SongSwitched(song);
        }
    }

    public delegate void DemoSongStartEvent(InstrumentDragAndDrop[] instruments,StageContainer[] stageContainers);
    public static event DemoSongStartEvent DemoSongStart;

    public static void TriggerDemoSongStart(InstrumentDragAndDrop[] instruments, StageContainer[] stageContainers)
    {
        if (DemoSongStart != null)
        {
            DemoSongStart(instruments, stageContainers);
        }
    }

    #endregion

    # region Misc Events

    public static event Action Pause;
    public static void TriggerPause()
    {
        if (Pause != null)
        {
            Pause();
        }
    }

    public static Action Resume;
    public static void TriggerResume()
    {
        if (Resume != null)
        {
            Resume();
        }
    }

    public static event Action TutorialCompleted;
    public static void TriggerTutorialCompleted()
    {
        if (TutorialCompleted != null)
        {
            TutorialCompleted();
        }
    }

    public static event Action SongCompleted;
    public static void TriggerSongCompleted()
    {
        if (SongCompleted != null)
        {
            SongCompleted();
        }
    }
    #endregion
}
