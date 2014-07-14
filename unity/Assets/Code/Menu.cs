using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour
{
    public GameObject menuButton;
    public GameObject menuGroup;
    public GameObject facebookButton;
    public GameObject twitterButton;
    public GameObject reviewButton;
    public UISprite background;
    public Vector3 hidePosition = new Vector3(-1030, 0, 0);
    bool leftApp = false;
    void Awake()
    {
        HideMenu(null);
        UIEventListener.Get(menuButton).onClick += OnButtonPressMenu;
        UIEventListener.Get(facebookButton).onClick += OnFacebookPress;
        UIEventListener.Get(twitterButton).onClick += OnTwitterPress;
        UIEventListener.Get(reviewButton).onClick += OnReviewPress;
        UIEventListener.Get(background.gameObject).onClick += HideMenu;

    }
    
    void OnDestroy()
    {
        NGUIHelper.RemoveClickEventListener(menuButton, OnButtonPressMenu);
    }
        
    void OnButtonPressMenu(GameObject go)
    {
        if (menuGroup.gameObject.transform.position.x == 0)
        {
            HideMenu(null);
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

        if (GoogleAnalytics.instance)
        {
            GoogleAnalytics.instance.LogScreen("Review Button Clicked");
        }

        UniRate.Instance.RateIfNetworkAvailable();
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
        if (GoogleAnalytics.instance)
        {
            GoogleAnalytics.instance.LogScreen("Facebook Button Clicked");
        }

        StartCoroutine(OpenSocialPage("fb://profile/443299165761335", "https://www.facebook.com/SteadfastGames")); 
    }

    void OnTwitterPress(GameObject go)
    {
        if (GoogleAnalytics.instance)
        {
            GoogleAnalytics.instance.LogScreen("Twitter Button Clicked");
        }
        StartCoroutine(OpenSocialPage("twitter:///user?screen_name=SteadfastGame", "https://twitter.com/SteadfastGame"));
    }

    void ShowMenu()
    {
        if (GoogleAnalytics.instance)
        {
            GoogleAnalytics.instance.LogScreen("Show Menu");
        }
        EventManager.TriggerPause();
        TweenPosition.Begin(menuGroup, 0.4f, Vector3.zero);
    }

    void HideMenu(GameObject go)
    {
        if (GoogleAnalytics.instance)
        {
            GoogleAnalytics.instance.LogScreen("Hide Menu");
        }
        EventManager.TriggerResume();
        TweenPosition.Begin(menuGroup, 0.4f, hidePosition);
    }

}
