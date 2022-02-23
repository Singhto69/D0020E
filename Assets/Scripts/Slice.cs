using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slice : MonoBehaviour
{
    private bool useMouse = false;
    public bool isSlicing = false;
    public netMan networkManager;
    private static Camera cam;
    float[] lastPos = new float[] {0.0f, 0.0f};
    public float threshold = 7.0f;
    public GameObject trail;

    void Start()
    {
        useMouse = GameObject.Find("Canvas").GetComponent<LetterController>().useMouse;
    }

    void FixedUpdate()
    {
        float diffX;
        float diffY;

        if (!useMouse)
        {
            networkManager.ReceiveData();
            Vector3 udppos = new Vector3((float)networkManager.coordList[0], (float)networkManager.coordList[1], 0.0f);
            diffX = Mathf.Abs(cam.ScreenToWorldPoint(udppos).x - lastPos[0]) / Screen.width;
            diffY = Mathf.Abs(cam.ScreenToWorldPoint(udppos).y - lastPos[1]) / Screen.height;
            lastPos[0] = cam.ScreenToWorldPoint(udppos).x;
            lastPos[1] = cam.ScreenToWorldPoint(udppos).y;
        }
        else
        {
            diffX = Mathf.Abs(Input.mousePosition.x - lastPos[0]) / Screen.width;
            diffY = Mathf.Abs(Input.mousePosition.y - lastPos[1]) / Screen.height;
            lastPos[0] = Input.mousePosition.x;
            lastPos[1] = Input.mousePosition.y;
        }
        float Pytsen = Mathf.Sqrt(Mathf.Pow(diffX, 2) + Mathf.Pow(diffY, 2)) * 100;
        isSlicing = Pytsen > threshold ? true : false;

    }

    void Update()
    {
        trail.SetActive(isSlicing);
        Vector3 newPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(Camera.main.transform.position.z - transform.position.z)));
        newPos.z = 0;


        trail.transform.position = newPos;
    }
}
