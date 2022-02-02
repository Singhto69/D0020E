 using System.Collections;
 using System.Collections.Generic;
 using System;
 using System.Net;
 using System.Net.Sockets;
 using System.IO;
 using System.Text;
 using System.Threading;
 using UnityEngine;




public class networkHost : MonoBehaviour
 {
     public string ip;
     public IPAddress hostaddress; 
     public int port = 5065;
     public IPEndPoint ipep;
     public IPEndPoint sender;
     public UdpClient newSock;
     public Thread networkThread;
 
     public byte[] receivedData;
     public string dataString;
 
 
     void Start()
     {
         networkThread = new Thread(ReceiveData);
         networkThread.Start();
 
         receivedData = new byte[0];
 
         ipep = new IPEndPoint(IPAddress.Any, port);
         newSock = new UdpClient(ipep);
         sender = new IPEndPoint(IPAddress.Any, 0);
     }
 
     void Update()
     {
         ReceiveData();
     }
 
     public void ReceiveData()
     {
         if (newSock.Available > 0)
         {
             receivedData = newSock.Receive(ref sender);
             dataString = Encoding.ASCII.GetString(receivedData);
            print(dataString);
         }
         else
         {
             print("No data to receive");
         }
     }
 
 }