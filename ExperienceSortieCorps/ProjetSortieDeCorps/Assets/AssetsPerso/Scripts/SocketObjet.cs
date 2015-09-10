using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class SocketObjet : MonoBehaviour {

	public Vector3 initPos;
	public Vector3 oldPosition = Vector3.zero;
	public Vector3 newPosition = Vector3.zero;
	private struct ReceiveOperationInfo { public UdpClient msgUDPClient; public IPEndPoint msgIPEndPoint;}
	Vector3 tmp = Vector3.zero;
	public Vector3 move = Vector3.zero;
	private Queue<float> X;
	private Queue<float> Y;
	private Queue<float> Z;
	Thread receiveData;
	UdpClient client;
	int port = 5005;
	String text = " ";
	public bool kinectIsLeft = true;
	// Use this for initialization
	void Start () {
		X = new Queue<float> ();
		Y = new Queue<float> ();
		Z = new Queue<float> ();
		client = new UdpClient (port);
		ReceiveOperationInfo msgInfo = new ReceiveOperationInfo ();
		msgInfo.msgUDPClient = client;
		msgInfo.msgIPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"),port);
		client.BeginReceive (new AsyncCallback (ReceiveDataAsync), msgInfo);
		/*receiveData = new Thread (new ThreadStart (Receive));
		receiveData.IsBackground = true;
		receiveData.Start ();*/
	}
	
	// Update is called once per frame
	void Update () {
		//angle = atan (y2 - y1 / x2 - x1)
		if (Input.GetKeyDown (KeyCode.I)) {
			/*Vector3 tmp = GameObject.FindGameObjectWithTag("Avatar").transform.position;
			new Vector3 (-tmp.z,tmp.y,tmp.x);*/
			if(kinectIsLeft){
				initPos = GameObject.FindGameObjectWithTag("Avatar").transform.position + new Vector3(0.80f,0.6f,1.25f);
				gameObject.transform.position = GameObject.FindGameObjectWithTag("Avatar").transform.position + new Vector3(0.80f,0.6f,1.25f);
			}else{
				initPos = GameObject.FindGameObjectWithTag("Avatar").transform.position + new Vector3(-0.80f,0.6f,-3.33f);
				gameObject.transform.position = GameObject.FindGameObjectWithTag("Avatar").transform.position + new Vector3(-0.80f,0.6f,-3.33f);
			}
			/*initPos = GameObject.FindGameObjectWithTag("Avatar").transform.position;
			gameObject.transform.position = GameObject.FindGameObjectWithTag("Avatar").transform.position;*/
			gameObject.GetComponentInChildren<MeshRenderer>().enabled = true;
		}
		if(!tmp.Equals(newPosition)){
			oldPosition = newPosition;
			newPosition = tmp;
			Vector3 mvt = newPosition - oldPosition;

			float z = 1.0f/((newPosition.x/100.0f)* -0.0030711016f) ;
			float x = z *2* (float)Math.Tan(1.0821/2)*(newPosition.z/640);
			float y = z *2* (float)Math.Tan(0.8423/2)*(newPosition.y/480);

			//float z = newPosition.x/1000.0f;
			//float x = (newPosition.z-640/2) * (z - 10) * 0.0021f;
			//float y = (newPosition.y-480/2) * (z - 10) * 0.0021f;
			/*X.Enqueue (x);
			Y.Enqueue (y);
			Z.Enqueue (z);
			
			if (X.Count > 5) {
				X.Dequeue ();
				Y.Dequeue ();
				Z.Dequeue ();
			}

			x = ExponentialMovingAverage (X.ToArray (), 0.9);
			y = ExponentialMovingAverage (Y.ToArray (), 0.9);
			z = ExponentialMovingAverage (Z.ToArray(),0.9);*/
			//move = new Vector3(mvt.z*0.1f, mvt.y*0.1f, mvt.x*1f);
			//gameObject.transform.Translate (-x*0.0000f, -y*0.005f, z*0.005f);
			//gameObject.transform.position = Vector3.Lerp(gameObject.transform.position,new Vector3(x,y,z),0.5f);
			//gameObject.transform.position = new Vector3(-x,-y,z);
			//gameObject.transform.position = newPosition*0.1f;
			//gameObject.transform.Translate (-mvt.z*0.0025f, -mvt.y*0.0025f, mvt.x*0.001f);
			//gameObject.transform.Translate (move);
			//gameObject.transform.Translate (z*1f, y*1f, x*5.0f);
			if(kinectIsLeft){
				gameObject.transform.position = initPos+new Vector3(x*0.025f,y*0.025f,0.025f*z);
			}else{
				gameObject.transform.position = initPos+new Vector3(-x*0.01f,y*0.01f,0.01f*z);
			}
			//Debug.Log (newPosition.x);
			//move = new Vector3(-x*0.1f,-y*0.1f,2.0f*z);
			//gameObject.transform.position = initPos+new Vector3(-mvt.z*0.0025f,-mvt.y*0.0025f,mvt.x*0.001f);
			//Debug.Log(gameObject.transform.position);
			//gameObject.transform.position = Vector3.Lerp(gameObject.transform.position,new Vector3(-x*0.5f,-y*0.5f,z),0.5f);
		}
	}

	private void ReceiveDataAsync(IAsyncResult msgResult){
		IPEndPoint anyIP = new IPEndPoint(IPAddress.Parse("127.0.0.1"),port);
		byte [] msg = client.EndReceive (msgResult,ref anyIP);
		String text = Encoding.UTF8.GetString (msg);
		String[] coordonees = text.Split(':');
		tmp = new Vector3(float.Parse(coordonees[2]),float.Parse(coordonees[1]),float.Parse(coordonees[0]));
		ReceiveOperationInfo msgInfo = new ReceiveOperationInfo ();
		msgInfo.msgUDPClient = client;
		msgInfo.msgIPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"),port);
		client.BeginReceive (new AsyncCallback (ReceiveDataAsync), msgInfo);
	}

	/*private void Receive(){
	//	client = new UdpClient (port);
		//Debug.Log ("Debut Thread");
		//Debug.Log ("Connection 127.0.0.1 :"+port);
		while(true){
			try{
				IPEndPoint anyIP = new IPEndPoint(IPAddress.Parse("127.0.0.1"),port);
				//Debug.Log ("Attente données ...");
				byte[] data = client.Receive(ref anyIP);
				//Debug.Log ("Données reçus ...");
				String text = Encoding.UTF8.GetString(data);
				String[] t = text.Split(':');
				tmp = new Vector3(int.Parse(t[2]),int.Parse(t[1]),int.Parse(t[0]));

				//Debug.Log ("X : " + tmp.x + "Y :" + tmp.y + "X :" + tmp.z);
			}catch(Exception e){
				print (e.ToString());
			}
		}
	}*/

	private void moveStick (String position){
		String[] coordonees = position.Split(':');
		tmp = new Vector3(int.Parse(coordonees[2]),int.Parse(coordonees[1]),int.Parse(coordonees[0]));

		if(!tmp.Equals(newPosition)){
			oldPosition = newPosition;
			newPosition = tmp;
			Vector3 mvt = newPosition - oldPosition;
			X.Enqueue (mvt.x);
			Y.Enqueue (mvt.y);
			Z.Enqueue(mvt.z);
			
			if (X.Count > 5) {
				X.Dequeue ();
				Y.Dequeue ();
				Z.Dequeue ();
			}
			float x = ExponentialMovingAverage (X.ToArray (), 0.9);
			float y = ExponentialMovingAverage (Y.ToArray (), 0.9);
			float z = ExponentialMovingAverage (Z.ToArray(),0.9);
			gameObject.transform.Translate (-mvt.x*0.0000f, -mvt.y*0.005f, -mvt.z*0.005f);
			
		}
	}
	void OnApplicationQuit(){
		client.Close();
		/*if (receiveData != null) {
			receiveData.Abort ();
		}*/
	}

	public float ExponentialMovingAverage( float[] data, double baseValue )
	{
		float numerator = 0;
		float denominator = 0;
		float average = sum(data);
		average /= data.Length;
		
		for ( int i = 0; i < data.Length; ++i )
		{
			numerator += data[i] * (float)Math.Pow( baseValue, data.Length - i - 1 );
			denominator += (float)Math.Pow( baseValue, data.Length - i - 1 );
		}
		
		numerator += average * (float)Math.Pow( baseValue, data.Length );
		denominator += (float)Math.Pow( baseValue, data.Length );
		
		return (numerator / denominator);
	}
	
	public float sum(float[] data){
		float sum = 0f;
		foreach (float value in data) {
			sum += value;
		}
		return sum;
	}
}
