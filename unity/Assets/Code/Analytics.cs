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
    private const int InstrumentCount = 11;

    private static bool initialized = false;

    public static void Initialize()
    {
        if (initialized)
        {
            return;
        }
        initialized = true;

        GoogleAnalytics.SafeLogScreen("game-start");
        
        int songSwitchCnt = 0;
        EventManager.SongSwitched += (song) => {
            // Avoid logging the first song switch that happens when the game starts.
            if (songSwitchCnt > 0)
            {
                GoogleAnalytics.SafeLogScreen("song-switched");
            }
            songSwitchCnt += 1;
            GoogleAnalytics.SafeLogScreen("song-play-" + song.songId);
        };

        // When the demo button is clicked, we get drop events for every instrument.
        // We do NOT want to record these as normal drops.
        // So when the demo button is clicked, we ignore the next 11 (InstrumentCount)
        // drops, since they are all fired from the demo change.
        // We use the same system and vars to ignore the drops that happen
        // when the game is started.
        int dropCount = 0;
        EventManager.DemoSongStart += (instruments, stageContainers) => {
            dropCount = 0;
            GoogleAnalytics.SafeLogScreen("demo-btn-clicked");
        };

        EventManager.Drop += (GameObject container, GameObject instrument) => {
            // See comments above about why we noop in some cases.
            dropCount += 1;
            if (dropCount <= InstrumentCount)
            {
                return;
            }
            GoogleAnalytics.SafeLogScreen("instrument-drop");
            if (container.tag == "stage_grid")
            {
                GoogleAnalytics.SafeLogScreen("instrument-onstage-" + instrument.name);
            }
            else
            {
                GoogleAnalytics.SafeLogScreen("instrument-offstage-" + instrument.name);
            }
        };

        EventManager.Pause += () => GoogleAnalytics.SafeLogScreen("menu-show");
        EventManager.Resume += () => GoogleAnalytics.SafeLogScreen("menu-hide");

        EventManager.Highlight += (HighlightManager.InstrumentType type) => {
            GoogleAnalytics.SafeLogScreen("highlight");
            GoogleAnalytics.SafeLogScreen("highlight-" + type.ToString().ToLower());
        };

        EventManager.TutorialCompleted += () => GoogleAnalytics.SafeLogScreen("tutorial-completed");
    }
}
