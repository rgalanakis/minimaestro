using UnityEngine;
using System.Collections;
using System.Diagnostics;

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
		HighlightEventManager.Highlight += OnHighlightOn;
		HighlightEventManager.NoHighlight += OnHighlightOff;
	}

    void OnDestroy()
    {
        NGUIHelper.RemoveClickEventListener(windButton, OnButtonClick);
        NGUIHelper.RemoveClickEventListener(stringButton, OnButtonClick);
        NGUIHelper.RemoveClickEventListener(brassButton, OnButtonClick);
        NGUIHelper.RemoveClickEventListener(percussionButton, OnButtonClick);
		HighlightEventManager.Highlight -= OnHighlightOn;
		HighlightEventManager.NoHighlight -= OnHighlightOff;
    }

    void Update()
    {
        if (blackOverlay.gameObject.activeSelf)
        {
            if (Input.GetMouseButtonDown(0))
            {
                HighlightEventManager.TriggerInstrumentNoHighlight();
            }
        }
    }

	void OnHighlightOn(InstrumentType instrumentType)
	{		
		sectionLabel.text = instrumentType.ToString("F");
		sectionLabel.gameObject.SetActive(true);
		blackOverlay.gameObject.SetActive(true);
	}

	void OnHighlightOff()
	{
		sectionLabel.gameObject.SetActive(false);
		blackOverlay.gameObject.SetActive(false);
	}

    void OnButtonClick(GameObject go)
    {
		InstrumentType it;
        if (go == windButton)
        {
			it = InstrumentType.Woodwind;
        }
        else if (go == stringButton)
        {
			it = InstrumentType.String;
        }
        else if (go == brassButton)
        {
			it = InstrumentType.Brass;
        }
        else
        {
			System.Diagnostics.Debug.Assert(go == percussionButton);
            it = InstrumentType.Percussion;
        }
		HighlightEventManager.TriggerInstrumentHighlight(it);
       
    }

}
