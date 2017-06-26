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
                Console.WriteLine(type.Name);
                var properties = type.GetProperties();
                foreach (var property in properties)
                {
                    var attributes = property.GetCustomAttributes();
                    Console.WriteLine(String.Join(", ", attributes));
                }
            }
        }
    }
}
