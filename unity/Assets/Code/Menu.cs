using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour
{
    public GameObject menuButton;

    void Start()
    {
        UIEventListener.Get(menuButton).onClick += OnButtonPressMenu;
    }
    
    void OnDestroy()
    {
        NGUIHelper.RemoveClickEventListener(menuButton, OnButtonPressMenu);
    }
        
    void OnButtonPressMenu(GameObject go)
    {
        LoadMenuScene();
    }

    void LoadMenuScene()
    {
        Application.LoadLevel("menu");
    }

}
