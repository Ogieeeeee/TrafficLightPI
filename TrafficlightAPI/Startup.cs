using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TrafficlightAPI.Interfaces;
using TrafficlightAPI.Managers;
using TrafficlightAPI.Service;

namespace TrafficlightAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration , OrangeTimerHostedService service)
        {
            Configuration = configuration;
            service.StopAsync(new System.Threading.CancellationToken());
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IPIManager, PIManager>();
            services.AddSingleton<IHostedService, GreenTimerHostedService>();
            //services.AddSingleton<IHostedService, OrangeTimerHostedService>();
            services.AddHostedService<OrangeTimerHostedService>();


            //services.AddHostedService<HelloWorldHostedService>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
