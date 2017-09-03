namespace JanHafner.Smartbar.Model
{
    using System;
    using Newtonsoft.Json.Serialization;

    internal sealed class UnresolvedTypeBinder : DefaultSerializationBinder
    {
        public override Type BindToType(String assemblyName, String typeName)
        {
            try
            {
                return base.BindToType(assemblyName, typeName);
            }
            catch
            {
                if (typeName.EndsWith("Application", StringComparison.InvariantCultureIgnoreCase))
                {
                    return typeof (UnresolvedApplication);
                }

                if (typeName.EndsWith("ApplicationImage", StringComparison.InvariantCultureIgnoreCase))
                {
                    return typeof(UnresolvedApplicationImage);
                }

                if (typeName.EndsWith("PluginConfiguration", StringComparison.InvariantCultureIgnoreCase))
                {
                    return typeof(UnresolvedPluginConfiguation);
                }

                throw new NotSupportedException();
            }
        }
    }
}