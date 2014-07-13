using UnityEngine;
using System.Collections;

public class Tutorial : MonoBehaviour
{
    public GameObject hand;
    public AccountSettings account;
    public Vector3 startPostion;
    public Vector3 endPosition;
    private float duration = 1.5f;
    // Update is called once per frame
    void Awake()
    {
        account.CompletedTutorialEvent += CompletedTutorial;
        EventManager.Pause += OnPause;
        EventManager.Resume += OnResume;
        if (!AccountSettings.completedTutorial)
        {
            EventDelegate.Add(TweenPosition.Begin(hand, duration, endPosition).onFinished, TweenEndFinished, true);
        }
    }
    void OnDestroy()
    {
        EventManager.Pause -= OnPause;
        EventManager.Resume -= OnResume;
        account.CompletedTutorialEvent -= CompletedTutorial;
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
        TweenPosition.Begin(hand, 0.0f, startPostion);
        TweenAlpha.Begin(hand, 0.0f, 1.0f);
        EventDelegate.Add(TweenPosition.Begin(hand, duration, endPosition).onFinished, TweenEndFinished, true);
    }

    public void CompletedTutorial()
    {
        hand.SetActive(false);
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
