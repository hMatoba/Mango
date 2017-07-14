using MangoFramework;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace MangoTests.Models
{
    [MongoDoc]
    class Model1 : MangoBase<Model1>
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
    class Model3 : MangoBase<Model3>
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string Id { get; set; }

        [BsonElement("token")]
        [BsonRepresentation(BsonType.String)]
        public string Token { get; set; }

        public static Model3 Get()
        {
            var model3 = new Model3();
            return model3;
        }
    }

    [MongoDoc]
    class ModelFooBar4 : MangoBase<ModelFooBar4>
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string Id { get; set; }
    }
}