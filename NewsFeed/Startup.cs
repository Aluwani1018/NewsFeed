using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NewsFeed.Business;
using NewsFeed.Business.Interfaces;
using NewsFeed.Business.Manager;
using NewsFeed.Store.EF;
using Swashbuckle.AspNetCore.Swagger;

namespace NewsFeed
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //Inject Database Connection
            services.AddDbContext<NewsFeedDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("NewsFeedConnection"), b => b.MigrationsAssembly("NewsFeed.WebApi")));

            //inject connection strings
            services.AddScoped<IHeadlinesManager, HeadlineManage>();
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            // Add Cors policy to "open" access to specific websites other than this. 
            // This can be set per Action, Controller or Globally.
            // WithOrigins take one or more URLs separated by comma. The URLs must be specified without a trailing slash (/).
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                            .WithOrigins(Configuration["CorsPolicy:Origins"])
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials());
            });

            //Add Swagger UI to generate Api UI on the 
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "News Feed WebApi", Version = "v1.0" });
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme()
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    { "Bearer", new string[] { } }
                });
            });

            // Add Policy to use expose Jwt bearer Authentication - [Authorize(Policy = "NewsFeed")]
            services.AddAuthorization(options =>
            {
                options.AddPolicy("NewsFeed", policy =>
                {
                    policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireAuthenticatedUser();
                });
            });

            services.AddMvcCore().AddApiExplorer();
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
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/v1/swagger.json", "News Feed WebApi V1.0");
            });
        }
    }
}
