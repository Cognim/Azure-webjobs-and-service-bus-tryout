using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Subscriber.Models;

namespace Subscriber
{
	class Program
	{
		private static string _servicesBusConnectionString;
		private static NamespaceManager namespaceManager;

		static void Main()
		{
			_servicesBusConnectionString = AmbientConnectionStringProvider.Instance.GetConnectionString(ConnectionStringNames.ServiceBus);
			namespaceManager = NamespaceManager.CreateFromConnectionString(_servicesBusConnectionString);

			if (!namespaceManager.QueueExists(nameof(Step1)))
			{
				namespaceManager.CreateQueue(nameof(Step1));
			}
			if (!namespaceManager.QueueExists(nameof(Step2)))
			{
				namespaceManager.CreateQueue(nameof(Step2));
			}

			JobHostConfiguration config = new JobHostConfiguration();
			config.UseServiceBus();



			var host = new JobHost(config);

			CreateStartMessage();

			host.RunAndBlock();
		}

		private static void CreateStartMessage()
		{
			QueueClient client1 = QueueClient.CreateFromConnectionString(_servicesBusConnectionString,nameof(Step1));
			//QueueClient client2 = QueueClient.CreateFromConnectionString(_servicesBusConnectionString, "test2");

			//var message2 = new BrokeredMessage(new Step2 { Number = 1 });
			//message2.Properties["Event"] = nameof(Step2);
			//client2.Send(message2);

			var step1 = new Step1 {Message = "Hello"};
			var message = BrokeredMessageFactory.CreateBrokeredMessage(step1);

			client1.Send(message);
		}
	}
}
