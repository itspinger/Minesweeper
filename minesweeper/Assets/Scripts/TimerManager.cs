using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using TMPro;

public class TimerManager : MonoBehaviour
{
    private static TimerManager instance;
    private readonly Stopwatch timer = new Stopwatch();

    public TMP_Text timeText;
    public TMP_Text flagText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else
        {
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        if (Game.instance == null)
        {
            return;
        }

        // Get the number of mines
        // In this game
        int mineCount = Game.instance.GetMineCount();

        // Check how many fields are flagged
        int flags = Game.instance.GetFlaggedMines();

        // Set the text
        flagText.text = (mineCount - flags).ToString();

        if (!timer.IsRunning)
        {
            return;
        }

        // Get the current time and apply it
        string time = FormatTime((int) timer.Elapsed.TotalSeconds);
        timeText.SetText(time);
    }

    /**
     * This method starts the current timer.
     */

    public void StartTimer()
    {
        timer.Start();
    }

    /**
     * This method restarts the current timer.
     */

    public void StopTimer()
    {
        timer.Reset();
    }

    public void ResetTimer()
    {
        // First stop the timer
        StopTimer();

        // Now replace the text
        timeText.SetText(FormatTime(0));
    }

    /**
     * This method is used to format an integer between
     * to a string which will have 3 characters at all times.
     */

    private string FormatTime(int time)
    {
        return Mathf.Clamp(time, 0, 999).ToString("000");
    }

    /**
     * This method returns the current instance
     * of the timer manager type.
     */

    public static TimerManager GetInstance()
    {
        return instance;
    }


}
