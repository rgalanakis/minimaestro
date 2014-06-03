using UnityEngine;
using System.Collections;

public class HighlightManager : MonoBehaviour
{
    public GameObject windButton;
    public GameObject stringButton;
    public GameObject brassButton;
    public GameObject percussionButton;
    public UISprite blackOverlay;
    public InstrumentDragAndDrop[] winds;
    public InstrumentDragAndDrop[] brasses;
    public InstrumentDragAndDrop[] strings;
    public InstrumentDragAndDrop[] percussions;
    enum InstrumentType
    {
        Wind = 0,
        String = 1,
        Brass = 2,
        Percussion = 3
    }
    void Start()
    {
        blackOverlay.gameObject.SetActive(false);
        blackOverlay.SetDimensions(Screen.width * 3, Screen.height * 3);
        UIEventListener.Get(windButton).onPress += OnButtonWind;
        UIEventListener.Get(stringButton).onPress += OnButtonString;
        UIEventListener.Get(brassButton).onPress += OnButtonBrass;
        UIEventListener.Get(percussionButton).onPress += OnButtonPercussion;
    }
    
    void OnDestroy()
    {
        NGUIHelper.RemovePressEventListener(windButton, OnButtonWind);
        NGUIHelper.RemovePressEventListener(stringButton, OnButtonString);
        NGUIHelper.RemovePressEventListener(brassButton, OnButtonBrass);
        NGUIHelper.RemovePressEventListener(percussionButton, OnButtonPercussion);
    }
    void DisplayBlackOverlay(bool display)
    {
        blackOverlay.gameObject.SetActive(display);
    }
    void OnButtonWind(GameObject go, bool press)
    {
        if (press)
        {
            DisplayBlackOverlay(true);
            Highlight(InstrumentType.Wind);
        }
        else
        {
            DisplayBlackOverlay(false);
            NoHighlight();
        }
    }
    
    void OnButtonString(GameObject go, bool press)
    {
        if (press)
        {
            DisplayBlackOverlay(true);
            Highlight(InstrumentType.String);
        }
        else
        {
            DisplayBlackOverlay(false);
            NoHighlight();
        }
    }
    
    void OnButtonBrass(GameObject go, bool press)
    {
        if (press)
        {
            DisplayBlackOverlay(true);
            Highlight(InstrumentType.Brass);
        }
        else
        {
            DisplayBlackOverlay(false);
            NoHighlight();
        }

    }

    void OnButtonPercussion(GameObject go, bool press)
    {
        if (press)
        {
            DisplayBlackOverlay(true);
            Highlight(InstrumentType.Percussion);
        }
        else
        {
            DisplayBlackOverlay(false);
            NoHighlight();
        }
    }

    void EnableHighlightGroup(InstrumentDragAndDrop[] group, bool highlight)
    {
        foreach (InstrumentDragAndDrop item in group)
        {
            item.HighlightWithContainer(highlight);
        }
    }
    void NoHighlight()
    {
        EnableHighlightGroup(winds, false);
        EnableHighlightGroup(brasses, false);
        EnableHighlightGroup(strings, false);
        EnableHighlightGroup(percussions, false);
    }
    void Highlight(InstrumentType type)
    {
        NoHighlight();
        if (type == InstrumentType.Wind)
        {
            EnableHighlightGroup(winds, true);
        }
        else if (type == InstrumentType.Brass)
        {
            EnableHighlightGroup(brasses, true);
        }
        else if (type == InstrumentType.String)
        {
            EnableHighlightGroup(strings, true);
        }
        else
        {
            EnableHighlightGroup(percussions, true);
        }
    }
}
