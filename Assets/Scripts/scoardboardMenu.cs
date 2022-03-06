using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Collections;

public class scoardboardMenu : MonoBehaviour
{

    static List<string> topNames = new List<string>();
    static List<int> topScores = new List<int>();

    Button mainMenubtn;

    void Start()
    {

        mainMenubtn = GameObject.Find("mainMenubtn").GetComponent<Button>();
        readFile();


        for (int i = 0; i < topNames.Count; i++)
        {
            GameObject.Find("Pos" + (i + 1)).GetComponent<Text>().text = topNames[i];
            GameObject.Find("score" + (i + 1)).GetComponent<Text>().text = topScores[i].ToString();
        }
        topNames.Clear();
        topScores.Clear();

    }
    public void readFile()
    {

        string path = @"Assets\score.txt";

        if (File.Exists(path))
        {
            string[] lines = System.IO.File.ReadAllLines(path);
            for (int i = 0; i < lines.Length; i++)
            {
                if (i % 2 == 0)
                {
                    topNames.Add(lines[i]);
                }
                else
                {
                    topScores.Add(int.Parse(lines[i]));
                }
            }
        }
    }
}
