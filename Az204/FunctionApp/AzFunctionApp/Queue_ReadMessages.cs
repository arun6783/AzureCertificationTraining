using AzFunctionApp.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzFunctionApp
{
    public class Queue_ReadMessages
    {
        [FunctionName("GetMessages")]
        public void Run([QueueTrigger("appqueue", Connection = "connectionString")] Order order, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: orderid =  {order.OrderID}");
        }
    }
}
