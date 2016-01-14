using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;

namespace Subscriber
{
	public class BrokeredMessageFactory
	{
		private const string EventProperty = "Event";

		public static BrokeredMessage CreateBrokeredMessage<T>(T wrappedMessage)
		{
			var message = new BrokeredMessage(wrappedMessage);
			message.Properties[EventProperty] = nameof(T);

			return message;
		}
	}
}
