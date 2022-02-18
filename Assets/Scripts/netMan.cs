 using System.Collections;
 using System.Collections.Generic;
 using System;
 using System.Net;
 using System.Net.Sockets;
 using System.IO;
 using System.Text;
 using System.Threading;
 using UnityEngine;




public class netMan : MonoBehaviour
 {
     public string ip;
     public IPAddress hostaddress; 
     public int port = 5065;
     public IPEndPoint ipep;
     public IPEndPoint sender;
     public UdpClient newSock;
     public Thread networkThread;

     public GameObject dot;
     public Transform prefab;

     //public Boolean rec;
     public int[] coordList;
     public byte[] receivedData;
     public string dataString;
 
    public string testString;

    public GameObject Sphere;

     void Start()
     {
         networkThread = new Thread(ReceiveData);
         networkThread.Start();
        testString = "305:112:0:0:";
        coordList = new int[4];
         receivedData = new byte[0];
        
         ipep = new IPEndPoint(IPAddress.Any, port);
         newSock = new UdpClient(ipep);
         sender = new IPEndPoint(IPAddress.Any, 0);
     }
 
     void Update()
     {
        
        //coordList = getCoords(testString);
        //getCoords(testString);
         //ReceiveData();
     }
 
     public void ReceiveData()
     {
        List<int>templist = new List<int>();
         if (newSock.Available > 0)
         {
            receivedData = newSock.Receive(ref sender);
            dataString = Encoding.ASCII.GetString(receivedData);
            getCoords(dataString);
    
         }
         else
         {
            print("No data to receive");
            
         }
     }

    public void getCoords(String dataString){
        //int? intbuilder = null;
        int tempint = 0;
        int count = 0;
        int loopint;
        foreach (char c in dataString){
            if(c == char.Parse(":")){
                coordList[count] = tempint;
                count = count + 1;
                tempint = 0;
                if (count == 4){
                    return;
                }
            }
            else{
                loopint = (int)Char.GetNumericValue(c);
                tempint = int.Parse(tempint.ToString() + loopint.ToString());
            }
        }
            //tempbox.Add((xnull ?? -1));
        return;
    }
 
 }