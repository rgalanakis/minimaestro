﻿using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour
{
    public GameObject menuButton;
    public GameObject menuGroup;
    public GameObject facebookButton;
    public GameObject twitterButton;
    public GameObject reviewButton;
    public UISprite underlay;
    private Vector3 hidePosition = new Vector3(-1030, 0, 0);
    bool leftApp = false;
    // Make the overlay larger than the screen to avoid alpha at the edges.
    private const int underlayFudgePixels = 80;

    void Awake()
    {
        hidePosition = new Vector3(-(Screen.width + underlayFudgePixels * 2), 0.0f, 0.0f);
        TweenPosition.Begin(menuGroup, 0.0f, hidePosition);
        UIEventListener.Get(menuButton).onClick += OnButtonPressMenu;
        UIEventListener.Get(facebookButton).onClick += OnFacebookPress;
        UIEventListener.Get(twitterButton).onClick += OnTwitterPress;
        UIEventListener.Get(reviewButton).onClick += OnReviewPress;
        UIEventListener.Get(underlay.gameObject).onClick += HideMenu;
    }

    void OnDestroy()
    {
        NGUIHelper.RemoveClickEventListener(menuButton, OnButtonPressMenu);
        NGUIHelper.RemoveClickEventListener(facebookButton, OnFacebookPress);
        NGUIHelper.RemoveClickEventListener(twitterButton, OnTwitterPress);
        NGUIHelper.RemoveClickEventListener(reviewButton, OnReviewPress);
        NGUIHelper.RemoveClickEventListener(underlay.gameObject, HideMenu);
    }

    void Update()
    {
        if (menuGroup.gameObject.transform.position.x != 0)
        {
            return;
        }

        if (Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            HideMenu(null, true);
        }
    }

    void OnButtonPressMenu(GameObject go)
    {
        if (menuGroup.gameObject.transform.position.x == 0)
        {
            HideMenu(null, true);
        }
        else
        {
            ShowMenu();
        }
    }

    void OnReviewPress(GameObject go)
    {
        if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor)
        {
            return;
        }

        GoogleAnalytics.SafeLogScreen("review-btn-clicked");

        UniRate.Instance.RateIfNetworkAvailable();
    }

    void OnApplicationPause()
    {
        leftApp = true;
    }

    IEnumerator OpenSocialPage(string appUrl, string browserUrl)
    {
        Application.OpenURL(appUrl);
        yield return new WaitForSeconds(1);
        if (leftApp)
        {
            leftApp = false;
        }
        else
        {
            Application.OpenURL(browserUrl);
        }
    }

    void OnFacebookPress(GameObject go)
    {
        GoogleAnalytics.SafeLogScreen("facebook-btn-clicked");
        StartCoroutine(OpenSocialPage("fb://profile/443299165761335", "https://www.facebook.com/SteadfastGames")); 
    }

    void OnTwitterPress(GameObject go)
    {
        GoogleAnalytics.SafeLogScreen("twitter-btn-clicked");
        StartCoroutine(OpenSocialPage("twitter:///user?screen_name=SteadfastGame", "https://twitter.com/SteadfastGame"));
    }

    void ShowMenu()
    {
        GoogleAnalytics.SafeLogScreen("menu-show");
        EventManager.TriggerPause();
        TweenPosition.Begin(menuGroup, 0.4f, Vector3.zero);
    }

    void HideMenu(GameObject go)
    {
        GoogleAnalytics.SafeLogScreen("menu-hide");
        EventManager.TriggerResume();
        TweenPosition.Begin(menuGroup, 0.4f, hidePosition);
    }

}
