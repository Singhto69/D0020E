using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LetterBehaviour : MonoBehaviour
{
    private Camera cam;
    private GameObject SliceDetector;

    private void Start()
    {
        cam = Camera.main;
        SliceDetector = GameObject.FindWithTag("SliceCheck");
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //If sword hit me, kill me.
        if(GameObject.FindWithTag("SliceCheck").GetComponent<Slice>().isSlicing)
        {
            string let = (gameObject.GetComponent<Text>().text);
            LetterController.getLet(let);

            FindObjectOfType<AudioManager>()?.Play("LetterBreak");

            Destroy(gameObject);
        }
    }

}
