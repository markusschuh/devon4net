using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.DependencyInjection;

namespace Devon4Net.Application.Configuration.Startup
{
    public static class SPAConfiguration
    {
        public static void ConfigureSPA(this IServiceCollection services, string rootPath)
        {
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = rootPath;
            });
        }

        public static void ConfigureSpa(this IApplicationBuilder app, string spaRootPath, string SpaNpmScript,string useProxyToSpaDevelopmentServer, string defaultUrl)
        {
            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = spaRootPath;
                if (!string.IsNullOrEmpty(SpaNpmScript)) spa.UseAngularCliServer(npmScript: SpaNpmScript);
                spa.UseProxyToSpaDevelopmentServer(string.IsNullOrEmpty(useProxyToSpaDevelopmentServer) ? defaultUrl : useProxyToSpaDevelopmentServer);
            });
        }
    }
}
