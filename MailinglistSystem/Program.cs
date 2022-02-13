using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Shared;

namespace MailinglistSystem
{
    internal class Program
    {
        static string connectionString = "Endpoint=sb://skysdemo1.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=DqlDmk6TjnPP6zQm+Ol7/GiujryzDp2CvgohVHCFqL4=";

        // name of your Service Bus queue
        static string queueName = "test";

        // the client that owns the connection and can be used to create senders and receivers
        static ServiceBusClient client;

        // the sender used to publish messages to the queue
        static ServiceBusProcessor processor;
        // number of messages to be sent to the queue
        private const int numOfMessages = 3;


        static async Task MessageHandler(ProcessMessageEventArgs args)
        {

            var c = args.Message.Body.ToObjectFromJson<CustomerCreated>();
            //agera...

            string body = args.Message.Body.ToString();
            Console.WriteLine($"Received: {body}");

            // complete the message. message is deleted from the queue. 
            await args.CompleteMessageAsync(args.Message);
        }

        // handle any errors when receiving messages
        static Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

        static async Task Main(string[] args)
        {
            
            client = new ServiceBusClient(connectionString);
            processor = client.CreateProcessor(queueName, new ServiceBusProcessorOptions());

            try
            {
                // add handler to process messages
                processor.ProcessMessageAsync += MessageHandler;

                // add handler to process any errors
                processor.ProcessErrorAsync += ErrorHandler;

                // start processing 
                await processor.StartProcessingAsync();

                Console.WriteLine("Wait for a minute and then press any key to end the processing");
                Console.ReadKey();

                // stop processing 
                Console.WriteLine("\nStopping the receiver...");
                await processor.StopProcessingAsync();
                Console.WriteLine("Stopped receiving messages");
            }
            finally
            {
                // Calling DisposeAsync on client types is required to ensure that network
                // resources and other unmanaged objects are properly cleaned up.
                await processor.DisposeAsync();
                await client.DisposeAsync();
            }

        }
    }
}
