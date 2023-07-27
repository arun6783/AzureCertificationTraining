
using Azure.Core.Pipeline;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;

string connectionString = "DefaultEndpointsProtocol=https;AccountName=stgacdotnetdemo;AccountKey=Q39NNMNUVeSy/yICMwoGi2uEIKH9ZGvd/9Nmve9kpBWjLJ9e1t0P7faoeimc389sm8pTBz3pX5RU+AStV+mRlQ==;EndpointSuffix=core.windows.net";
string queueName = "appqueue";

Console.WriteLine("Hello, World! Simple program to demostrate read, peek and send message to storage queue!!");

SendMessage("Test message 1");
SendMessage("Test message 2");
SendMessage("Test message 3");
//PeekMessage();

ReceiveMessages();

GetQueueLength();


void SendMessage(string message)
{
    QueueClient client = GetClient(connectionString, queueName);

    if (client.Exists())
    {
        client.SendMessage(message);
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