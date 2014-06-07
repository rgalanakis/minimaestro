using UnityEngine;
using System.Collections;

public class HighlightManager : MonoBehaviour
{
    public GameObject windButton;
    public GameObject stringButton;
    public GameObject brassButton;
    public GameObject percussionButton;
    public UISprite blackOverlay;
    public UILabel sectionLabel;
    public enum InstrumentType
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
        UIEventListener.Get(windButton).onClick += OnButtonClick;
        UIEventListener.Get(stringButton).onClick += OnButtonClick;
        UIEventListener.Get(brassButton).onClick += OnButtonClick;
        UIEventListener.Get(percussionButton).onClick += OnButtonClick;
    }
    
    void OnDestroy()
    {
        NGUIHelper.RemoveClickEventListener(windButton, OnButtonClick);
        NGUIHelper.RemoveClickEventListener(stringButton, OnButtonClick);
        NGUIHelper.RemoveClickEventListener(brassButton, OnButtonClick);
        NGUIHelper.RemoveClickEventListener(percussionButton, OnButtonClick);
    }
    void Update()
    {
        if (blackOverlay.gameObject.activeSelf)
        {
            if (Input.GetMouseButtonDown(0))
            {
                blackOverlay.gameObject.SetActive(false);
                HighlightEventManager.TriggerInstrumentNoHighlight();
            }
        }
    }
    void OnButtonClick(GameObject go)
    {
        blackOverlay.gameObject.SetActive(true);
        if (go == windButton)
        {
            HighlightEventManager.TriggerInstrumentHighlight(InstrumentType.Woodwind);
        }
        else if (go == stringButton)
        {
            HighlightEventManager.TriggerInstrumentHighlight(InstrumentType.String);
        }
        else if (go == brassButton)
        {
            HighlightEventManager.TriggerInstrumentHighlight(InstrumentType.Brass);
        }
        else
        {
            HighlightEventManager.TriggerInstrumentHighlight(InstrumentType.Percussion);
        }
       
    }

}
