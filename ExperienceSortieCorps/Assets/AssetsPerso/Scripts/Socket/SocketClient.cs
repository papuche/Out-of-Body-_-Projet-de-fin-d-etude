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
		

		public static SocketClient GetInstance()
		{
			if (_instance == null) _instance = new SocketClient();
			return _instance;
		}
		
		private SocketClient()
		{
			_stopThread = false;
			
			IPAddress ipAddress = IPAddress.Parse(Utils.SERVER_IP);
			IPEndPoint remoteEP = new IPEndPoint(ipAddress, Utils.SERVER_PORT);
			
			_socket = new Socket (remoteEP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
			_socket.Connect (remoteEP);
			if (IsSocketConnected (_socket))
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
					_thread.Interrupt();
					System.Environment.Exit(1);
				}
			}
			while (!_stopThread);
		}
		
		public void Write(String message) {
			_socket.Send (System.Text.Encoding.ASCII.GetBytes(message));
		}

		private Boolean IsSocketConnected(Socket s)
		{
			return !((s.Poll (1000, SelectMode.SelectRead) && (s.Available == 0)) || !s.Connected);
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