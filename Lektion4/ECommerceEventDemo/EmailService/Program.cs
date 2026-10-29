using Azure.Messaging.ServiceBus;
using System.Text.Json;
using Shared;

Console.WriteLine("=== 📧 Email Service ===\n");

// VIKTIGT: Byt ut denna connection string!
var connectionString = "KLISTRA-IN-DIN-CONNECTION-STRING-HÄR";
var topicName = "order-events";
var subscriptionName = "email";

// Skapa Service Bus client och processor
var client = new ServiceBusClient(connectionString);
var processor = client.CreateProcessor(topicName, subscriptionName, new ServiceBusProcessorOptions
{
    MaxConcurrentCalls = 1,
    AutoCompleteMessages = false
});

// Hantera meddelanden
processor.ProcessMessageAsync += async args =>
{
    try
    {
        // Deserialisera meddelandet
        var body = args.Message.Body.ToString();
        var orderEvent = JsonSerializer.Deserialize<OrderCreatedEvent>(body);

        if (orderEvent == null)
        {
            Console.WriteLine(" Kunde inte deserialisera meddelande");
            await args.DeadLetterMessageAsync(args.Message);
            return;
        }

        // Visa email (simulerat)
        Console.WriteLine("\n==========================================");
        Console.WriteLine("            EMAIL");
        Console.WriteLine("==========================================");
        Console.WriteLine($"Till: {orderEvent.CustomerName.ToLower().Replace(" ", ".")}@email.com");
        Console.WriteLine($"Ämne: Orderbekräftelse #{orderEvent.OrderId}");
        Console.WriteLine();
        Console.WriteLine($"Hej {orderEvent.CustomerName}!");
        Console.WriteLine();
        Console.WriteLine($"Tack för din order på totalt {orderEvent.TotalAmount:N2} kr.");
        Console.WriteLine();
        Console.WriteLine("Dina produkter:");
        foreach (var product in orderEvent.ProductNames)
        {
            Console.WriteLine($"  • {product}");
        }
        Console.WriteLine();
        Console.WriteLine("Vi behandlar din order nu!");
        Console.WriteLine($"Order skapad: {orderEvent.CreatedAt:yyyy-MM-dd HH:mm:ss}");
        Console.WriteLine("==========================================");
        Console.WriteLine(" Email skickat!");
        Console.WriteLine();

        // Markera meddelandet som klart
        await args.CompleteMessageAsync(args.Message);
    }
    catch (Exception ex)
    {
        Console.WriteLine($" Fel vid processning: {ex.Message}");
        // Låt meddelandet gå tillbaka till kön för retry
        await args.AbandonMessageAsync(args.Message);
    }
};

// Hantera fel
processor.ProcessErrorAsync += args =>
{
    Console.WriteLine($" Error: {args.Exception.Message}");
    return Task.CompletedTask;
};

// Starta processor
await processor.StartProcessingAsync();

Console.WriteLine(" Email Service lyssnar på 'email' subscription...");
Console.WriteLine("Väntar på OrderCreated events...");
Console.WriteLine("Tryck Enter för att stoppa...\n");
Console.ReadLine();

// Stoppa processor
Console.WriteLine("\n  Stoppar Email Service...");
await processor.StopProcessingAsync();
await processor.DisposeAsync();
await client.DisposeAsync();

Console.WriteLine(" Email Service avstängd!");
