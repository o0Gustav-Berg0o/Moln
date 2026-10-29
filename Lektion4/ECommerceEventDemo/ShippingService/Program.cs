using Azure.Messaging.ServiceBus;
using Shared;
using System.Diagnostics;
using System.Text.Json;

Console.WriteLine("===  Shipping Service ===\n");

// TODO: Klistra in din connection string här
var connectionString = "KLISTRA-IN-DIN-CONNECTION-STRING-HÄR";
var topicName = "order-events";
var subscriptionName = "shipping";

// TODO: Skapa ServiceBusClient
var client = new ServiceBusClient(connectionString);


// TODO: Skapa ServiceBusProcessor
var processor = client.CreateProcessor(topicName, subscriptionName, new ServiceBusProcessorOptions
{
    MaxConcurrentCalls = 1,
    AutoCompleteMessages = false
});


// TODO: Hantera ProcessMessageAsync event
processor.ProcessMessageAsync += async args =>
{
    try
    {
        // TODO: Deserialisera meddelandet
        var body = args.Message.Body.ToString();
        var orderEvent = JsonSerializer.Deserialize<OrderCreatedEvent>(body);

        if (orderEvent == null)
        {
            Console.WriteLine(" Kunde inte deserialisera meddelande");
            await args.DeadLetterMessageAsync(args.Message);
            return;
        }

        //  Output är redan klar - du behöver bara få orderEvent att fungera!
        Console.WriteLine("\n==========================================");
        Console.WriteLine("          FRAKTBOKNING");
        Console.WriteLine("==========================================");
        Console.WriteLine($"Order ID:    #{orderEvent.OrderId}");
        Console.WriteLine($"Kund:        {orderEvent.CustomerName}");
        Console.WriteLine($"Adress:      Exempelgatan 1, Stockholm");
        Console.WriteLine($"Paket:       {orderEvent.ProductNames.Count} produkter");

        // Generera tracking number baserat på OrderId
        var trackingNumber = $"TRACK-{DateTime.Now.Year}-{orderEvent.OrderId:D6}";
        Console.WriteLine($"Spårningsnr: {trackingNumber}");

        // Estimerad leverans (3 dagar framåt)
        var estimatedDelivery = orderEvent.CreatedAt.AddDays(3);
        Console.WriteLine($"Estimerad leverans: {estimatedDelivery:yyyy-MM-dd} (3 dagar)");
        Console.WriteLine("==========================================");
        Console.WriteLine(" Frakt bokad med PostNord!");
        Console.WriteLine();

        // TODO: Markera meddelandet som klart
        await args.CompleteMessageAsync(args.Message);
    }
    catch (Exception ex)
    {
        Console.WriteLine($" Fel vid processning: {ex.Message}");
        await args.AbandonMessageAsync(args.Message);
    }
};

// TODO: Hantera ProcessErrorAsync event
processor.ProcessErrorAsync += args =>
{
    Console.WriteLine($" Error: {args.Exception.Message}");
    return Task.CompletedTask;
};


// TODO: Starta processor
await processor.StartProcessingAsync();


Console.WriteLine(" Shipping Service lyssnar på 'shipping' subscription...");
Console.WriteLine("Väntar på OrderCreated events...");
Console.WriteLine("Tryck Enter för att stoppa...\n");
Console.ReadLine();

// TODO: Stoppa processor
await processor.StopProcessingAsync();


Console.WriteLine(" Shipping Service avstängd!");
