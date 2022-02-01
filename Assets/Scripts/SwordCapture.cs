using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class SwordCapture : MonoBehaviour
{
	Thread receiveThread; //1
	UdpClient client; //2
	int port = 5065; //3

	// Start is called before the first frame update
	void Start()
    {
		print("UDP Initialized");

		receiveThread = new Thread(new ThreadStart(ReceiveData));
		receiveThread.IsBackground = true;
		receiveThread.Start();
	}


	// 4. Receive Data
	private void ReceiveData()
	{
		client = new UdpClient(port); //1
		while (true) //2
		{
			try
			{
				IPEndPoint anyIP = new IPEndPoint(IPAddress.Parse("0.0.0.0"), port); //3
				byte[] data = client.Receive(ref anyIP); //4

				string text = Encoding.UTF8.GetString(data); //5
				print(">> " + text);

			}
			catch (Exception e)
			{
				print(e.ToString()); //7
			}
		}
	}

	// Update is called once per frame
	void Update()
    {
        
    }
}
