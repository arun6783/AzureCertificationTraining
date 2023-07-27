
using Azure.Core.Pipeline;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using StorageQueueDemo;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

string connectionString = "";
string queueName = "appqueue";

Console.WriteLine("Hello, World! Simple program to demostrate read, peek and send message to storage queue!!");

//SendMessage("Test message 1", true);
//SendMessage("Test message 2", true);
//SendMessage("Test message 3", true)
//PeekMessage();

//ReceiveMessages();

//GetQueueLength();

SendOrderMessage();

void SendOrderMessage()
{
    var order = new Order() { OrderID = "asd1", Quantity = 10 };
    var order1 = new Order() { OrderID = "asd2", Quantity = 20 };

    SendMessage(Newtonsoft.Json.JsonConvert.SerializeObject(order), true);
    SendMessage(Newtonsoft.Json.JsonConvert.SerializeObject(order1), true);
}

void SendMessage(string message, bool asBase64=false)
{
    QueueClient client = GetClient(connectionString, queueName);

    if (client.Exists())
    {

        client.SendMessage(asBase64 ? ToBase64Encode(message) : message);
        Console.WriteLine("Message has been sent");
    }
    else
    {
        Console.WriteLine($"queue {queueName} does not exist");
    }
}



void GetQueueLength()
{
    QueueClient client = GetClient(connectionString, queueName);

    if (client.Exists())
    {
        QueueProperties properties = client.GetProperties();

        Console.WriteLine($"Number of messages in the queue {queueName} is {properties.ApproximateMessagesCount}");
    }
    else
    {
        Console.WriteLine($"queue {queueName} does not exist");
    }
}

void PeekMessage()
{
    QueueClient client = GetClient(connectionString, queueName);

    PeekedMessage [] messages =  client.PeekMessages(10);

    if (messages != null)
    {
        Console.WriteLine("The messages in the queue are ");

        foreach (var message in messages)
        {
            Console.WriteLine(message.Body);
        }
    }
    else
    {
        Console.WriteLine("No messages are present in the queue");
    }
}


void ReceiveMessages()
{
    QueueClient client = GetClient(connectionString, queueName);

    QueueMessage[] messages = client.ReceiveMessages(10);

    if (messages != null)
    {
        Console.WriteLine("The Received messages in the queue are ");

        foreach (var message in messages)
        {
            Console.WriteLine(message.Body);
            client.DeleteMessage(message.MessageId, message.PopReceipt);

        }
    }
    else
    {
        Console.WriteLine("No messages are present in the queue");
    }
}

static QueueClient GetClient(string connectionString, string queueName)
{
    return new QueueClient(connectionString, queueName);
}


 static string ToBase64Encode(string plainText)
{
    var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
    return System.Convert.ToBase64String(plainTextBytes);
}

static string Base64Decode(string base64EncodedData)
{
    var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
    return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
}