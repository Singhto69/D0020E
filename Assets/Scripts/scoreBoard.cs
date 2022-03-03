using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Collections;

public class scoreBoard : MonoBehaviour
{

    public InputField nameInputField;
    private GameObject DataOBJ;

    static int maxPos = 5;
    static List<string> topNames = new List<string>();
    static List<int> topScores = new List<int>();
    string name = "";
    int score = 0;
    bool newHighScore = false;
    int arrPos = 0;
    public static string input;

    GameObject InputFieldGameObject;

    InputField inputScoreField;

    string textOfField;
    Button savebtn;
    Button mainMenubtn;

    public bool isStart = false;

    void FixedUpdate()
    {
        if(newHighScore)
        {
            GameObject.Find("Pos" + (arrPos + 1)).GetComponent<Text>().text = inputScoreField.text;
            GameObject.Find("Pos" + (arrPos + 1)).GetComponent<Text>().color = Color.red;
            GameObject.Find("score" + (arrPos + 1)).GetComponent<Text>().text = topScores[arrPos].ToString();
            GameObject.Find("score" + (arrPos + 1)).GetComponent<Text>().color = Color.red;
        }
    }


    void TaskOnClick()
    {
        Debug.Log("You have clicked the button!");
        savebtn.interactable = false;
        inputScoreField.interactable = false;
        if(newHighScore)
        {
            topNames[arrPos] = inputScoreField.text;
            GameObject.Find("Pos" + (arrPos + 1)).GetComponent<Text>().text = inputScoreField.text;
            GameObject.Find("Pos" + (arrPos + 1)).GetComponent<Text>().color = Color.red;
            GameObject.Find("score" + (arrPos + 1)).GetComponent<Text>().text = topScores[arrPos].ToString();
            GameObject.Find("score" + (arrPos + 1)).GetComponent<Text>().color = Color.red;
        }
        
        writeToFile();
        topNames.Clear();
        topScores.Clear();
    }


    void Start()
    {
        DataOBJ = GameObject.Find("DataOBJ");

        score = DataOBJ.GetComponent<GameData>().scoreTot;
        savebtn = GameObject.Find("saveScoreButton").GetComponent<Button>();
        savebtn.onClick.AddListener(TaskOnClick);

        //mainMenubtn = GameObject.Find("mainMenubtn").GetComponent<Button>();
        //mainMenubtn.onClick.AddListener(TaskOnClick2);

        inputScoreField = GameObject.Find("InputScoreField").GetComponent<InputField>();
        savebtn.interactable = false;
        inputScoreField.interactable = false;

        readFile();

        int lastPosition = topScores.Count - 1;
        int lowestScore = 0;
        if (topScores.Count > 1)
        {
            lowestScore = (int)topScores[lastPosition];
        }
        if (score > lowestScore || topScores.Count < 1 || lastPosition < maxPos-1)
        {
            savebtn.interactable = true;
            inputScoreField.interactable = true;
            newHighScore = true;
            if(newHighScore)
            {
                GameObject.Find("Congratulations").GetComponent<Text>().text = "Grattis, dina poäng är bland de fem bästa!";
                if (lastPosition + 1 < maxPos)
                {
                    topScores.Add(score);
                    topNames.Add(name);
                }
                else
                {
                    topScores[lastPosition] = score;
                    topNames[lastPosition] = name;
                }
                bubbleSort(topNames, topScores);
            }  
        }


        bool found = false;

        for (int i = 0; i < topNames.Count; i++)
        {
            print("name = " + topNames[i]);
            GameObject.Find("Pos" + (i + 1)).GetComponent<Text>().text = topNames[i] + " : " + topScores[i];

            if (newHighScore && (score == topScores[i]))
                {
                    arrPos = i;
                }
            GameObject.Find("Pos" + (i + 1)).GetComponent<Text>().text = topNames[i];
            GameObject.Find("score" + (i + 1)).GetComponent<Text>().text = topScores[i].ToString();

        }

    }
    public void readFile() {   
        
            string path = @"Assets\score.txt";

            if (File.Exists(path))
            {
                string[] lines = System.IO.File.ReadAllLines(path);
                for(int i = 0; i < lines.Length; i++) {
                    if(i % 2 == 0) {
                        topNames.Add(lines[i]);
                    }
                    else {
                        topScores.Add(int.Parse(lines[i]));
                    }
                }
            }
    }

    public void writeToFile() {
        try
        {
            print("name2 = " + topNames[0]);
            print("score2 = " + topScores[0]);
            string path = @"Assets\score.txt";
            string nameScore = "";
            print("count = " + topScores.Count);
            for(int i = 0; i < topScores.Count; i++) {
                nameScore += topNames[i] + Environment.NewLine + topScores[i].ToString() + Environment.NewLine;
            }
            print(nameScore);
            File.WriteAllText(path, nameScore);
        }
        catch(Exception e)
        {
            Debug.Log("Exception: " + e.Message);
        }
        finally
        {
            Debug.Log("Executing finally block, write.");
        }

    }


    /*
    * Sort descending
    */
    public void bubbleSort(List<string> topNames, List<int> topScores) {
        int tempScore;
        string tempName;
        for (int j = 0; j <= topScores.Count - 2; j++) {
            for (int i = 0; i <= topScores.Count - 2; i++) {
                if ((int)topScores[i] < (int)topScores[i + 1]) {
                    tempScore = (int)topScores[i + 1];
                    tempName = (string)topNames[i + 1];
                    topScores[i + 1] = topScores[i];
                    topNames[i + 1] = topNames[i];
                    topScores[i] = tempScore;
                    topNames[i] = tempName;
                }
            }
        }
    }

   

}