using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Oasp4Net.Arquitecture.CommonTools.Source.Interfaces
{
    public interface IAssemblyResolver
    {
        void GetReferencedAssemblyController(string section, ref IServiceCollection services, IConfigurationRoot configuration);
    }
}