using System;
using Xunit;
using MongoDB.Bson;
using MongoDB.Driver;
using Mango;
using System.Linq;

namespace MangoTests
{
    [CollectionDefinition("CallFixture")]
    public class UnitTest1
    {
        private string connectionString = "mongodb://localhost";
        private string dbName = "mango";

        [Fact]
        public void Test1()
        {
            MongoInitializer.Run(connectionString, dbName, "MangoTests");

            DbConnection.SetDB(connectionString, dbName);
            var db = DbConnection.db;
            var collections = db.ListCollections().ToList<BsonDocument>().Select(e => e["name"].AsString);
            Assert.True(collections.Contains("Model1"));
            Assert.True(collections.Contains("model3"));
            Assert.True(collections.Contains("ModelFooBar4"));
        }

    }

    public class Fixture : IDisposable
    {
        private string connectionString = "mongodb://localhost";
        private string dbName = "mango";

        public Fixture()
        {
            Console.WriteLine("init.");
            DbConnection.SetDB(connectionString, dbName);
            var db = DbConnection.db;
            db.DropCollection("Model1");
            db.DropCollection("model3");
            db.DropCollection("ModelFooBar4");
        }

        public void Dispose()
        {
            var db = DbConnection.db;
            db.DropCollection("Model1");
            Console.WriteLine("over.");
        }
    }

    [CollectionDefinition("CallFixture")]
    public class ProjectSetup : ICollectionFixture<Fixture> { }

    public static class DbConnection
    {
        public static IMongoDatabase db { get; set; }

        public static void SetDB(IMongoDatabase db)
        {
            DbConnection.db = db;
        }

        public static void SetDB(string connectionString, string dbName)
        {
            Connect(connectionString, dbName);
        }


        private static void Connect(string connectionString, string dbName)
        {
            var client = new MongoClient(connectionString);
            db = client.GetDatabase(dbName);
        }
    }
}
