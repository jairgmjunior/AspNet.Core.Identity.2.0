using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Overview.Identity.Core.Data;
using Overview.Identity.Core.Models;
using Overview.Identity.Core.Services;

namespace Overview.Identity.Core
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(options => 
            {
                /*Lockouts*/
                //determina se um novo usuario poderá ser bloqueado
                options.Lockout.AllowedForNewUsers = true;
                //determina o tempo que um usuario sera bloqueado para realizar o login
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                //determina a quantidade de vezes que um usuario poderá errar o login
                options.Lockout.MaxFailedAccessAttempts = 5;

                /*Passwords*/
                //exige que tenha ao menos um numero na senha
                options.Password.RequireDigit = true;
                //exige que tenha no minimo 6 digitos
                options.Password.RequiredLength = 6;
                //determina a quantidade de caracteres distintos na senha (default 1)
                options.Password.RequiredUniqueChars = 1;
                //determina a obrigatoriedade de letra minuscula na senha
                options.Password.RequireLowercase = true;
                //determina a obrigatoriedade de letra maiúscula na senha
                options.Password.RequireUppercase = true;
                //determina a obrigatoriedade de um caractere especial na senha
                options.Password.RequireNonAlphanumeric = true;

                /*SignIn*/
                //define que o usuario deve ter um email confirmado
                options.SignIn.RequireConfirmedEmail = false;
                //define que o usuario deve confirmar por telefone
                options.SignIn.RequireConfirmedPhoneNumber = false;

                /*Token  (Já existem valores default definidos)*/
                //define o autentication token provider
                //options.Tokens.AuthenticatorTokenProvider
                //usado para gerar tokens no email do usuario
                //options.Tokens.ChangeEmailTokenProvider
                //utilizado para gerar tokens do numero de telefone do usuario
                //options.Tokens.ChangePhoneNumberTokenProvider
                //usado para a configuração de um email informado pelo usuario
                //options.Tokens.EmailConfirmationTokenProvider
                //usado no processo de recuperação da senha do usuario
                //options.Tokens.PasswordResetTokenProvider

                /*User*/
                //define os caracteres permitidos para o nome do usuario
                options.User.AllowedUserNameCharacters = 
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                //determina se cada usuario tera um email unico
                options.User.RequireUniqueEmail = false;

            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                //informa o caminho para o handler responsavel pelo erro 403 (acesso proibido)
                options.AccessDeniedPath = "/Account/AccessDenied"; //action
                //obtem o issuer que sera utilizado para a criação de claims
                //options.ClaimsIssuer
                //define o dominio do cookie criado pertence
                //options.Cookie.Domain
                //define o tempo de vida de um cookie HTTP
                //options.Cookie.Expiration
                //indica se o cookie pode ser acessado pelo client side
                options.Cookie.HttpOnly = true;
                //define o nome do cookie
                options.Cookie.Name = ".AspNetCore.Cookies";
                //define o caminho do cookie
                //options.Cookie.Path
                //define o atributo samesite, cookies que não devem ser anexados a solicitações
                //(strict e lax)
                options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax;
                //configurações de security policy
                options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.SameAsRequest;
                //define os cookies do response - deve definir a classe que faça implementação do ICookieManager
                //options.CookieManager
                //deve ter uma classe que implementa de IDataProtectProvider 
                //é provider para o cookie autentication handler
                //options.DataProtectionProvider.
                //chama o metodo do provider para dar permissão
                //options.Events
                //responsavel por obter a instancia dos eventos deve ter uma classe que herda de AuthenticationSkinsOptions
                //options.EventsType
                //controla quanto tempo o ticket de autenticação no cookie sera válido a partir da data de criação
                options.ExpireTimeSpan = TimeSpan.FromDays(14);
                //define o caminho onde o login deve ser realizado quando o acesso precisa de autorização
                options.LoginPath = "/Account/Login";
                //define o caminho onde o logout deve ser realizado quando o acesso precisa de autorização
                options.LogoutPath = "/Account/Logout";
                //nome do parametro que recebera a url para a redirecionamento apos o login
                options.ReturnUrlParameter = "ReturnUrl";
                //define o container opcional ao qual armazenara a identidade do usuario nas requisições
                //options.SessionStore
                //quando habilitado um novo cookie sera criado com uma nova hora de expiração
                //quando o cookie atual tiver ultrapassado a metade do tempo de expiração
                options.SlidingExpiration = true;
                //utilizado para proteger ou não as informações do cookie
                //options.TicketDataFormat
            });

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
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
