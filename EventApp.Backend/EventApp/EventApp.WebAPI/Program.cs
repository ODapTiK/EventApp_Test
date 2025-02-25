using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using DotNetEnv;

namespace EventApp.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var services = builder.Services;
            var configuration = builder.Configuration;

            configuration.AddEnvironmentVariables();

            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
                cfg.AddProfile(new AssemblyMappingProfile(typeof(IEventAppDbContext).Assembly));
                cfg.AddProfile(new AssemblyMappingProfile(typeof(IParticipantRepository).Assembly));
            });

            services.AddDbContext<EventAppDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("EventAppDB"), npgsqlOptionsAction => 
                    npgsqlOptionsAction.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorCodesToAdd: null)
                    ));
            services.AddScoped<IEventAppDbContext>(provider => provider.GetService<EventAppDbContext>());

            var jwtOptions = new JwtOptions()
            {
                ExpiredMinutes = int.Parse(Environment.GetEnvironmentVariable("JWT_EXPIRED_TIME") ?? throw new InvalidOperationException("JWT_EXPIRED_TIME is not set in environment variables")),
                Key = Environment.GetEnvironmentVariable("JWT_KEY") ?? throw new InvalidOperationException("JWT_KEY is not set in environment variables")
            };

            services.AddApplication();
            services.AddPersistence();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyHeader();
                    policy.AllowAnyMethod();
                    policy.AllowAnyOrigin();
                });
            });

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.IncludeErrorDetails = true;

                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
                    };
                });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
            });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "EventAppAPI", Version = "v1" });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description =
                        "JWT Authorization header using the Bearer scheme. \r\n\r\n" +
                        "Enter 'Bearer' [space] and the your token in the text input below. \r\n\r\n" +
                        "Example: \"Bearer 12casdc1sd\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference()
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                    }

                });
            });
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                });

            services.AddSingleton<ICurrentUserService, CurrentUserService>();
            services.AddHttpContextAccessor();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                try
                {
                    var context = serviceProvider.GetRequiredService<EventAppDbContext>();
                    context.Database.EnsureCreated();
                }
                catch (Exception exception)
                {
                    //Log.Fatal(exception, "An error occured while app initialization");
                }
            }

            app.UseExceptionHandler();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors("AllowAll");
            app.UseSwagger();
            app.UseSwaggerUI(config =>
            {
                config.RoutePrefix = string.Empty;
                config.SwaggerEndpoint("/swagger/v1/swagger.json", "Event App API V1");
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.Run();
        }
    }
}
