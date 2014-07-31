using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Hooks up analytics to game events.
/// We should try to hook analytics to events as much as possible
/// rather than putting them inline.
/// </summary>
public class Analytics
{
    public static void Initialize()
    {
        GoogleAnalytics.SafeLogScreen("game-start");

        EventManager.SongSwitched += (song) => {
            GoogleAnalytics.SafeLogScreen("song-switched");
            GoogleAnalytics.SafeLogScreen("song-play-" + song.songId);
        };
        EventManager.DemoSongStart += (instruments, stageContainers) => {
            GoogleAnalytics.SafeLogScreen("demo-btn-clicked");
        };


        EventManager.Drop += (GameObject container, GameObject instrument) => {
            GoogleAnalytics.SafeLogScreen("instrument-drop" + instrument.name);
            GoogleAnalytics.SafeLogScreen("instrument-drop-" + instrument.name);
        };

        EventManager.Pause += () => GoogleAnalytics.SafeLogScreen("menu-show");
        EventManager.Resume += () => GoogleAnalytics.SafeLogScreen("menu-hide");

        EventManager.Highlight += (HighlightManager.InstrumentType type) => {
            GoogleAnalytics.SafeLogScreen("highlight");
            GoogleAnalytics.SafeLogScreen("highlight-" + type.ToString());
        };

        EventManager.TutorialCompleted += () => GoogleAnalytics.SafeLogScreen("tutorial-completed");
    }
}
