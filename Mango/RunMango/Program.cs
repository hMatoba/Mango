using System;
using Mango;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Attributes;

namespace RunMango
{
    class Program
    {
        static void Main(string[] args)
        {
            MongoInitializer.Initialize("RunMango");
        }
    }
}

namespace RunMango.Models
{
    class Model1
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string Id { get; set; }

        [BsonElement("token")]
        [BsonRepresentation(BsonType.String)]
        public string Token { get; set; }
    }

    class Model2
    {

    }
}