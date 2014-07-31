using UnityEngine;
using System.Collections;

public class Tutorial : MonoBehaviour
{
    private static bool tutorialCompleted = false;

    public GameObject hand;
    public Vector3 endOffset;
    private float duration = 1.5f;
    private Vector3 startPosition;
    private Vector3 endPosition;

    void Awake()
    {
        // world position isn't working, not sure why.
        startPosition = hand.transform.localPosition;
        endPosition = startPosition + endOffset;
        EventManager.Drop += CompleteTutorial;
        EventManager.Pause += OnPause;
        EventManager.Resume += OnResume;
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
        if (container.tag == "stage_grid")
        {
            if (!tutorialCompleted)
            {
                GoogleAnalytics.SafeLogScreen("tutorial-completed");
            }
            tutorialCompleted = true;
            hand.SetActive(false);
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
}
