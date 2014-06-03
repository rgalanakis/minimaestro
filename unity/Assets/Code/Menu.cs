using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour
{
    public GameObject menuButton;
    public UISprite blackOverlay;
    public UISprite timer;
    private float holdTime;
    public float holdTimeToMenu;
    private bool pressing;
    void Start()
    {
        blackOverlay.gameObject.SetActive(false);
        timer.gameObject.SetActive(false);
        holdTime = 0;
        blackOverlay.SetDimensions(Screen.width * 3, Screen.height * 3);
        UIEventListener.Get(menuButton).onPress += OnButtonPressMenu;
    }
    
    void OnDestroy()
    {
        NGUIHelper.RemovePressEventListener(menuButton, OnButtonPressMenu);
    }

    void FixedUpdate()
    {
        if (pressing)
        {
            holdTime += Time.deltaTime;
            timer.fillAmount = Mathf.Lerp(0, 1, holdTime / holdTimeToMenu);
            if (holdTime >= holdTimeToMenu)
            {
                LoadMenuScene();
            }
        }
    }

    void OnButtonPressMenu(GameObject go, bool press)
    {
        pressing = press;
        if (!press)
        {
            blackOverlay.gameObject.SetActive(false);
            timer.gameObject.SetActive(false);
            holdTime = 0;
            CancelInvoke("LoadMenuScene");
        }
        else
        {
            blackOverlay.gameObject.SetActive(true);
            timer.fillAmount = 0;
            timer.gameObject.SetActive(true);
        }
    }

    void LoadMenuScene()
    {
        pressing = false;
        Application.LoadLevel("menu");
    }

}
