using Admin_API.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace Admin_API.Controllers
{
    [ApiController]
    [Route("api/admin")]
    public class CustomerController : ControllerBase
    {
        //one collection for productModel,and Customer
        private readonly IMongoCollection<ProductDocument> _productCollection;
        private readonly IMongoCollection<Customer> _customerCollection;

        public CustomerController(IMongoDatabase mongoDatabase)
        {
            _productCollection = mongoDatabase.GetCollection<ProductDocument>("products");
            _customerCollection = mongoDatabase.GetCollection<Customer>("customers");
        }

        [HttpGet("products")]
        public async Task<IEnumerable<ProductDocument>> GetProducts()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5289/");
            HttpResponseMessage response = await client.GetAsync("api/products");

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                List<ProductDocument> products = JsonConvert.DeserializeObject<List<ProductDocument>>(content);

                foreach (ProductDocument product in products)
                {
                    // Check if the product already exists in the MongoDB collection
                    FilterDefinition<ProductDocument> filter = Builders<ProductDocument>.Filter.Eq(p => p.Id, product.Id);
                    bool exists = await _productCollection.Find(filter).AnyAsync();

                    if (!exists)
                    {
                        await _productCollection.InsertOneAsync(product);
                    }
                    else
                    {
                        // Update the product in the MongoDB collection
                        UpdateDefinition<ProductDocument> update = Builders<ProductDocument>.Update.Set(p => p.Name, product.Name)
                                                                                                 .Set(p => p.Price, product.Price);
                                                                                                 
                        await _productCollection.UpdateOneAsync(filter, update);
                    }
                }

                return products;
            }

            return null;
        }

        [HttpGet("customers")]
        public async Task<IEnumerable<Customer>> GetCustomer()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7117/");
            HttpResponseMessage response = await client.GetAsync("api/customers");
            Console.WriteLine(response);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                List<Customer> customers = JsonConvert.DeserializeObject<List<Customer>>(content);

                foreach (Customer cust in customers)
                {
                    // Check if the product already exists in the MongoDB collection
                    FilterDefinition<Customer> filter = Builders<Customer>.Filter.Eq(p => p.Id, cust.Id);
                    bool exists = await _customerCollection.Find(filter).AnyAsync();

                    if (!exists)
                    {
                        await _customerCollection.InsertOneAsync(cust);
                    }
                    else
                    {
                        // Update the product in the MongoDB collection
                        UpdateDefinition<Customer> update = Builders<Customer>.Update.Set(p => p.Cname, cust.Cname)
                                                                                                 .Set(p => p.City, cust.City);

                        await _customerCollection.UpdateOneAsync(filter, update);
                    }
                }

                return customers;
            }

            return null;
        }

        [HttpGet("products/pdf")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileStreamResult))]
        public async Task<FileStreamResult> GetProductsPdf()
        {
            var products = await _productCollection.Find(_ => true).ToListAsync();
            var memoryStream = new MemoryStream();
            var document = new Document();
            var writer = PdfWriter.GetInstance(document, memoryStream);

            document.Open();
            //document.Add(new Paragraph("Products"));

            // Add centered heading
            var heading = new Paragraph("Products");
            heading.Alignment = Element.ALIGN_CENTER;
            document.Add(heading);

            // Add table
            var table = new PdfPTable(2);
            table.AddCell("Name");
            table.AddCell("Price");

            foreach (var product in products)
            {
                //document.Add(new Paragraph($"{product.Name}, {product.Price}"));
                table.AddCell(product.Name);
                table.AddCell(product.Price.ToString());
            }
            document.Add(table);
            document.Close();

            // Create a copy of the memory stream
            var copyStream = new MemoryStream(memoryStream.ToArray());

            return new FileStreamResult(copyStream, "application/pdf")
            {
                FileDownloadName = "products.pdf"
            };
        }









    }

}
