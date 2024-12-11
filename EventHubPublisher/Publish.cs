using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.EventHubs;
using Newtonsoft.Json;

namespace EventHubPublisher
{
    class Publish
    {
        // Azure Event Hub connection settings
        private const string EventHubConnectionString = "";
        private const string EventHubName = "";
        private const string partitionId = "1";

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

                List<TrainData> trainDataList =  PrepareData.CreateTelemetryData();

                

                foreach (var trainData in  trainDataList)
                {
                   
                    Console.WriteLine($"Sending: {trainData}");
                    string json = JsonConvert.SerializeObject(trainData);

                    var eventData = new EventData(Encoding.UTF8.GetBytes(json));
                    await eventHubClient.SendAsync(eventData, partitionId);
                }

                Console.WriteLine($"{eventCount} events successfully sent!");
                int eventCount1 = int.Parse(Console.ReadLine());
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
