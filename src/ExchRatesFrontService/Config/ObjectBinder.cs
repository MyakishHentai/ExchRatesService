using System;
using System.Reflection;

namespace ExchRatesFrontService.Config
{
    public sealed class ObjectBinder : System.Runtime.Serialization.SerializationBinder
    {
        public override Type BindToType(string assemblyName, string typeName)
        {
            Type typeToDeserialize = null;
            string currentAssembly = Assembly.GetExecutingAssembly().FullName;

            // In this case we are always using the current assembly
            assemblyName = currentAssembly;

            // Get the type using the typeName and assemblyName
            typeToDeserialize = Type.GetType(string.Format("{0}, {1}", 
                typeName, assemblyName));

            return typeToDeserialize;
        }
    }
}
