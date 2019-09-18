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

            services.Configure<IdentityOptions>(opt =>
            {
                //referente a tipo de password
                opt.Password.RequireDigit = true;
                opt.Password.RequiredLength = 8;
                opt.Password.RequiredUniqueChars = 6;
                opt.Password.RequireLowercase = true;
                opt.Password.RequireNonAlphanumeric = true;
                opt.Password.RequireUppercase = true;
            });

            services.ConfigureApplicationCookie(opt =>
            {
                opt.CookieHttpOnly = true;
                opt.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                opt.LoginPath = "/Account/Login";
                opt.LogoutPath = "/Account/Logout";
                opt.AccessDeniedPath = "/Account/AccessDenied";
                opt.SlidingExpiration = true;
            });

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
