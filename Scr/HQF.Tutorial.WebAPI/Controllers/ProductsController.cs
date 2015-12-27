using HQF.Tutorial.WebAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace HQF.Tutorial.WebAPI.Controllers
{
    //[EnableCors("http://localhost:31272", "*", "*")]//Golally Open CORS for website:http://localhost:31272
    public class ProductsController : ApiController
    {
        private readonly Product[] products =
        {
            new Product {Id = 1, Name = "Tomato Soup", Category = "Groceries", Price = 1},
            new Product {Id = 2, Name = "Yo-yo", Category = "Toys", Price = 3.75M},
            new Product {Id = 3, Name = "Hammer", Category = "Hardware", Price = 16.99M}
        };

        /// <summary>
        /// Get all Products
        /// </summary>
        /// <returns>A array of Product</returns>
        public IEnumerable<Product> GetAllProducts()
        {
            return products;
        }

        /// <summary>
        /// Get the Product
        /// </summary>
        /// <param name="id">Prodcut Id</param>
        /// <returns>200 if find the product</returns>
        public IHttpActionResult GetProduct(int id)
        {
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }
    }
}