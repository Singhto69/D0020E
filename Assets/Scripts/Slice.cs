using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slice : MonoBehaviour
{
    public bool isSlicing = false;
    float[] lastPos = new float[] {0.0f, 0.0f};
    public float threshold = 7.0f;
    public GameObject trail;

    void FixedUpdate()
    {
        float diffX = Mathf.Abs(Input.mousePosition.x - lastPos[0])/Screen.width;
        float diffY = Mathf.Abs(Input.mousePosition.y - lastPos[1])/Screen.height;
        float Pytsen = Mathf.Sqrt(Mathf.Pow(diffX, 2) + Mathf.Pow(diffY, 2))*100;
        //Debug.Log((int)Pytsen);
        if (Pytsen > threshold)
        {
            isSlicing = true;
        }
        else
        {
            isSlicing = false;
        }

        lastPos[0] = Input.mousePosition.x;
        lastPos[1] = Input.mousePosition.y;

    }

    void Update()
    {
        trail.SetActive(isSlicing);
        Vector3 newPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(Camera.main.transform.position.z - transform.position.z)));
        newPos.z = 0;


        trail.transform.position = newPos;
    }
}
