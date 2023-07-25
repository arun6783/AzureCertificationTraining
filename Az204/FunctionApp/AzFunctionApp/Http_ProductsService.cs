using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data.SqlClient;
using AzFunctionApp.Models;
using System.Collections.Generic;

namespace AzFunctionApp
{
    public static class Http_ProductsService
    {
        [FunctionName("ProductService")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Products")] HttpRequest req,
            ILogger log)
        {
            var products = new List<Product>();
            string productsStatement = "select productID, ProductName, Quantity from Products";


            using(var conn = GetConnection())
            {
                conn.Open();

                var _sqlCommand = new SqlCommand(productsStatement, conn);

                using(var _reader = _sqlCommand.ExecuteReader())
                {
                    while (await _reader.ReadAsync())
                    {
                        products.Add(new Product()
                        {
                            ProductID = _reader.GetInt32(0),
                            ProductName = _reader.GetString(1),
                            Quantity = _reader.GetInt32(2),
                        });
                    }
                }

                conn.Close();
            }
            return new OkObjectResult(products);

        }

        private static SqlConnection GetConnection()
        {
            return new SqlConnection(Environment.GetEnvironmentVariable("SQLAZURECONNSTR_SqlConnectionString"));
        }
    }
}
