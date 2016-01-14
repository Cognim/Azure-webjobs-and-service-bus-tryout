using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace Subscriber.Models
{
	[Serializable]
	public class Customer : TableEntity
	{
		public string Name { get; set; }
		public string Email { get; set; }
	}
}