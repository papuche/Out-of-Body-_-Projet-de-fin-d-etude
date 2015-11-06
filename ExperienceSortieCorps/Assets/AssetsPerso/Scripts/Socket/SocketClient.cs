using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace AssemblyCSharp
{
	public class SocketClient
	{
		private static SocketClient _instance;
		private Thread _thread;
		private Socket _socket;
		
		private Boolean _stopThread;
		
		private string _message;

		private IPEndPoint _remoteEP;

		private int _nbTryReconnection = 0;

		public static SocketClient GetInstance()
		{
			if (_instance == null)
				_instance = new SocketClient();
			return _instance;
		}
		
		private SocketClient()
		{
			_stopThread = false;
			IPAddress ipAddress = IPAddress.Parse(Utils.SERVER_IP);
			_remoteEP = new IPEndPoint(ipAddress, Utils.SERVER_PORT);
			
			_socket = new Socket (_remoteEP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
			try {
				_socket.Connect (_remoteEP);
				new Thread (new ThreadStart (Receive)).Start ();
			} catch (Exception e) {
				_socket.Close();
				new Thread (new ThreadStart (LaunchThreadConnect)).Start ();
			}
		}

		private void Connect(){
			if(_nbTryReconnection > 60) {
				// Redémarrer serveur node
				_nbTryReconnection = 0;
			}
			while (!_socket.Connected && !_stopThread) {
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
}