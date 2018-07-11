using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace AzureQueueStorage
{
    class Program
    {
        static void Main(string[] args)
        {
            // Parse the connection string and return a reference to the storage account.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();


            // Retrieve a reference to a container.
            CloudQueue queue = queueClient.GetQueueReference("myqueue");

            // Create the queue if it doesn't already exist
            queue.CreateIfNotExists();
            // Peek at the next message
            CloudQueueMessage peekedMessage = queue.PeekMessage();

            // Display message.
            Console.WriteLine(peekedMessage.AsString);
            // Create a message and add it to the queue.
            for (int i = 0; i < 10; i++)
            {
                CloudQueueMessage message = new CloudQueueMessage("Hello, World"+i);
                queue.AddMessage(message);
            }

            var temp = queue.GetMessages(30);
            // Display message.
            foreach (var item in temp)
            {
                Console.WriteLine(item.AsString);
                queue.DeleteMessageAsync(item);
                // Async delete the message
                Console.WriteLine("Deleted message");
            }


            Console.ReadLine();
        }
    }
}
