using UnityEngine;
using System.Diagnostics;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    private readonly Stopwatch timer = new Stopwatch();

    public TMP_Text timeText;
    public TMP_Text flagText;

    public TMP_Text scoreText;
    public TMP_Text recordText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else
        {
            Destroy(this.gameObject);
        }

        Game.GetInstance().OnWin.AddListener(() => UpdateTime());
    }

    private void Update()
    {
        if (Game.GetInstance() == null)
        {
            return;
        }

        // Get the number of mines
        // In this game
        int mineCount = Game.GetInstance().GetMineCount();

        // Check how many fields are flagged
        int flags = Game.GetInstance().GetFlaggedMines();

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
     * This method updates the record time 
     * on the game finish.
     */

    private void UpdateTime()
    {
        // Update the score text
        scoreText.text = timeText.text;

        // In order to see if it is a new record
        // We need to check for the differences in text
        try
        {
            if (int.Parse(recordText.text) > int.Parse(scoreText.text))
            {
                recordText.text = scoreText.text;
            }
        }
        catch (Exception) { 
        }
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

    /**
     * This method restarts the current timer,
     * and also resets the time text to 0.
     */

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

    public static GameManager GetInstance()
    {
        return instance;
    }


}
