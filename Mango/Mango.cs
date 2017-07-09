using System;
using System.Reflection;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;

namespace Mango
{
    public class MongoInitializer
    {
        public static void Run(IMongoDatabase db, string assemblyName, string namespaceName = "")
        {
            DbConnection.SetDB(db);
            InitializeDb(assemblyName, namespaceName);
        }

        public static void Run(string connectionString, string dbName, string assemblyName, string namespaceName = "")
        {
            DbConnection.SetDB(connectionString, dbName);
            InitializeDb(assemblyName, namespaceName);
        }


        public static void InitializeDb(string assemblyName, string namespaceName)
        {
            var namespaceFullName = namespaceName == "" ? $"{assemblyName}.Models" : namespaceName;
            var models = GetModels(assemblyName, namespaceFullName);
            foreach (var modelClass in models)
            {
                var mongoDoc = HasMongoDocAttribute(modelClass);
                if (mongoDoc != null)
                {
                    var collectionName = mongoDoc.CollectionName != "" ? mongoDoc.CollectionName : modelClass.Name;
                    var collections = DbConnection.db.ListCollections().ToList<BsonDocument>().Select(c => c["name"].AsString);

                    if (!collections.Contains(collectionName))
                    {
                        var collectionOptions = modelClass.GetField("collectionOptions") != null
                                                    ? (CreateCollectionOptions)modelClass.GetField("collectionOptions").GetValue(null)
                                                    : null;
                        DbConnection.db.CreateCollection(collectionName, collectionOptions);
                        var collection = DbConnection.db.GetCollection<BsonDocument>(collectionName);

                        var indexOptions = modelClass.GetField("indexOptions") != null
                                                    ? (CreateIndexOptions)modelClass.GetField("indexOptions").GetValue(null)
                                                    : null;
                        if (indexOptions != null)
                        {
                            var field = new StringFieldDefinition<BsonDocument>(collectionName);
                            var indexDefinition = new IndexKeysDefinitionBuilder<BsonDocument>().Ascending(field);
                            collection.Indexes.CreateOne(indexDefinition, indexOptions);
                        }
                    }
                }
            }
          
        }

        private static Type[] GetModels(string assemblyName, string modelNamespace)
        {
            var myAssembly = Assembly.Load(new AssemblyName(assemblyName));
            var models = myAssembly.GetTypes().Where(t => t.Namespace == modelNamespace).ToArray();
            return models;
        }

        private static MongoDocAttribute HasMongoDocAttribute(Type type)
        {
            var has = type.GetTypeInfo().IsDefined(typeof(MongoDocAttribute));
            if (has)
            {
                var mongoDocAttribute = type.GetTypeInfo().GetCustomAttribute<MongoDocAttribute>();
                return mongoDocAttribute;
            }
            else
            {
                return null;
            }
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class MongoDocAttribute : Attribute
    {
        public string CollectionName { get; set; } = "";

        public MongoDocAttribute(string name = "")
        {
            {
                this.CollectionName = name;
            }
        }
    }
}
