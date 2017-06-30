using System;
using System.Reflection;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;
using System.Text.RegularExpressions;

namespace Mango
{
    public class MongoInitializer
    {
        public static void Initialize(string assemblyName, string namespaceName="")
        {
            var namespaceFullName = namespaceName == "" ? $"{assemblyName}.Models" : namespaceName;
            var myAssembly = Assembly.Load(new AssemblyName(assemblyName));
            var myTypes = myAssembly.GetTypes().Where(t => t.Namespace == namespaceFullName).ToList();
            foreach (var type in myTypes)
            {
                var mongoDoc = HasMongoDocAttribute(type);
                if (mongoDoc != null)
                {
                    Console.WriteLine(type.Name);
                    string collectionName;
                    if (mongoDoc.CollectionName != "")
                    {
                        collectionName = mongoDoc.CollectionName;
                    }
                    else
                    {
                        collectionName = GetSnakeCase(type.Name);
                    }
                    Console.WriteLine(collectionName);
                    
                }

            }


        }

        private static string GetSnakeCase(string str)
        {
            var pattern = "[a-z][A-Z]";
            var rgx = new Regex(pattern);
            var snakeStr = rgx.Replace(str, m => m.Groups[0].Value[0] + "_" + m.Groups[0].Value[1])
                         .ToLower();
            return snakeStr;
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
