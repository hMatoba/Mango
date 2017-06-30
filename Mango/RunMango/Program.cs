using System;
using Mango;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.RegularExpressions;

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
    [MongoDoc]
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

    [MongoDoc("model3")]
    class Model3
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string Id { get; set; }

        [BsonElement("token")]
        [BsonRepresentation(BsonType.String)]
        public string Token { get; set; }
    }

    [MongoDoc]
    class ModelFooBar4
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string Id { get; set; }
    }
    }