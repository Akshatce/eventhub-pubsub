using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.EventHubs;

namespace EventHubPublisher
{
    class Publish
    {
        // Azure Event Hub connection settings
        private const string EventHubConnectionString = "connection-string";
        private const string EventHubName = "traintracer";

        static async Task Main(string[] args)
        {
            Console.WriteLine("Azure Event Hub Publisher");

            // Create Event Hub client
            var connectionStringBuilder = new EventHubsConnectionStringBuilder(EventHubConnectionString)
            {
                EntityPath = EventHubName
            };

            var eventHubClient = EventHubClient.CreateFromConnectionString(connectionStringBuilder.ToString());

            try
            {
                Console.WriteLine("Enter the number of events to send:");
                int eventCount = int.Parse(Console.ReadLine());

                for (int i = 1; i <= eventCount; i++)
                {
                    string message = $"Event {i} at {DateTime.Now}";
                    Console.WriteLine($"Sending: {message}");

                    var eventData = new EventData(Encoding.UTF8.GetBytes(message));
                    await eventHubClient.SendAsync(eventData);
                }

                Console.WriteLine($"{eventCount} events successfully sent!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                await eventHubClient.CloseAsync();
            }
        }
    }
}
