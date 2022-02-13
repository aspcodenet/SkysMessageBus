using System;
using Azure.Messaging.ServiceBus;
using Shared;

namespace MessageClient
{
    internal class Program
    {
        static string connectionString = "Endpoint=sb://skysdemo1.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=DqlDmk6TjnPP6zQm+Ol7/GiujryzDp2CvgohVHCFqL4=";

        // name of your Service Bus queue
        static string queueName = "test";

        // the client that owns the connection and can be used to create senders and receivers
        static ServiceBusClient client;

        // the sender used to publish messages to the queue
        static ServiceBusSender sender;

        // number of messages to be sent to the queue
        private const int numOfMessages = 3;

        static void Main(string[] args)
        {
            client = new ServiceBusClient(connectionString);
            sender = client.CreateSender(queueName);

            while (true)
            {
                Console.WriteLine("1. Create customer");
                Console.WriteLine("0. Exit");
                Console.Write(":>");
                var action =  Console.ReadLine();
                if (action == "0")
                    break;
                if (action == "1")
                {
                    Console.Write("Ange namn:");
                    string namn = Console.ReadLine();
                    Guid customerId = Guid.NewGuid();
                    //Save to database
                    sender.SendMessageAsync(new ServiceBusMessage(new BinaryData(new CustomerCreated
                    {
                        CustomerId = customerId,
                        DateTimeUtc = DateTime.UtcNow,
                        Name = namn
                    })));
                }

            }

        }
    }
}
