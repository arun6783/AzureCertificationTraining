using AspNetcoreWebApp.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.FeatureManagement;
using System.Data.SqlClient;

namespace AspNetcoreWebApp.Services
{
    public class ProductsService : IProductsService
    {

        private readonly IConfiguration _configuration;
        private readonly IFeatureManager _featureManager;

        public ProductsService(IConfiguration configuration, IFeatureManager featureManager)
        {
            _configuration = configuration;
            _featureManager = featureManager;
        }

        public List<Product> GetProducts()
        {
            string connString = _configuration["SqlConnection"].ToString();
            List<Product> _product_lst = new List<Product>();
            string _statement = "SELECT ProductID,ProductName,Quantity from Products";
            SqlConnection _connection = new SqlConnection(connString);

            _connection.Open();

            SqlCommand _sqlcommand = new SqlCommand(_statement, _connection);

            using (SqlDataReader _reader = _sqlcommand.ExecuteReader())
            {
                while (_reader.Read())
                {
                    Product _product = new Product()
                    {
                        ProductID = _reader.GetInt32(0),
                        ProductName = _reader.GetString(1),
                        Quantity = _reader.GetInt32(2)
                    };

                    _product_lst.Add(_product);
                }
            }
            _connection.Close();
            return _product_lst;
        }


        public async Task<bool> IsFaetureA()
        {
            return await _featureManager.IsEnabledAsync("FeatureA");
        }

    }

    public interface IProductsService
    {
        Task<bool> IsFaetureA();
        List<Models.Product> GetProducts();
    }
}
