using System;
using System.Text;

namespace Net.DDP.Client
{
	public class DDPClient : IClient
	{
		private const string DDPVERSION = "1";
		private readonly DDPConnector _connector;
		private int _uniqueId;
		private readonly ResultQueue _queueHandler;

		public DDPClient(IDataSubscriber subscriber)
		{
			_connector = new DDPConnector(this, DDPVERSION);
			_queueHandler = new ResultQueue(subscriber);
			_uniqueId = 1;
		}

		public void AddItem(string jsonItem)
		{
			_queueHandler.AddItem(jsonItem);
		}

		public void Connect(string url)
		{
			_connector.Connect(url);
		}

		public void Call(string methodName, params string[] args)
		{
			string message = string.Format("\"msg\": \"method\",\"method\": \"{0}\",\"params\": [{1}],\"id\": \"{2}\"", methodName, CreateJSonArray(args), this.NextId().ToString());
			message = "{" + message + "}";
			_connector.Send(message);
		}

		public int Subscribe(string subscribeTo, params string[] args)
		{
			var message = string.Format("\"msg\": \"sub\",\"name\": \"{0}\",\"params\": [{1}],\"id\": \"{2}\"", subscribeTo, CreateJSonArray(args), NextId());
			message = "{" + message + "}";
			_connector.Send(message);

			Console.WriteLine(message);

			return GetCurrentRequestId();
		}

		private static string CreateJSonArray(params string[] args)
		{
			if (args == null)
				return string.Empty;

			var argumentBuilder = new StringBuilder();
			var delimiter = string.Empty;
			foreach (var token in args)
			{
				argumentBuilder.Append(delimiter);
				argumentBuilder.Append(string.Format("\"{0}\"", token));
				delimiter = ",";
			}

			return argumentBuilder.ToString();
		}
		private int NextId()
		{
			return _uniqueId++;
		}

		public int GetCurrentRequestId()
		{
			return _uniqueId;
		}

	}
}
