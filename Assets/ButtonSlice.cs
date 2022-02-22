using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSlice : MonoBehaviour
{



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(GameObject.FindWithTag("SliceCheck").GetComponent<Slice>().isSlicing == true)
        {
            Debug.Log("ouch");
        }
    }
}
