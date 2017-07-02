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
            DbConnection.SetDB(dbName, connectionString);
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
                        var collectionOptions = modelClass.GetField("CollectionOptions") != null
                                                    ? (CreateCollectionOptions)modelClass.GetField("CollectionOptions").GetValue(null)
                                                    : null;
                        DbConnection.db.CreateCollection(collectionName, collectionOptions);
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
            if (name != "")
            {
                this.CollectionName = name;
            }
        }
    }
}
