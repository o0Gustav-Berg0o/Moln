using Azure.Messaging.ServiceBus;
using Shared;
using System.Diagnostics;
using System.Text.Json;

Console.WriteLine("=== 📄 Invoice Service ===\n");

// TODO: Klistra in din connection string här
var connectionString = "KLISTRA-IN-DIN-CONNECTION-STRING-HÄR";
var topicName = "order-events";
var subscriptionName = "invoice";

// TODO: Skapa ServiceBusClient
var client = new ServiceBusClient(connectionString);


// TODO: Skapa ServiceBusProcessor
var processor = client.CreateProcessor(topicName, subscriptionName, new ServiceBusProcessorOptions
{
    MaxConcurrentCalls = 1,
    AutoCompleteMessages = false
});


// TODO: Hantera ProcessMessageAsync event
// processor.ProcessMessageAsync += async args =>
// {
//     Här ska du:
//     1. Hämta message body: args.Message.Body.ToString()
//     2. Deserialisera till OrderCreatedEvent
//     3. Visa output (nedan är redan klar!)
//     4. Anropa CompleteMessageAsync
// };

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
        Console.WriteLine("            FAKTURA");
        Console.WriteLine("==========================================");
        Console.WriteLine($"Order ID:       #{orderEvent.OrderId}");
        Console.WriteLine($"Kund:           {orderEvent.CustomerName}");
        Console.WriteLine($"Totalbelopp:    {orderEvent.TotalAmount:N2} kr");
        Console.Write($"Produkter:      ");
        Console.WriteLine(string.Join(", ", orderEvent.ProductNames));
        Console.WriteLine($"Fakturadatum:   {orderEvent.CreatedAt:yyyy-MM-dd HH:mm:ss}");
        Console.WriteLine("==========================================");
        Console.WriteLine(" Faktura skapad och sparad!");
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


Console.WriteLine(" Invoice Service lyssnar på 'invoice' subscription...");
Console.WriteLine("Väntar på OrderCreated events...");
Console.WriteLine("Tryck Enter för att stoppa...\n");
Console.ReadLine();

// TODO: Stoppa processor
await processor.StopProcessingAsync();


Console.WriteLine(" Invoice Service avstängd!");
