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

		public static string message;

		public static SocketClient GetInstance()
		{
			if (_instance == null) _instance = new SocketClient();
			return _instance;
		}
		
		private SocketClient()
		{
			_thread = new Thread(new ThreadStart(Receive));
			_thread.Start();
		}
		
		private static void Receive()
		{
			IPAddress ipAddress = IPAddress.Parse(Utils.SERVER_IP);
			IPEndPoint remoteEP = new IPEndPoint(ipAddress, Utils.SERVER_PORT);
			Socket socket = new Socket(remoteEP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
			socket.Connect(remoteEP);
			
			int bytes = 0;
			Byte[] bytesReceived = new Byte[256];
			
			if (socket == null) throw new Exception("Impossible de connecter la socket.");
			
			do
			{
				try {
					bytes = socket.Receive(bytesReceived, bytesReceived.Length, 0);
					message = Encoding.ASCII.GetString(bytesReceived, 0, bytes);
				} catch(SocketException) {
					_thread.Interrupt();
					System.Environment.Exit(1);
				}
			}
			while (bytes > 0);
		}
	}
}