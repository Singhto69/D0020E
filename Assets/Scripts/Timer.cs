using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public float timeRemaining = 90;
    public bool timerIsRunning = false;
    public Text timeText;
    public GameObject LetterController;
    private bool cd2change = false;
    private void OnEnable()
    {
        // Starts the timer automatically
        timerIsRunning = true;
    }
    void FixedUpdate()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                timerIsRunning = false;
                LetterController.GetComponent<LetterController>().enabled = false;
                if (cd2change)
                {
                    SceneManager.LoadScene("Scoreboard");
                }
                else
                {
                    cd2change = true;
                    timeRemaining = 3;
                    timerIsRunning = true;
                }
            }
        }
    }
    void DisplayTime(float timeToDisplay)
    {
        if(!cd2change)
        {
            timeToDisplay += 1;
            float minutes = Mathf.FloorToInt(timeToDisplay / 60);
            float seconds = Mathf.FloorToInt(timeToDisplay % 60);
            timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        else
        {
            timeText.text = "";
        }
    }
}