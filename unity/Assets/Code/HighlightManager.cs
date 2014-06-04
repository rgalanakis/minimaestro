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
    public UILabel sectionLabel;
    enum InstrumentType
    {
        Woodwind = 0,
        String = 1,
        Brass = 2,
        Percussion = 3
    }
    void Start()
    {
        blackOverlay.gameObject.SetActive(false);
        sectionLabel.gameObject.SetActive(false);
        blackOverlay.SetDimensions(Screen.width * 3, Screen.height * 3);
        UIEventListener.Get(windButton).onClick += OnButtonWind;
        UIEventListener.Get(stringButton).onClick += OnButtonString;
        UIEventListener.Get(brassButton).onClick += OnButtonBrass;
        UIEventListener.Get(percussionButton).onClick += OnButtonPercussion;
    }
    
    void OnDestroy()
    {
        NGUIHelper.RemoveClickEventListener(windButton, OnButtonWind);
        NGUIHelper.RemoveClickEventListener(stringButton, OnButtonString);
        NGUIHelper.RemoveClickEventListener(brassButton, OnButtonBrass);
        NGUIHelper.RemoveClickEventListener(percussionButton, OnButtonPercussion);
    }
    void Update()
    {
        if (blackOverlay.gameObject.activeSelf)
        {
            if (Input.GetMouseButtonDown(0))
            {
                DisplayBlackOverlay(false);
                NoHighlight();
            }
        }
    }
    void DisplayBlackOverlay(bool display)
    {
        blackOverlay.gameObject.SetActive(display);
    }
    void OnButtonWind(GameObject go)
    {
        DisplayBlackOverlay(true);
        Highlight(InstrumentType.Woodwind);
    }
    
    void OnButtonString(GameObject go)
    {
        DisplayBlackOverlay(true);
        Highlight(InstrumentType.String);
    }
    
    void OnButtonBrass(GameObject go)
    {
        DisplayBlackOverlay(true);
        Highlight(InstrumentType.Brass);
    }

    void OnButtonPercussion(GameObject go)
    {
        DisplayBlackOverlay(true);
        Highlight(InstrumentType.Percussion);
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
        sectionLabel.gameObject.SetActive(false);
        EnableHighlightGroup(winds, false);
        EnableHighlightGroup(brasses, false);
        EnableHighlightGroup(strings, false);
        EnableHighlightGroup(percussions, false);
    }
    void Highlight(InstrumentType type)
    {
        NoHighlight();
        sectionLabel.gameObject.SetActive(true);
        if (type == InstrumentType.Woodwind)
        {
            sectionLabel.text = "Woodwind Instruments";
            EnableHighlightGroup(winds, true);
        }
        else if (type == InstrumentType.Brass)
        {
            sectionLabel.text = "Brass Instruments";
            EnableHighlightGroup(brasses, true);
        }
        else if (type == InstrumentType.String)
        {
            sectionLabel.text = "String Instruments";
            EnableHighlightGroup(strings, true);
        }
        else
        {
            sectionLabel.text = "Percussion Instruments";
            EnableHighlightGroup(percussions, true);
        }
    }
}
