using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using System;
using System.Threading.Tasks;

class EventConsumer
{
    private const string connectionString = "<Your-Event-Hub-Connection-String>";
    private const string eventHubName = "<Your-Event-Hub-Name>";
    private const string consumerGroup = EventHubConsumerClient.DefaultConsumerGroupName;
    private const string partitionId = "0";

    static async Task Main(string[] args)
    {
        await using var consumerClient = new EventHubConsumerClient(consumerGroup, connectionString, eventHubName);

        Console.WriteLine($"Starting to consume events from partition {partitionId}...");

        int eventCount = 0;

        await foreach (PartitionEvent partitionEvent in consumerClient.ReadEventsFromPartitionAsync(
            partitionId,
            EventPosition.Earliest))
        {
            Console.WriteLine($"Event Received: {partitionEvent.Data.EventBody}");

            eventCount++;
            if (eventCount >= 10) // Stop after receiving 10 events
            {
                break;
            }
        }

        Console.WriteLine("Test complete.");
    }
}
