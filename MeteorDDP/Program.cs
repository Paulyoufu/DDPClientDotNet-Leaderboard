using Net.DDP.Client;
using System;

namespace MeteorDDP
{
	public class Program
	{
		public static void Main(string[] args)
		{
			IDataSubscriber subscriber = new Subscriber();
			var client = new DDPClient(subscriber);

			client.Connect("localhost:3000");
			client.Subscribe("players");

			while (true)
			{
				Console.ReadLine();
				client.Call("updatePerson", "yzzXrXXrb6Cxu95pg");	// id of "Claude Shannon"
			}
		}

		public class Subscriber : IDataSubscriber
		{
			public void DataReceived(dynamic data)
			{
				if (data.type == "sub")
				{
					Console.WriteLine(data.prodCode + ": " + data.prodName + ": collection: " + data.collection);
				}
			}

			public void DataReceived(string data)
			{
				Console.WriteLine(data);
			}
		}	
	}
}