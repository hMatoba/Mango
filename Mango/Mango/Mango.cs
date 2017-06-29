using System;
using System.Reflection;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;

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
                var isMongoDoc = HasMongoDocAttribute(type);
                if (isMongoDoc)
                {
                    Console.WriteLine(type.Name);
                }

            }
        }

        private static bool HasMongoDocAttribute(Type type)
        {
            var has = type.GetTypeInfo().IsDefined(typeof(MongoDocAttribute));
            return has;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class MongoDocAttribute : Attribute
    {
    }
}
