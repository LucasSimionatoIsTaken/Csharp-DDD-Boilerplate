using System.Reflection;
using System.Text;
using API.SeedWork.Filters.Swagger;
using Application.SeedWork.Responses;
using Application.Services.User;
using Core;
using FluentValidation;
using Infrastructure.Contexts;
using Infrastructure.Options;
using Infrastructure.Repositories;
using Infrastructure.SeedWork.UnitOfWork;
using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace API.Extensions;

public static class IServiceCollectionExtension
{
    private static void AddDbContext(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env)
    {
        if (!env.IsEnvironment("Test"))
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Environment.GetEnvironmentVariable("CONTAINER_ENVIRONMENT") == "true"
                        ? configuration.GetConnectionString("Container")
                        : configuration.GetConnectionString("Default"),
                    sqlOptions => sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
            });
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        var assembly = typeof(GenericRepository<>).Assembly;

        var types = assembly.GetTypes()
            .Where(t => t.IsClass &&
                        !t.IsAbstract &&
                        t.BaseType != null &&
                        t.BaseType.IsGenericType &&
                        t.BaseType.GetGenericTypeDefinition() == typeof(GenericRepository<>));

        foreach (var type in types)
        {
            var interfaces = type.GetInterfaces();
            foreach (var @interface in interfaces)
                services.AddScoped(@interface, type);
        }
    }


    private static void AddUnitOfWork(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    private static void AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1"
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);

            options.CustomSchemaIds(type => (type.FullName ?? type.Name).Replace("+", "."));
            
            options.EnableAnnotations();

            options.OperationFilter<UnauthorizedAndForbiddentDescriptionFilter>();

            var jwtSecurityScheme = new OpenApiSecurityScheme()
            {
                Scheme = "Bearer",
                BearerFormat = "JWT",
                Name = "JWT Authentication",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };

            options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

            options.MapType<TimeSpan>(() => new OpenApiSchema
            {
                Type = "string",
                Example = new OpenApiString("00:00:00")
            });

            options.MapType<TimeSpan?>(() => new OpenApiSchema
            {
                Type = "string",
                Example = new OpenApiString("00:00:00")
            });
        });
    }

    private static void AddControllersAndFilers(this IServiceCollection services)
    {
        services.AddControllers(options => { options.Filters.Add<BaseResponseResultFilter>(); })
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            });
    }

    private static void ConfigureFluentValidation(this IServiceCollection services)
    {
        AssemblyScanner.FindValidatorsInAssembly(typeof(BaseResponse<>).Assembly, true)
            .ForEach(e => services.AddTransient(e.InterfaceType, e.ValidatorType));
    }

    private static void AddMappings(this IServiceCollection services)
    {
        TypeAdapterConfig.GlobalSettings.Scan(typeof(BaseResponse<>).Assembly);
    }

    private static void AddBearerTokenSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<AuthTokenOptions>().Bind(configuration.GetSection(nameof(AuthTokenOptions)));

        services.AddAuthentication(op =>
            {
                op.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer("Bearer", op =>
            {
                var authConfig = configuration.GetSection(nameof(AuthTokenOptions))
                    .Get<AuthTokenOptions>()!;

                op.RequireHttpsMetadata = false;
                op.SaveToken = true;

                op.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authConfig.Key)),

                    ValidateAudience = true,
                    ValidAudience = authConfig.Audience,

                    ValidateIssuer = true,
                    ValidIssuer = authConfig.Issuer,

                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                op.Events = new JwtBearerEvents
                {
                    OnChallenge = a =>
                    {
                        a.HttpContext.Response.StatusCode = 401;
                        return Task.CompletedTask;
                    },
                };
            });

        services.AddCors(setup => setup
            .AddDefaultPolicy(policy =>
                policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()));
    }

    /// <summary>
    /// Adds dependency injections
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static void AddDependencyInjections(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env)
    {
        services.AddSwagger();

        services.ConfigureFluentValidation();

        services.AddDbContext(configuration, env);
        services.AddRepositories();
        services.AddUnitOfWork();

        services.AddBearerTokenSettings(configuration);

        services.AddMappings();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<BaseResponse<object>>());

        services.AddControllersAndFilers();
    }
}