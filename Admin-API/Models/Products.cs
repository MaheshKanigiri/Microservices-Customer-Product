using MongoDB.Bson.Serialization.Attributes;

namespace Admin_API.Models
{
    public class ProductDocument
    {
        [BsonId]
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

    }
}
