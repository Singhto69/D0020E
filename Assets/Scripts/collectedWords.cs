using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Collections;

public class collectedWords : MonoBehaviour
{
    private GameObject DataOBJ;

    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("Content").GetComponent<Text>().text = "";
        DataOBJ = GameObject.Find("DataOBJ");

        for (int i = 0; i < DataOBJ.GetComponent<GameData>().words.Count; i++)
        {
            GameObject.Find("Content").GetComponent<Text>().text += DataOBJ.GetComponent<GameData>().words[i] + "\n";
            GameObject.Find("Content").GetComponent<Text>().text += DataOBJ.GetComponent<GameData>().scorePWord[i] + " poäng \n \n";
        }
        Destroy(DataOBJ);
    }

}
