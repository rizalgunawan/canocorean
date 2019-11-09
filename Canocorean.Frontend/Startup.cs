using Canocorean.Frontend.Bootstrap;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Canocorean.Frontend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .ConfigureInfrastructure()
                .AddFrontEndAPI()
                .ConfigureSwagger()
                .ConfigureValidation()
                .ConfigureAutoMapper()
                .AddHealthChecks();

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage().UseSwagger();
            }
            else
            {
                app.UseHsts().UseHttpsRedirection();
            }

            app.UseHealthChecks("/health/alive")
                .UseStaticFiles()
                .UseSpaStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            }).UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";
                if (Configuration["RUN_DEV_SERVER"] == "true")
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
