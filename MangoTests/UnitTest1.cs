using System;
using Xunit;
using MongoDB.Bson;
using MongoDB.Driver;
using MangoFramework;
using System.Linq;
using MangoTests.Models;

namespace MangoTests
{
    [CollectionDefinition("CallFixture")]
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            MongoInitializer.Run(Settings.connectionString,
                                 Settings.dbName,
                                 Settings.assemblyName);

            DbConnection.SetDB(Settings.connectionString,
                               Settings.dbName);
            var db = DbConnection.db;
            var collections = db.ListCollections().ToList<BsonDocument>().Select(e => e["name"].AsString);
            Assert.Contains("Model1", collections);
            Assert.Contains("model3", collections);
            Assert.Contains("ModelFooBar4", collections);
        }

}

    public class Fixture : IDisposable
    {
        public Fixture()
        {
            DbConnection.SetDB(Settings.connectionString,
                               Settings.dbName);
            var db = DbConnection.db;
            db.DropCollection("Model1");
            db.DropCollection("model3");
            db.DropCollection("ModelFooBar4");
        }

        public void Dispose()
        {
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
