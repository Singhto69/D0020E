using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterBehaviour : MonoBehaviour
{
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //If sword hit me, kill me.
        Destroy(gameObject);
    }

}
