using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSlice : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if(GameObject.FindWithTag("SliceCheck").GetComponent<Slice>().isSlicing == true)
        {
            Debug.Log("ouch");
        }
    }
}
