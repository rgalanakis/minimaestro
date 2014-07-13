using UnityEngine;
using System.Collections;
using System;

public class AccountSettings : MonoBehaviour
{
    public static bool completedTutorial = false;
    public event Action CompletedTutorialEvent;
    void Awake()
    {      
        //PlayerPrefs.DeleteAll()
        LoadSettings();
    }

    public void LoadSettings()
    {
        //completedTutorial = PlayerPrefs.GetInt("completedTutorial", 0);
    }

    public void SaveSettings()
    {
        //PlayerPrefs.SetInt("completedTutorial", completedTutorial);
    }

    public void CompletedTutorial()
    {
        completedTutorial = true;
        if (CompletedTutorialEvent != null)
        {
            CompletedTutorialEvent();
        }
    }
}
