using System;
using MangoFramework;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace MangoTests.Models
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

        [BsonElement("createdAt")]
        [BsonRepresentation(BsonType.Timestamp)]
        public DateTime CreatedAt { get; set; }


        public static CreateCollectionOptions collectionOptions = new CreateCollectionOptions()
        {
            Capped = false,
        };

        public static List<CreateIndexModel<BsonDocument>> indexModels = new List<CreateIndexModel<BsonDocument>>()
        {
            new CreateIndexModel<BsonDocument>(
                new IndexKeysDefinitionBuilder<BsonDocument>().Ascending(new StringFieldDefinition<BsonDocument>("createdAt")),
                new CreateIndexOptions(){ ExpireAfter = TimeSpan.FromSeconds(10) }
            )
        };

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