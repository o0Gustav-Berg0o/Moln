using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using System.Text.Json;
using Shared;

Console.WriteLine("===  Startar Order Publisher ===\n");

// VIKTIGT: Byt ut denna connection string!
var connectionString = "KLISTRA-IN-DIN-CONNECTION-STRING-HÄR";
var topicName = "order-events";

// Skapa Service Bus clients
var client = new ServiceBusClient(connectionString);
var adminClient = new ServiceBusAdministrationClient(connectionString);

try
{
    // Skapa topic om den inte finns
    if (!await adminClient.TopicExistsAsync(topicName))
    {
        await adminClient.CreateTopicAsync(topicName);
        Console.WriteLine($" Topic '{topicName}' skapad!");
    }
    else
    {
        Console.WriteLine($" Topic '{topicName}' finns redan");
    }

    // Skapa subscriptions om de inte finns
    var subscriptions = new[] { "invoice", "email", "inventory", "shipping" };

    foreach (var sub in subscriptions)
    {
        if (!await adminClient.SubscriptionExistsAsync(topicName, sub))
        {
            await adminClient.CreateSubscriptionAsync(topicName, sub);
            Console.WriteLine($" Subscription '{sub}' skapad!");
        }
        else
        {
            Console.WriteLine($" Subscription '{sub}' finns redan");
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($" Fel vid setup: {ex.Message}");
    return;
}

// Skapa sender
var sender = client.CreateSender(topicName);

Console.WriteLine("\n===  Order Publisher redo! ===\n");

// Huvudloop - skapa ordrar
while (true)
{
    try
    {
        Console.WriteLine("--- Skapa ny order ---");

        // Ta emot input
        Console.Write("Order ID (nummer): ");
        if (!int.TryParse(Console.ReadLine(), out int orderId))
        {
            Console.WriteLine(" Ogiltigt Order ID\n");
            continue;
        }

        Console.Write("Kundnamn: ");
        var customerName = Console.ReadLine() ?? "";
        if (string.IsNullOrWhiteSpace(customerName))
        {
            Console.WriteLine(" Kundnamn krävs\n");
            continue;
        }

        Console.Write("Totalbelopp (kr): ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal totalAmount))
        {
            Console.WriteLine(" Ogiltigt belopp\n");
            continue;
        }

        Console.Write("Produkter (kommaseparerade, t.ex. Laptop,Mus,Tangentbord): ");
        var productsInput = Console.ReadLine() ?? "";
        var products = productsInput
            .Split(',')
            .Select(p => p.Trim())
            .Where(p => !string.IsNullOrWhiteSpace(p))
            .ToList();

        if (!products.Any())
        {
            Console.WriteLine(" Minst en produkt krävs\n");
            continue;
        }

        // Skapa event
        var orderEvent = new OrderCreatedEvent
        {
            OrderId = orderId,
            CustomerName = customerName,
            TotalAmount = totalAmount,
            ProductNames = products,
            CreatedAt = DateTime.Now
        };

        // Serialisera och skicka
        var messageBody = JsonSerializer.Serialize(orderEvent);
        var message = new ServiceBusMessage(messageBody)
        {
            ContentType = "application/json",
            Subject = "OrderCreated"
        };

        await sender.SendMessageAsync(message);

        Console.WriteLine("\n Event publicerat till topic!");
        Console.WriteLine($"    Order #{orderId} för {customerName} ({totalAmount} kr)");
        Console.WriteLine($"    Alla 4 services kommer få detta event!\n");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"\n Fel: {ex.Message}\n");
    }

    Console.WriteLine("Vill du skapa en till order? (j/n)");
    if (Console.ReadLine()?.ToLower() != "j")
        break;

    Console.WriteLine();
}

await sender.DisposeAsync();
await client.DisposeAsync();

Console.WriteLine("\n Tack för idag!");
Console.WriteLine("Tryck Enter för att avsluta...");
Console.ReadLine();
