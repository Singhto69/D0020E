using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{

    public List<string> words = new List<string>();
    public List<int> scorePWord = new List<int>();
    public int scoreTot = 0;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
