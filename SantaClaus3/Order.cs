using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SantaClaus3
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ID { get; set; }

        [BsonElement("status")]
        public int Status { get; set; }

        [BsonElement("kid")]
        public string KidName { get; set; }

        [BsonElement("requestDate")]
        public DateTime Date { get; set; }

        [BsonElement("toys")]
        public List<ToyKid> ToyKids { get; set; }

        public List<Decimal> PriceRequest { get; set; }

    }

    public class ToyKid
    {
        [BsonElement("name")]
        public string ToyName { get; set; }

    }
}