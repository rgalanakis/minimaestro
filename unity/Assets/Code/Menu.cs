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
    private Vector3 hidePosition = new Vector3(-1030, 0, 0);
    bool leftApp = false;
    void Awake()
    {
        hidePosition = new Vector3(-(Screen.width + 10.0f), 0.0f, 0.0f);
        background.SetDimensions(Screen.width + 15, Screen.height + 15);
        TweenPosition.Begin(menuGroup, 0.0f, hidePosition);
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

        GoogleAnalytics.SafeLogScreen("review-btn-clicked");

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
