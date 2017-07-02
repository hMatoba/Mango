using MongoDB.Driver;
using System.IO;
using System.Collections.Generic;

namespace Mango
{
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