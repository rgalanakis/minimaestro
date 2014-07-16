using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class HighlightManager : MonoBehaviour
{
    public GameObject windButton;
    public GameObject stringButton;
    public GameObject brassButton;
    public GameObject percussionButton;

    private UISprite windButtonSprite;
    private UISprite stringButtonSprite;
    private UISprite brassButtonSprite;
    private UISprite percussionButtonSprite;
    private const int normalDepth = 10;
    private const int highlightDepth = 14;

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
        windButtonSprite = windButton.GetComponentInChildren<UISprite>();
        stringButtonSprite = stringButton.GetComponentInChildren<UISprite>();
        brassButtonSprite = brassButton.GetComponentInChildren<UISprite>();
        percussionButtonSprite = percussionButton.GetComponentInChildren<UISprite>();
        blackOverlay.gameObject.SetActive(false);
        sectionLabel.gameObject.SetActive(false);
        blackOverlay.SetDimensions(Screen.width * 3, Screen.height * 3);
        UIEventListener.Get(windButton).onClick += OnButtonClick;
        UIEventListener.Get(stringButton).onClick += OnButtonClick;
        UIEventListener.Get(brassButton).onClick += OnButtonClick;
        UIEventListener.Get(percussionButton).onClick += OnButtonClick;
        EventManager.Highlight += OnHighlightOn;
        EventManager.NoHighlight += OnHighlightOff;
    }

    void OnDestroy()
    {
        NGUIHelper.RemoveClickEventListener(windButton, OnButtonClick);
        NGUIHelper.RemoveClickEventListener(stringButton, OnButtonClick);
        NGUIHelper.RemoveClickEventListener(brassButton, OnButtonClick);
        NGUIHelper.RemoveClickEventListener(percussionButton, OnButtonClick);
        EventManager.Highlight -= OnHighlightOn;
        EventManager.NoHighlight -= OnHighlightOff;
    }

    void Update()
    {
        if (blackOverlay.gameObject.activeSelf)
        {
            if (CheckButtonTap())
            {
                EventManager.TriggerInstrumentNoHighlight();
            }
        }
    }
    bool CheckButtonTap()
    {
        bool windows = Application.isEditor;
        #if UNITY_STANDALONE_WIN
        windows = true;
        #endif
        if (windows)
        {
            if (Input.GetMouseButtonDown(0))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began; 
        }
    }
    void OnHighlightOn(InstrumentType instrumentType)
    {    
        sectionLabel.text = instrumentType.ToString("F");
        sectionLabel.gameObject.SetActive(true);
        blackOverlay.gameObject.SetActive(true);
        EnableButtonCollider(false);
    }

    void OnHighlightOff()
    {
        sectionLabel.gameObject.SetActive(false);
        blackOverlay.gameObject.SetActive(false);
        windButtonSprite.depth = normalDepth;
        brassButtonSprite.depth = normalDepth;
        stringButtonSprite.depth = normalDepth;
        percussionButtonSprite.depth = normalDepth;
        EnableButtonCollider(true);
    }

    private void EnableButtonCollider(bool enabled)
    {
        windButton.collider.enabled = enabled;
        brassButton.collider.enabled = enabled;
        percussionButton.collider.enabled = enabled;
        stringButton.collider.enabled = enabled;
    }

    void OnButtonClick(GameObject go)
    {
        InstrumentType it;
        if (go == windButton)
        {
            it = InstrumentType.Woodwind;
            windButtonSprite.depth = highlightDepth;

        }
        else if (go == stringButton)
        {
            it = InstrumentType.String;
            stringButtonSprite.depth = highlightDepth;
        }
        else if (go == brassButton)
        {
            it = InstrumentType.Brass;
            brassButtonSprite.depth = highlightDepth;
        }
        else
        {
            it = InstrumentType.Percussion;
            percussionButtonSprite.depth = highlightDepth;
        }
        EventManager.TriggerInstrumentHighlight(it);
    }

}
