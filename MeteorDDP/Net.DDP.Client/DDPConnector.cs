using SuperSocket.ClientEngine;
using System;
using System.Threading;
using WebSocket4Net;

namespace Net.DDP.Client
{
// ReSharper disable once InconsistentNaming
	internal class DDPConnector
	{
		private WebSocket _socket;
		private string _url = string.Empty;
		private readonly IClient _client;
		private readonly string _version;

		private readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(false);

		public DDPConnector(IClient client, string version)
		{
			_client = client;
			_version = version;
		}

		public void Connect(string url)
		{
			_url = "ws://" + url + "/websocket";

			_socket = new WebSocket(_url);
			_socket.Opened += _socket_Opened;
			_socket.Error += _socket_Error;
			_socket.Closed += _socket_Closed;
			_socket.MessageReceived += socket_MessageReceived;
			_socket.Open();
		}

		private static void _socket_Closed(object sender, EventArgs e)
		{
			Console.WriteLine(e);
		}

		private static void _socket_Error(object sender, ErrorEventArgs e)
		{
			Console.WriteLine(e.Exception);
		}

		public void Close()
		{
			_socket.Close();
		}

		public void Send(string message)
		{
			_socket.Send(message);
		}

		private void _socket_Opened(object sender, EventArgs e)
		{
			var message = string.Format("{{\"msg\":\"connect\",\"version\":\"{0}\",\"support\":[\"{0}\"]}}", _version);

			Console.WriteLine(message);

			Send(message);
		}

		void socket_MessageReceived(object sender, MessageReceivedEventArgs e)
		{
			_client.AddItem(e.Message);

			_autoResetEvent.Set();
		}
	}
}
