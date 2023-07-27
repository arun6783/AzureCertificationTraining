using AzFunctionApp.Models;
using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;



namespace AzFunctionApp
{
    public class Queue_ReadMessagesTableOutMessage
    {
        [FunctionName("GetMessages")]

        public void Run([QueueTrigger("appqueue", Connection = "connectionString")] Order order, ILogger log, [Table("Orders", Connection = "connectionString")] ICollector<TableOrder> tableorder)
        {
            TableOrder tableOrder = new TableOrder()
            {
                PartitionKey = order.OrderID,
                RowKey = order.Quantity.ToString()
            };
            tableorder.Add(tableOrder);
            log.LogInformation("Order written to table");

        }
    }



}
