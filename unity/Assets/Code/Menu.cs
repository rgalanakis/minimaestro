using UnityEngine;
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
    bool menuVisible;
    // Make the overlay larger than the screen to avoid alpha at the edges.
    private const int underlayFudgePixels = 80;

    void Awake()
    {
        hidePosition = new Vector3(-(underlay.drawingDimensions.z * 2), 0.0f, 0.0f);
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
        if (underlay != null)
        {
            NGUIHelper.RemoveClickEventListener(underlay.gameObject, HideMenu);
        } 
    }

    void Update()
    {
        if (menuVisible)
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                HideMenu(null);
            }
        }
    }

    void OnButtonPressMenu(GameObject go)
    {
        if (menuVisible)
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
        menuVisible = true;
        EventManager.TriggerPause();
        TweenPosition.Begin(menuGroup, 0.4f, Vector3.zero);
    }

    void HideMenu(GameObject go)
    {
        menuVisible = false;
        EventManager.TriggerResume();
        TweenPosition.Begin(menuGroup, 0.4f, hidePosition);
    }

}
