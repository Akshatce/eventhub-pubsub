# Connect AEH  & publish events

```markdown
# Azure Event Hub Configuration and Event Publishing

## 1. Configuring Connection to Azure Event Hub

This snippet sets up the necessary configuration to connect to your Azure Event Hub. The `EventHubsConnectionStringBuilder` is used to specify the connection string and Event Hub name.

```csharp
// Azure Event Hub connection settings
private const string EventHubConnectionString = "<Your-Connection-String>"; // Replace with your Event Hub connection string
private const string EventHubName = "<Your-Event-Hub-Name>"; // Replace with your Event Hub name
private const string partitionId = "1"; // Optional: Target a specific partition (or leave it blank for auto-partitioning)

// Create Event Hub client
var connectionStringBuilder = new EventHubsConnectionStringBuilder(EventHubConnectionString)
{
    EntityPath = EventHubName // Specifies the Event Hub to connect to
};
var eventHubClient = EventHubClient.CreateFromConnectionString(connectionStringBuilder.ToString());
```

### Explanation:
- Replace `<Your-Connection-String>` and `<Your-Event-Hub-Name>` with your Event Hub credentials.
- The `partitionId` is optional and allows targeting a specific partition if required. Without it, Azure automatically manages the distribution.

---

## 2. Preparing and Serializing Data

This snippet serializes custom data objects into JSON format, which is then sent as events.

```csharp
List<TrainData> trainDataList = PrepareData.CreateTelemetryData();

// Example: Serializing data into JSON format
string json = JsonConvert.SerializeObject(trainData); // Convert data object to JSON format

// Create EventData from JSON string
var eventData = new EventData(Encoding.UTF8.GetBytes(json));
```

### Explanation:
- `trainDataList` contains the list of telemetry data to be sent to the Event Hub. Replace this with your own data generation or preparation logic.
- `JsonConvert.SerializeObject` converts the data object into JSON format for transmission.
- `EventData` wraps the JSON payload and sends it using the Event Hub client.

---

## 3. Sending Events to Event Hub

This snippet shows how to loop through the prepared events and send them to the Event Hub. It also handles user input for the number of events to send.

```csharp
await eventHubClient.SendAsync(eventData, partitionId); // Send event to Event Hub
```

### Explanation:
- Events are sent asynchronously using the `SendAsync` method.

---

## 4. Closing the Client

After publishing all events, the Event Hub client is closed to release resources.

```csharp
finally
{
    await eventHubClient.CloseAsync(); // Ensure the Event Hub client is properly closed
    Console.WriteLine("Event Hub client closed."); // Log client closure
}
```

### Explanation:
- Closing the client is essential to free up resources and prevent memory leaks.
- Use `finally` to guarantee the client closure, even if exceptions occur.

---

## 5. Putting It All Together

This snippet combines all the above steps into the main entry point of the program.

```csharp
static async Task Main(string[] args)
{
    Console.WriteLine("Azure Event Hub Publisher");

    // Initialize Event Hub Client
    var connectionStringBuilder = new EventHubsConnectionStringBuilder(EventHubConnectionString)
    {
        EntityPath = EventHubName
    };
    var eventHubClient = EventHubClient.CreateFromConnectionString(connectionStringBuilder.ToString());

    try
    {
        Console.WriteLine("Enter the number of events to send:");
        int eventCount = int.Parse(Console.ReadLine());

        // Prepare data and send
        List<TrainData> trainDataList = PrepareData.CreateTelemetryData(); // Replace with your data preparation logic
        foreach (var trainData in trainDataList)
        {
            string json = JsonConvert.SerializeObject(trainData);
            var eventData = new EventData(Encoding.UTF8.GetBytes(json));
            await eventHubClient.SendAsync(eventData, partitionId);
        }

        Console.WriteLine($"{eventCount} events successfully sent!");
    }
    finally
    {
        await eventHubClient.CloseAsync(); // Clean up the client
    }
}
```

### Explanation:
- This is the program's main entry point, tying together all steps: initialization, data preparation, sending events, and cleanup.
- Replace placeholder methods like `PrepareData.CreateTelemetryData` with your actual data preparation logic.