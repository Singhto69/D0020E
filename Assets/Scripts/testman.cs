using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testman : MonoBehaviour
{
    public GameObject netmanobject;
    public int[] clist;

    void Start()
    {
        //clist = netmanobject.GetComponent<netMan>().coordList;
    }

    // Update is called once per frame
    void Update()
    {
        clist = netmanobject.GetComponent<netMan>().coordList;
    }
}
