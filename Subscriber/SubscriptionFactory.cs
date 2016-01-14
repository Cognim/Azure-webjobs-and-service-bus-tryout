using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Subscriber.Models;

namespace Subscriber
{
	public static class SubscriptionFactory
	{
		public const string Step1SubName = nameof(Step1);

		public static void CreateSubscription<T>(NamespaceManager namespaceManager, string topic)
		{
			var typeName = nameof(T);
			var subscriptionName = $"sub-{typeName}";
			if (!namespaceManager.SubscriptionExists(topic, subscriptionName))
			{
				SqlFilter filter = new SqlFilter($"Event LIKE '{typeName}'");
				namespaceManager.CreateSubscription(topic, subscriptionName, filter);
			}
		}
	}
}


// Extension method????????