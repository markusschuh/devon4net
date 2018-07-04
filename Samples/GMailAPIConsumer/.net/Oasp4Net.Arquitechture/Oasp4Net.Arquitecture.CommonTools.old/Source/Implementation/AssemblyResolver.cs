using Microsoft.Extensions.Configuration;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Oasp4Net.Arquitecture.CommonTools.Source.Common;
using Oasp4Net.Arquitecture.CommonTools.Source.Interfaces;

namespace Oasp4Net.Arquitecture.CommonTools.Source.Implementation
{
    public class AssemblyResolver : IAssemblyResolver
    {
        public void GetReferencedAssemblyController(string section, ref IServiceCollection services, IConfigurationRoot configuration)
        {
            var config = configuration.GetSection(section).Get<ReferencedAssemblyConfiguration>();
            var assemblies = config.ReferencedAssemblies.Split(config.SeparatorChar);
            foreach (var assemblyName in assemblies)
            {
                if (string.IsNullOrEmpty(assemblyName)) continue;
                var assembly = Assembly.Load(new AssemblyName(assemblyName));
                services.AddMvc().AddApplicationPart(assembly).AddControllersAsServices();
            }
        }
    }
}
