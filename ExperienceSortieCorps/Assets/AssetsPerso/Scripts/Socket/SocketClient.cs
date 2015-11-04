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
		private static Thread _thread;
		private static Socket _socket;

		public static string message;

		public static SocketClient GetInstance()
		{
			if (_instance == null) _instance = new SocketClient();
			return _instance;
		}
		
		private SocketClient()
		{
			IPAddress ipAddress = IPAddress.Parse(Utils.SERVER_IP);
			IPEndPoint remoteEP = new IPEndPoint(ipAddress, Utils.SERVER_PORT);
			_socket = new Socket(remoteEP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
			_socket.Connect(remoteEP);

			if (_socket == null) throw new Exception("Impossible de connecter la socket.");
			_thread = new Thread(new ThreadStart(Receive));
			_thread.Start();
		}
		
		private static void Receive()
		{
			int bytes = 0;
			Byte[] bytesReceived = new Byte[256];
			do
			{
				try {
					bytes = _socket.Receive(bytesReceived, bytesReceived.Length, 0);
					message = Encoding.ASCII.GetString(bytesReceived, 0, bytes);
				} catch(SocketException) {
					_thread.Interrupt();
					System.Environment.Exit(1);
				}
			}
			while (bytes > 0);
		}

		public void Write(String message) {
			_socket.Send (System.Text.Encoding.ASCII.GetBytes(message));
		}
	}
}