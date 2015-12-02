using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Linq;

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

	private System.Diagnostics.Process _showIpProcess;

	public static SocketClient GetInstance()
	{
		if (_instance == null)
			_instance = new SocketClient();
		return _instance;
	}
	
	private SocketClient()
	{
		System.Diagnostics.Process process = new System.Diagnostics.Process
		{
			StartInfo =
			{
				FileName = "netsh.exe",
				Arguments = "wlan show hostednetwork",
				UseShellExecute = false,
				RedirectStandardOutput = true,
				CreateNoWindow = true
			}
		};
		process.Start();

		string output = process.StandardOutput.ReadToEnd ();

		//string ssid = output.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault(l => l.Contains("SSID") && !l.Contains("BSSID")).Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries)[1].TrimStart();

		try {
			output.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault(l => l.Contains("Canal")).Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries)[1].TrimStart();
		}
		catch {
			//if (!ssid.Contains ("Out Of Body")) {
				new System.Diagnostics.Process {
					StartInfo =
					{
						WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
						FileName = "cmd.exe",
						Arguments = "/c netsh wlan set hostednetwork mode=allow ssid=\"Out Of Body\" key=outofbody && netsh wlan start hostednetwork"
					}
				}.Start ();
			//}
		}

		IPAddress ipAddress = IPAddress.Parse(Utils.SERVER_IP);
		_remoteEP = new IPEndPoint(ipAddress, Utils.SOCKET_PORT);

		_process = new System.Diagnostics.Process () {
			StartInfo = 
			{
				FileName = "cmd.exe",
				Arguments = "/c node ..\\ApplicationMenu\\server\\server.js",
				WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden
			}
		};

		_socket = new Socket (_remoteEP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
		try {
			_socket.Connect (_remoteEP);
			new Thread (() => Receive()).Start ();
		} catch (Exception) {
			_socket.Close();
			new Thread (() => LaunchThreadConnect()).Start ();
		}
		_showIpProcess = System.Diagnostics.Process.Start ("ShowIp");
	}

	private void LaunchThreadConnect() {
		Connect ();
		new Thread (() => Receive()).Start ();
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

	private void StopNodeServer() {
		foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcessesByName("node")) {
			p.Kill ();
		}
	}

	public void StopAllProcess(){
		StopNodeServer ();
		if(_showIpProcess != null && !_showIpProcess.HasExited)
			_showIpProcess.Kill ();
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
				_socket.Close();
				if(!_stopThread) {
					Connect ();
				}
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