using System;
using System.IO;
using System.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.Storage.Table;
using Subscriber.Models;

namespace Subscriber
{
	public class Functions
	{
		public static void ProcessStep1(
			[ServiceBusTrigger(nameof(Step1))] Step1 step1,
			[ServiceBus(nameof(Step2))] out BrokeredMessage output,
			TextWriter log)
		{
			var step2 = new Step2 {Number = 1};
			output = BrokeredMessageFactory.CreateBrokeredMessage(step2);
		}

		public static void ProcessStep2(
			[ServiceBusTrigger(nameof(Step2))] Step2 step2,
			[ServiceBus(nameof(Step3))] out BrokeredMessage step3Message,
			[Table("Customers")] CloudTable tableBinding,
			TextWriter log)
		{
			var step3 = new Step3 { BlobId = Guid.NewGuid() };
			step3Message = BrokeredMessageFactory.CreateBrokeredMessage(step3);

			var customer = new Customer
			{
				PartitionKey = step3.BlobId.ToString(),
				RowKey = 1.ToString(),
				Name = "Darren", Email = "darrenh@digiterre.com"
			};
			TableOperation operation = TableOperation.InsertOrReplace(customer);
			var result = tableBinding.Execute(operation);
		}

		public static void ProcessStep3(
			[ServiceBusTrigger(nameof(Step3))] Step3 step3,
			[Table("Customers")] IQueryable<Customer> tableBinding,
			TextWriter log)
		{
			var customers = tableBinding.Where(c => string.Equals(c.PartitionKey, step3.BlobId.ToString())).ToList();
			log.WriteLine($"Got {customers.Count()} customers");
		}
	}
}