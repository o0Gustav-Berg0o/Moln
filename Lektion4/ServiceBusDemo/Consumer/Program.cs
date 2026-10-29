using Azure.Messaging.ServiceBus;

// SAMMA CONNECTION STRING!
var connectionString = "KLISTRA-IN-DIN-CONNECTION-STRING-HÄR";
var queueName = "orders";

// Skapa Service Bus client
await using var client = new ServiceBusClient(connectionString);

// Skapa processor (consumer)
var processor = client.CreateProcessor(queueName, new ServiceBusProcessorOptions
{
    MaxConcurrentCalls = 1,
    AutoCompleteMessages = false
});

// Hantera meddelanden
processor.ProcessMessageAsync += async args =>
{
    var body = args.Message.Body.ToString();
    Console.WriteLine($"\n Mottaget meddelande:");
    Console.WriteLine($"   {body}");

    // Simulera processing
    await Task.Delay(1000);

    // Markera som klar
    await args.CompleteMessageAsync(args.Message);
    Console.WriteLine("    Processad!");
};

// Hantera errors
processor.ProcessErrorAsync += args =>
{
    Console.WriteLine($" Error: {args.Exception.Message}");
    return Task.CompletedTask;
};

// Starta!
await processor.StartProcessingAsync();

Console.WriteLine(" Lyssnar på meddelanden...");
Console.WriteLine("Tryck Enter för att stoppa...\n");
Console.ReadLine();

await processor.StopProcessingAsync();
