using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
namespace Example.Web
{
    public class Startup
    {
        private IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                    //var builder = new ConfigurationBuilder()
                    //    //.AddConsul(options=> {
                    //    //    options.Address = new Uri("https://demo.consul.io");
                    //    //    options.Datacenter = "nyc3";
                    //    //    //consul中的key是区分大小写的,所以这里的prefix要注意大小写
                    //    //    options.Prefix = null;
                    //    //});
                    //    .AddSqlite("Data Source=./config.db;");
                    //IConfigurationRoot configuration = builder.Build();

                    //await context.Response.WriteAsync(configuration["global:time"]);
                });
                endpoints.MapGet("/set", async context =>
                {
                    _configuration["global:time"] = DateTime.Now.ToString();
                    await context.Response.WriteAsync(_configuration["global:time"]);
                });

                endpoints.MapGet("/get", async context =>
                {
                    await context.Response.WriteAsync(_configuration["global:time"]);
                });
            });
        }

    }
}
