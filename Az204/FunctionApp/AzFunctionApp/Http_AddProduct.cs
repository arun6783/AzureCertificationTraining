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
using System.Data;

namespace AzFunctionApp
{
    public static class Http_AddProduct
    {
        [FunctionName("AddProduct")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "products")] HttpRequest req, 
            ILogger log)
        {
         


            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            var product = JsonConvert.DeserializeObject<Product>(requestBody);

            using (var conn = GetConnection())
            {
                conn.Open();

                var statement = "insert into products(ProductId, ProductName, Quantity) VALUES (@param1, @param2, @param3)";


                using(var cmd = new SqlCommand(statement, conn))
                {
                    cmd.Parameters.Add("@param1", SqlDbType.Int).Value = product.ProductID;
                    cmd.Parameters.Add("@param2", SqlDbType.VarChar).Value = product.ProductName;
                    cmd.Parameters.Add("@param3", SqlDbType.Int).Value = product.Quantity;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
                conn.Close();

            }
            return new OkObjectResult("Product Added");
            
          
        }

        private static SqlConnection GetConnection()
        {

            

            return new SqlConnection(Environment.GetEnvironmentVariable("SQLAZURECONNSTR_SqlConnectionString"));
        }
    }
}
