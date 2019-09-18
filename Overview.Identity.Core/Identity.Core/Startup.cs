using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Identity.Core.Data;
using Identity.Core.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Core
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connection = Configuration.GetConnectionString("IdentityDb");
            services.AddDbContext<ApplicationDataContext>(options => 
            {
                options.UseSqlServer(connection);
            });

            //recebe dois parametros genericos, define as classes que serão utilizadas
            //para representar o usuario e as roles da aplicação, indica que quer utilizar 
            //a classe customizada para o usuario e a classe identityRole com id tipo Guid
            services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
                //registra o entity como responsavel pelo armazenamento dos dados pelo identity
                .AddEntityFrameworkStores<ApplicationDataContext>()
                //registra os serviços e adiciona o provider padrão de token
                .AddDefaultTokenProviders();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
