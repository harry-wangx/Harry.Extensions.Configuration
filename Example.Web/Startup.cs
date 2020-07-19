using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Harry.Extensions.MicrosoftConfiguration.Sqlite;

namespace Example.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {

        }

        public void ConfigureServices(IServiceCollection services)
        {
            //using (var db = new ConfigurationDbContext("Data Source=./config.db;"))
            //{
            //    if (db.Database.EnsureCreated())
            //    {
            //        db.Configs.Add(new ConfigInfo() { Key = "global:time", Value = DateTime.Now.ToString(), LastUpdateTime = DateTime.Now });
            //        db.SaveChanges();
            //    }
            //}
        }


        public void Configure(IApplicationBuilder app)
        {
            app.Run(async (context) =>
            {
                var builder = new ConfigurationBuilder()
                    //.AddConsul(options=> {
                    //    options.Address = new Uri("https://demo.consul.io");
                    //    options.Datacenter = "nyc3";
                    //    //consul中的key是区分大小写的,所以这里的prefix要注意大小写
                    //    options.Prefix = null;
                    //});
                    .AddSqlite("Data Source=./config.db;");
                IConfigurationRoot configuration = builder.Build();

                await context.Response.WriteAsync(configuration["global:time"]);
            });
        }
    }
}
