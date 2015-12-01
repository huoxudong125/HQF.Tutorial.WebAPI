using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using HQF.Tutorial.WebAPI.Models;

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

        public IEnumerable<Product> GetAllProducts()
        {
            return products;
        }

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