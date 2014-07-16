using UnityEngine;
using System.Collections;

public class MenuScreen : MonoBehaviour
{
    public GameObject playButton;
    public GameObject rateUsButton;
    public GameObject getHelpButton;
    public GameObject facebookButton;
    public GameObject twitterButton;
    bool leftApp = false;

    void Start()
    {
        UIEventListener.Get(playButton).onClick += OnButtonPlay;
        UIEventListener.Get(rateUsButton).onClick += OnButtonRateUs;
        UIEventListener.Get(getHelpButton).onClick += OnButtonGetHelp;
        UIEventListener.Get(facebookButton).onClick += OnButtonFacebook;
        UIEventListener.Get(twitterButton).onClick += OnButtonTwitter;

    }

    void OnDestroy()
    {
        NGUIHelper.RemoveClickEventListener(playButton, OnButtonPlay);
        NGUIHelper.RemoveClickEventListener(rateUsButton, OnButtonRateUs);
        NGUIHelper.RemoveClickEventListener(getHelpButton, OnButtonGetHelp);
        NGUIHelper.RemoveClickEventListener(facebookButton, OnButtonFacebook);
        NGUIHelper.RemoveClickEventListener(twitterButton, OnButtonTwitter);
    }

    void OnButtonPlay(GameObject go)
    {
        Application.LoadLevel("game");
    }

    void OnButtonRateUs(GameObject go)
    {
        if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor)
        {
            return;
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

    void OnApplicationPause(bool inIsPause)
    {
        leftApp = true;
    }

    void OnButtonGetHelp(GameObject go)
    {
        Application.OpenURL("http://minimaestrogame.herokuapp.com/");
    }

    void OnButtonFacebook(GameObject go)
    {
        StartCoroutine(OpenSocialPage("fb://profile/443299165761335", "https://www.facebook.com/SteadfastGames"));
    }

    void OnButtonTwitter(GameObject go)
    {
        StartCoroutine(OpenSocialPage("twitter:///user?screen_name=SteadfastGame", "https://twitter.com/SteadfastGame"));
    }
}
