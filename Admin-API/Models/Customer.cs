using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Admin_API.Models
{
    public class Customer
    {
        [BsonId]
        public int Id { get; set; }
        public string Cname { get; set; }
        public string Email { get; set; }
        public string Passwd { get; set; }
        public string City { get; set; }
    }
}
