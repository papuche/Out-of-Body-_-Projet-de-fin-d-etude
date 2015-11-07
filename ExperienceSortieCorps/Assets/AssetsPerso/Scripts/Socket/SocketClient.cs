using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

	public class SocketClient
	{
		private static SocketClient _instance;
		private Thread _thread;
		private Socket _socket;
		
		private Boolean _stopThread = false;
		
		private string _message;

		private IPEndPoint _remoteEP;

		private int _nbTryReconnection = 0;

		private System.Diagnostics.Process _process;

		public static SocketClient GetInstance()
		{
			if (_instance == null)
				_instance = new SocketClient();
			return _instance;
		}
	
		private SocketClient()
		{	
			IPAddress ipAddress = IPAddress.Parse(Utils.SERVER_IP);
			_remoteEP = new IPEndPoint(ipAddress, Utils.SOCKET_PORT);

			string batDir = string.Format(@".");
			_process = new System.Diagnostics.Process();
			_process.StartInfo.WorkingDirectory = batDir;
			_process.StartInfo.FileName = "server.bat";
			_process.StartInfo.CreateNoWindow = true;
			_process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

			_socket = new Socket (_remoteEP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
			try {
				_socket.Connect (_remoteEP);
				new Thread (new ThreadStart (Receive)).Start ();
			} catch (Exception e) {
				_socket.Close();
				new Thread (new ThreadStart (LaunchThreadConnect)).Start ();
			}

			if (UnityEditor.EditorUtility.DisplayDialog ("Adresse IP de la machine", GetLocalIP() + ":" + Utils.SERVER_PORT, "Ouvrir la page", "Quitter"))
				Application.OpenURL("http://127.0.0.1:" + Utils.SERVER_PORT);
	}
	
	private String GetLocalIP() {
		string localIP = "?";
		foreach (IPAddress ip in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
		{
			if (ip.AddressFamily == AddressFamily.InterNetwork)
			{
				localIP = ip.ToString();
			}
		}
		return localIP;
	}

		private void Connect(){
			_process.Start();
			while (!_socket.Connected && !_stopThread) {
				if(_nbTryReconnection > 2) {
					_process.Kill();
					StopNodeServer();
					Thread.Sleep(2000);
					_process.Start();
					_nbTryReconnection = 0;
				}
				try {
					_socket = new Socket (_remoteEP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
					_socket.Connect (_remoteEP);
					_nbTryReconnection = 0;
				}
				catch {
					_socket.Close();
					Thread.Sleep (1000);
					_nbTryReconnection++;
				}
			}
		}

		public void StopNodeServer(){
			foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcessesByName("node")) {
				p.Kill();
			}
		}
	
		private void LaunchThreadConnect(){
			Connect ();
			new Thread (new ThreadStart (Receive)).Start ();
		}

			private void Receive()
		{
			int bytes = 0;
			Byte[] bytesReceived = new Byte[256];
			do
			{
				try {
					bytes = _socket.Receive(bytesReceived, bytesReceived.Length, 0);
					message = Encoding.ASCII.GetString(bytesReceived, 0, bytes);
					if(message.Equals(Utils.SOCKET_EXIT))
						_stopThread = true;
				} catch(SocketException) {
					/*_thread.Interrupt();
					System.Environment.Exit(1);*/
					_socket.Close();
					Connect ();
				}
			}
			while (!_stopThread);
		}
		
		public void Write(String message) {
			_socket.Send (System.Text.Encoding.ASCII.GetBytes(message));
		}
		
		public Socket socket {
			get {
				return _socket;
			}
			set {
				_socket = value;
			}
		}
		
		public Boolean stopThread {
			get {
				return _stopThread;
			}
			set {
				_stopThread = value;
			}
		}
		
		public String message {
			get {
				return _message;
			}
			set {
				_message = value;
			}
		}
	}