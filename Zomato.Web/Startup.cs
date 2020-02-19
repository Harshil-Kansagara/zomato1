using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zomato.DomainModel.Data;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Zomato.Repository.UnitofWork;
using Zomato.Repository.RestCuisineRepository;
using Zomato.Repository.UserRepository;
using Zomato.Repository.UserAddressRepository;
using Zomato.Repository.ReviewRepository;
using Zomato.Repository.FollowRepository;
using Zomato.Repository.CommentRepository;
using Zomato.Repository.CuisineRepository;
using Zomato.Repository.CategoryRepository;
using Zomato.Repository.LikeRepository;
using Zomato.Repository.MenuRepository;
using Zomato.Repository.OrderedItemRepository;
using Zomato.Repository.OrderRepository;
using Zomato.Repository.RestaurantRepository;
using Zomato.Repository.RestCategoryRepository;
using Zomato.Repository.RestaurantLocationRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Zomato.DomainModel.Models;
using Zomato.Repository.NotificationRepository;
using Zomato.Core.Hubs;
using Zomato.Repository.DataRepository;
using Microsoft.Extensions.FileProviders;
using System.IO;

namespace Zomato.Web
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

            services.Configure<ApplicationSettingModel>(Configuration.GetSection("ApplicationSettings"));

            
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<ICuisineRepository, CuisineRepository>();
            services.AddScoped<IFollowRepository, FollowRepository>();
            services.AddScoped<ILikeRepository, LikeRepository>();
            services.AddScoped<IMenuRepository, MenuRepository>();
            services.AddScoped<IOrderedItemRepository, OrderedItemRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IRestLocationRepository, RestLocationRepository>();
            services.AddScoped<IRestaurantRepository, RestaurantRepository>();
            services.AddScoped<IRestCategoryRepository, RestCategoryRepository>();
            services.AddScoped<IRestCuisineRepository, RestCuisineRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IUserAddressRepository, UserAddressRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IOrderNotificationRepository, OrderNotificationRepository>();
            services.AddScoped<IDataRepository, DataRepository>();

            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(
                    Configuration.GetConnectionString("zomato"),
                    b => b.MigrationsAssembly("Zomato.DomainModel")));

            //services.AddIdentityCore<IdentityUser>().AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddCors(o => o.AddPolicy("CorsPolicy", builder => {
                builder
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                //.WithOrigins("http://localhost:59227/OrderHub");
                .WithOrigins("https://zomato.us-west-1.elasticbeanstalk.com/");
            }));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            var key = Encoding.UTF8.GetBytes(Configuration["ApplicationSettings:JWT_Secret"].ToString());

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = false;
                x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };

                x.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        // If the request is for our hub...
                        var httpContextRequestPath = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) &&
                            (httpContextRequestPath.StartsWithSegments("/OrderHub")))
                        {
                            // Read the token out of the query string
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            services.AddSignalR();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ApplicationDbContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseHsts();
            }

            context.Database.Migrate();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
           
            app.UseAuthentication();

            app.UseCors("CorsPolicy");

            app.UseSignalR(routes =>
            {
                routes.MapHub<OrderHub>("/OrderHub");
            });

            //app.UseMvc();
            app.UseMvc(routes =>
            {
                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
                //routes.MapRoute(
                //    name: "default",
                //    template: "{controller=Home}/{action=Index}/{id?}");
            });

            //app.UseSpa(spa =>
            //{
            //    spa.Options.SourcePath = "app";
            //    if (env.IsDevelopment())
            //    {
            //        spa.UseAngularCliServer(npmScript: "start");
            //    }
            //});


        }
    }
}
