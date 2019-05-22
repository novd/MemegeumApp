using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using memegeumApp.Data;
using memegeumApp.Models;
using memegeumApp.Parsers;
using memegeumApp.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

/*

*/

namespace memegeumApp
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSession(options => options.IdleTimeout = TimeSpan.FromMinutes(30));

            services.AddDbContextPool<AppDbContext>(
                options => options.UseSqlite("Data Source=memegeum.db")
                );
            services.AddMvc();

            services.AddTransient<IMemeRespository, DbMemeRespository>();

            services.AddSingleton<IMemeParser, GeneralMemeParser>();

            services.AddHttpContextAccessor();
            services.AddLogging();
            services.AddHostedService<MemeParseService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseSession();
            app.UseMvc();


        }
    }
}
