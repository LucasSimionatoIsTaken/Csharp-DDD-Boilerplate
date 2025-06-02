using System.Reflection;
using API.SeedWork.Filters;
using Application.SeedWork.Responses;
using Application.User;
using FluentValidation;
using Infrastructure.Contexts;
using Infrastructure.Repositories;
using Infrastructure.Repositories.UserRepository;
using Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace API.Extensions;

public static class IServiceCollectionExtension
{
    private static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<Context>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("Default"));
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

        services.AddScoped<IUserRepository, UserRepository>();
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
            
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            
            options.CustomSchemaIds(type => type.FullName!.Replace("+", "."));
        });
    }

    private static void AddControllersAndFilers(this IServiceCollection services)
    {
        services.AddControllers(options =>
        {
            options.Filters.Add<BaseResponseResultFilter>();
        })
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

    /// <summary>
    /// Adds dependency injections
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static void AddDependencyInjections(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOpenApi();
        services.AddSwagger();

        services.ConfigureFluentValidation();
        
        services.AddDbContext(configuration);
        services.AddRepositories();
        services.AddUnitOfWork();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<ListAll>());

        services.AddControllersAndFilers();
    }
}