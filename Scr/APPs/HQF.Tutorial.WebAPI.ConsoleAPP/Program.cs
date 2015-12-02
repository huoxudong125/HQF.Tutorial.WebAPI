using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using HQF.Tutorial.WebAPI.MessageHandlers.ClientSide;
using HQF.Tutorial.WebAPI.Models;

namespace HQF.Tutorial.WebAPI.ConsoleAPP
{
    internal class Program
    {
        private static void Main()
        {
            try
            {
                RunAsync().Wait();

                Console.ReadKey();
            }
            catch (AggregateException ex)
            {
                Console.WriteLine(ex.InnerException.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static async Task RunAsync()
        {
            using (var sw = File.OpenWrite("WebApiHttpClientLog.log"))
            {
                //using (var client = new HttpClient())
                using (var client = HttpClientFactory.Create(new LoggingHandler(sw)))//in order to add message handler using create()
                {
                    client.BaseAddress = new Uri("http://localhost:14583/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // HTTP GET
                    var response = await client.GetAsync("api/products/1");
                    if (response.IsSuccessStatusCode)
                    {
                        var product = await response.Content.ReadAsAsync<Product>();
                        Console.WriteLine("{0}\t${1}\t{2}", product.Name, product.Price, product.Category);
                    }

                    // HTTP POST
                    var gizmo = new Product {Name = "Gizmo", Price = 100, Category = "Widget"};
                    response = await client.PostAsJsonAsync("api/products", gizmo);
                    if (response.IsSuccessStatusCode)
                    {
                        var gizmoUrl = response.Headers.Location;

                        // HTTP PUT
                        gizmo.Price = 80; // Update price
                        response = await client.PutAsJsonAsync(gizmoUrl, gizmo);

                        // HTTP DELETE
                        response = await client.DeleteAsync(gizmoUrl);
                    }
                }
            }
        }
    }
}