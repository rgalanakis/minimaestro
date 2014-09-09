using UnityEngine;
using System.Collections;

public class Tutorial : MonoBehaviour
{
    private static bool tutorialCompleted = false;

    public GameObject hand;
    public GameObject handPlayButton;
    public GameObject particlePlayButton;
    public GameObject playButton;
    public Vector3 endOffset;
    private float duration = 1.5f;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private int songSwitchCount;

    void Awake()
    {
        // world position isn't working, not sure why.
        handPlayButton.SetActive(false);
        Vector3 handPlayPos = playButton.transform.localPosition;
        handPlayPos.y -= 50.0f;
        handPlayButton.transform.localPosition = handPlayPos;
        particlePlayButton.SetActive(false);
        startPosition = hand.transform.localPosition;
        endPosition = startPosition + endOffset;
        EventManager.Drop += CompleteTutorial;
        EventManager.Pause += OnPause;
        EventManager.Resume += OnResume;
        EventManager.SongSwitched += OnSwitchedSong;
        EventManager.SongCompleted += OnCompleteSong;

        if (!tutorialCompleted)
        {
            EventDelegate.Add(TweenPosition.Begin(hand, duration, endPosition).onFinished, TweenEndFinished, true);
        }
    }
    void OnDestroy()
    {
        EventManager.Pause -= OnPause;
        EventManager.Resume -= OnResume;
        EventManager.Drop -= CompleteTutorial;
        EventManager.SongSwitched -= OnSwitchedSong;
        EventManager.SongCompleted -= OnCompleteSong;
    }

    public void TweenEndFinished()
    {
        TweenHide();
    }

    public void TweenHide()
    {
        if (!hand.activeSelf)
        {
            return;
        }
        EventDelegate.Add(TweenAlpha.Begin(hand, 0.75f, 0.1f).onFinished, TweenToEndPosition, true);
    }

    public void TweenToEndPosition()
    {
        if (!hand.activeSelf)
        {
            return;
        }
        TweenPosition.Begin(hand, 0.0f, startPosition);
        TweenAlpha.Begin(hand, 0.0f, 1.0f);
        EventDelegate.Add(TweenPosition.Begin(hand, duration, endPosition).onFinished, TweenEndFinished, true);
    }

    public void CompleteTutorial(GameObject container, GameObject instrumentObj)
    {
        if (container.tag == "stage_grid" && !tutorialCompleted)
        {
            tutorialCompleted = true;
            hand.SetActive(false);
            EventManager.TriggerTutorialCompleted();
        }
    }

    void OnPause()
    {
        EnableTweens(false);
    }

    void OnResume()
    {
        EnableTweens(true);
    }

    void EnableTweens(bool enabled)
    {
        UITweener[] tweens = hand.GetComponents<UITweener>();
        foreach (UITweener tween in tweens)
        {
            tween.enabled = enabled;
        }
    }

    void OnSwitchedSong(SongObject song)
    {
        songSwitchCount++;
        if (songSwitchCount > 1)
        {
            handPlayButton.SetActive(false);
            particlePlayButton.SetActive(false);
        }
       
    }

    void OnCompleteSong()
    {
        if (songSwitchCount < 2)
        {
            particlePlayButton.SetActive(true);
            handPlayButton.SetActive(true);
            TweenPlayButtonToStartPositionFinished();
        }
    }

    public void TweenPlayButtonEndFinished()
    {
        if (!handPlayButton.activeSelf)
        {
            return;
        }
        Vector3 startPos = playButton.transform.localPosition;
        startPos.y -= 50;
        //startPos.x = handPlayButton.transform.localPosition.x;
        EventDelegate.Add(TweenPosition.Begin(handPlayButton, duration, startPos).onFinished, TweenPlayButtonToStartPositionFinished, true);
    }

    public void TweenPlayButtonToStartPositionFinished()
    {
        if (!handPlayButton.activeSelf)
        {
            return;
        }
        Vector3 endPos = playButton.transform.localPosition;
        endPos.y += 20; 
        //endPos.x = handPlayButton.transform.localPosition.x;
        EventDelegate.Add(TweenPosition.Begin(handPlayButton, duration, endPos).onFinished, TweenPlayButtonEndFinished, true);
    }
}
