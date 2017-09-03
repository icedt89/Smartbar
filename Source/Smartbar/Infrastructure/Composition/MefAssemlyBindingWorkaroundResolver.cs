namespace JanHafner.Smartbar.Infrastructure.Composition
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    internal static class MefAssemlyBindingWorkaroundResolver
    {
        private static readonly IDictionary<String, Assembly> resolvedAssemblies = new Dictionary<String, Assembly>();

        public static Assembly CurrentDomainOnAssemblyResolve(Object sender, ResolveEventArgs args)
        {
            Assembly result;
            if (!resolvedAssemblies.TryGetValue(args.Name, out result))
            {
                var appDomainAssemblies = AppDomain.CurrentDomain.GetAssemblies();

                var simpleAssemblyName = ExtractSimpleAssemblyName(args.Name);

                result = appDomainAssemblies.FirstOrDefault(a => a.ManifestModule.Name.Equals(simpleAssemblyName, StringComparison.InvariantCultureIgnoreCase));
                resolvedAssemblies.Add(args.Name, result);
            }

            return result;
        }

        public static Assembly CurrentDomainOnTypeResolve(Object sender, ResolveEventArgs args)
        {
            return null;
        }

        private static String ExtractSimpleAssemblyName(String fullAssemblyName)
        {
            var partsOfFullName = fullAssemblyName.Split(',');

            return partsOfFullName[0] + ".dll";
        }
    }
}
