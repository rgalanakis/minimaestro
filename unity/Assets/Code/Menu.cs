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
    public UILabel holdText;
    bool rateButtonDown;
    bool facebookButtonDown;
    bool twitterButtonDown;
    // Make the overlay larger than the screen to avoid alpha at the edges.
    private const int underlayFudgePixels = 80;
    float timer;

    void Awake()
    {

        UIEventListener.Get(menuButton).onClick += OnButtonPressMenu;
        UIEventListener.Get(facebookButton).onPress += OnFacebookPress;
        UIEventListener.Get(twitterButton).onPress += OnTwitterPress;
        UIEventListener.Get(reviewButton).onPress += OnReviewPress;
        UIEventListener.Get(underlay.gameObject).onClick += HideMenu;
        EventManager.MenuHide += MenuHide;
    }

    void MenuHide()
    {
        hidePosition = new Vector3(-(underlay.drawingDimensions.z * 2), 0.0f, 0.0f);
        TweenPosition.Begin(menuGroup, 0.0f, hidePosition);

    }
    void OnDestroy()
    {
        EventManager.MenuHide -= MenuHide;
        NGUIHelper.RemoveClickEventListener(menuButton, OnButtonPressMenu);
        NGUIHelper.RemovePressEventListener(facebookButton, OnFacebookPress);
        NGUIHelper.RemovePressEventListener(twitterButton, OnTwitterPress);
        NGUIHelper.RemovePressEventListener(reviewButton, OnReviewPress);
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
        if (rateButtonDown || twitterButtonDown || facebookButtonDown)
        {
            timer += Time.deltaTime;
            holdText.gameObject.SetActive(true);
        }
        else
        {
            timer = 0.0f;
            holdText.gameObject.SetActive(false);
        }

        if (timer >= 3.0f)
        {
            rateButtonDown = false;
            twitterButtonDown = false;
            facebookButtonDown = false;
            timer = 0.0f;
            if (facebookButtonDown)
            {
                GoogleAnalytics.SafeLogScreen("facebook-btn-clicked");
                StartCoroutine(OpenSocialPage("fb://profile/443299165761335", "https://www.facebook.com/SteadfastGames")); 
            }
            else if (twitterButtonDown)
            {
                GoogleAnalytics.SafeLogScreen("twitter-btn-clicked");
                StartCoroutine(OpenSocialPage("twitter:///user?screen_name=SteadfastGame", "https://twitter.com/SteadfastGame"));
            }
            else
            {
                GoogleAnalytics.SafeLogScreen("review-btn-clicked");
                UniRate.Instance.RateIfNetworkAvailable();
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

    void OnReviewPress(GameObject go, bool pressed)
    {
        if (Application.isEditor)
        {
            return;
        }
        rateButtonDown = pressed;
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

    void OnFacebookPress(GameObject go, bool pressed)
    {
        facebookButtonDown = pressed;
    }

    void OnTwitterPress(GameObject go, bool pressed)
    {
        twitterButtonDown = pressed;
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
