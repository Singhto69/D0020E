using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Collections;

public class scoreBoard : MonoBehaviour
{
    static int top10 = 10;
    static List<string> topNames = new List<string>();
    static List<int> topScores = new List<int>();
    string name;
    int score;

    public scoreBoard(string name, int score) {
        this.name = name;
        this.score = score;
        readFile();
    }
    void Start()
    {
        readFile();
        for(int i = 0; i<5; i++)
        {
            if (i < topNames.Count)
            {
                GameObject.Find("Pos" + (i + 1)).GetComponent<Text>().text = topNames[i] + ": " + topScores[i];
            }
            else
            {
                GameObject.Find("Pos" + (i + 1)).GetComponent<Text>().text = "- : -";
            }
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
            string path = @"/Users/erik/Desktop/test.txt";
            string nameScore = "";

            for(int i = 0; i < topNames.Count; i++) {
                nameScore += topNames[i] + "\n" + topScores[i].ToString() + "\n";
            }
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

    public void checkNewRecord() {
        int lastPosition = topScores.Count-1;
        int lowestScore = 0;
        if(topScores.Count > 1) {
            lowestScore = (int)topScores[lastPosition];
        }
        if(score > lowestScore || topScores.Count < 1 || lastPosition < top10) {
            if(lastPosition + 1 < top10) {
                topScores.Add(score);
                topNames.Add(name);
            }
            else {
                topScores[lastPosition] = score;
                topNames[lastPosition] = name;
            }
            bubbleSort(topNames, topScores);
            writeToFile();
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