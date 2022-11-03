using System.Text;
using AuthenticationService.Core.Services.interfaces;
using AuthenticationService.Dal;
using AuthenticationService.Dal.Models;
using AuthenticationService.Grpc.Agents;
using AuthenticationService.Grpc.Agents.Interfaces;
using GenericDal;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace AuthenticationService.Api
{
    public class Startup
    {
        readonly string AllowOriginsKey = "_allowOriginsKey";
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string server = Configuration.GetSection("MySql").GetValue<string>("Server");
            string username = Configuration.GetSection("Mysql").GetValue<string>("Username");
            string password = Configuration.GetSection("Mysql").GetValue<string>("Password");
            string database = Configuration.GetSection("Mysql").GetValue<string>("Database");
            string connectionString = $"server={server};user={username};password={password};database={database}";
            
            services.AddDbContext<AuthenticationServiceContext>(builder =>
                {
                    builder.UseLoggerFactory(new NullLoggerFactory());
                    builder.UseMySQL(connectionString);
                }
            );
            
            //repositories
            services.AddTransient<IAsyncRepository<Account, long>, BaseAsyncRepository<Account, long, AuthenticationServiceContext>>();
            
            //services
            services.AddTransient<IAuthenticationService, Core.Services.AuthenticationService>();

            //agents
            services.AddTransient<IAccountAgent, AccountAgent>();
            
            //Authentication
            string secret = Configuration.GetSection("AppSettings")["Secret"];
            services.AddAuthentication(configureOptions =>
            {
                configureOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                configureOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(configureOptions =>
            {
                configureOptions.RequireHttpsMetadata = false;
                configureOptions.SaveToken = false;
                configureOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret)),
                    ValidateAudience = false,
                    ValidateIssuer = false
                };
            });

            services.AddControllers();
            services.AddCors(options =>
            {
                options.AddPolicy(name: AllowOriginsKey,
                    builder =>
                    {
                        builder
                            .AllowCredentials()
                            .AllowAnyHeader()
                            .WithOrigins(
                                "https://kantilever.store",
                                "https://backoffice.kantilever.store",
                                "http://localhost:4200",
                                "http://localhost:4201");
                    });
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "AuthenticationService.Api", Version = "v1"});
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AuthenticationServiceContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CustomerAuthService.Api v1"));
            }
            
            context.Database.EnsureCreated();

            app.UseHttpsRedirection();
            
            app.UseCors(AllowOriginsKey);

            app.UseAuthentication();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}