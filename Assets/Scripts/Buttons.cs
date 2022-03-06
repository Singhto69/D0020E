using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{

    private GameObject thisMenu;
    public GameObject otherMenu;
    public GameObject buttonz;
    public bool isStart = false;
    public string sceneToLoad = "";

    void Start()
    {
        thisMenu = this.gameObject;
        var b = buttonz.GetComponent<Button>();
        b.onClick.AddListener(knapp);
    }

    void knapp()
    {
        if (!isStart)
        {
            thisMenu.SetActive(false);
            otherMenu.SetActive(true);
        }
        else
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
