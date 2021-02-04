using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Kopyw.Data;
using Kopyw.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Kopyw.Services.DTOs.Interfaces;
using Kopyw.Services.DTOs;
using Kopyw.Services.DataAccess;
using Kopyw.Services.DataAccess.Interfaces;
using Kopyw.Services;
using AutoMapper;
using Kopyw.DTOs;
using Microsoft.CodeAnalysis.Options;
using Kopyw.Hubs;
using Kopyw.Services.Notifiers.Interfaces;
using Kopyw.Services.Notifiers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Kopyw.Middleware;
using Kopyw.Services.Converters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;

namespace Kopyw
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<ApplicationUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = false;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddScoped<UserFinder, UserFinder>();
            services.AddScoped<IPostDTOManager, PostDTOManager>();
            services.AddScoped<ICommentDTOManager, CommentDTOManager>();
            services.AddScoped<IFollowDTOManager, FollowDTOManager>();
            services.AddScoped<IUserStatsDTOManager, UserStatsDTOManager>();
            services.AddScoped<IConversationDTOManager, ConversationDTOManager>();
            services.AddScoped<IFollowManager, FollowManager>();
            services.AddScoped<IPostManager, PostManager>();
            services.AddScoped<ICommentManager, CommentManager>();
            services.AddScoped<IUserStatsManager, UserStatsManager>();
            services.AddScoped<IPostNotifier, PostNotifier>();
            services.AddScoped<IConversationManager, ConversationManager>();
            services.AddScoped<IMessageNotifier, MessageNotifier>();


            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            if (Environment.IsProduction())
            {
                services.AddIdentityServer(options =>
                {
                    options.IssuerUri = "https://kopyw.azurewebsites.net";
                })
                    .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();
            }
            else
            {
                services.AddIdentityServer()
                    .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();
            }

            services.AddAuthentication()
                .AddIdentityServerJwt();

            services.AddControllersWithViews()
                .AddJsonOptions(conf =>
                {
                    conf.JsonSerializerOptions.Converters.Add(new DateTimeUtcConverter());
                });
            services.AddRazorPages();
            services.AddSignalR();


            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseMiddleware<WebSocketsQueryToken>();
            app.UseRouting();
            app.UseAuthentication();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
                endpoints.MapHub<PostSubscriptionHub>("/posthub");
                endpoints.MapHub<MessageHub>("/messagehub");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
