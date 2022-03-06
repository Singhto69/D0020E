using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    public Text CountdownText;
    public float timeLeft = 4;
    public GameObject Timer;
    public GameObject LetterController;
    private bool notDone = true;
    // Update is called once per frame
    void Start()
    {
        Timer.GetComponent<Timer>().enabled = false;
        LetterController.GetComponent<LetterController>().enabled = false;
    }

    void FixedUpdate()
    {
        if (notDone) { 
        if(timeLeft > 1)
        {
            timeLeft -= Time.fixedDeltaTime;
            CountdownText.text = ((int)timeLeft).ToString();
        }
        else
        {
            CountdownText.text = "";
            Timer.GetComponent<Timer>().enabled = true;
            LetterController.GetComponent<LetterController>().enabled = true;
            notDone = false;
        }
        }
    }
}
