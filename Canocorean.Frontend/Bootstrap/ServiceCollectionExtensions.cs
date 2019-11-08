using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AutoMapper;
using Canocorean.Frontend.Bootstrap.Interceptors;
using Canocorean.Infrastructure.EntityFramework;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Serilog;
using Swashbuckle.AspNetCore.Swagger;

namespace Canocorean.Frontend.Bootstrap
{
    public static class ServiceCollectionExtensions
    {
        private static readonly Func<ActionContext, IActionResult> CastMvcValidationResponseToFluentValidationResponse = context =>
        {
            var errors = context.ModelState
                .Where(ms => ms.Value.ValidationState == ModelValidationState.Invalid)
                .Select(ms => new ValidationFailure(ms.Key, ms.Value.Errors.First().ErrorMessage));

            Log.Error("ModelBinding error {@errors}", errors);

            return new ObjectResult(errors)
            {
                StatusCode = StatusCodes.Status400BadRequest
            };
        };

        public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services)
        {
            var dbContextOptions = new DbContextOptionsBuilder<CanocoreanDbContext>().UseInMemoryDatabase(databaseName: "Canocorean").Options;
            services.AddTransient(isp => new CanocoreanDbContext(dbContextOptions));
            return services;
        }
        public static IServiceCollection AddFrontEndAPI(this IServiceCollection services)
        {
            services.AddMvc(options => { options.Filters.Add(typeof(APIExceptionFilter)); })
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ConfigureSerializationForFrontend();
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = CastMvcValidationResponseToFluentValidationResponse;
                });
            return services;
        }
        public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = Assembly.GetExecutingAssembly().GetName().Name
                });
                c.DescribeAllEnumsAsStrings();
                c.SchemaFilter<AllPropertiesRequiredFilter>();
                c.CustomSchemaIds(MergeNestedClassesNamesStrategy);
            });
            return services;
        }
        public static IServiceCollection ConfigureValidation(this IServiceCollection services)
        {
                GetAssembliesToScanLocalDependencies()
                .SelectMany(a => a.GetTypes())
                .Where(t => !t.IsAbstract && typeof(IValidator).IsAssignableFrom(t))
                .ToList()
                .ForEach(vtype =>
                {
                    services.AddTransient(vtype).As(vtype.BaseType);
                });
            return services;
        }
        public static IServiceCollection ConfigureAutoMapper(this IServiceCollection services)
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.ClearPrefixes();
                cfg.RecognizePrefixes("temp");
                cfg.AddMaps(GetAssembliesToScanLocalDependencies().ToArray());
            });
            configuration.CompileMappings();
            configuration.AssertConfigurationIsValid();

            services.AddSingleton(configuration.CreateMapper());
            return services;
        }

        private static IEnumerable<Assembly> GetAssembliesToScanLocalDependencies()
        {
            return typeof(ServiceCollectionExtensions).Assembly.GetReferencedAssemblies()
                .Where(assemblyName => assemblyName.Name.StartsWith("Canocorean.")).Select(Assembly.Load);
        }

        private static void ConfigureSerializationForFrontend(this JsonSerializerSettings settings)
        {
            settings.Converters.Add(new StringEnumConverter());
            settings.Converters.Add(new IsoDateTimeConverter());
        }
        private static string MergeNestedClassesNamesStrategy(Type currentClass)
        {
            var resultName = currentClass.IsInterface && currentClass.Name.StartsWith("I")
                ? currentClass.Name.Substring(1, currentClass.Name.Length - 1)
                : currentClass.Name;

            if (currentClass.IsNested)
            {
                // ReSharper disable once PossibleNullReferenceException
                var nameWithNestedClasses = currentClass.FullName
                    // ReSharper disable once AssignNullToNotNullAttribute
                    .Replace(currentClass.Namespace, "")
                    .Replace("Model+", "")
                    .Replace("Info+", "")
                    .Replace("State+", "")
                    .Replace("+", "");

                resultName = nameWithNestedClasses.Substring(0, nameWithNestedClasses.Length - currentClass.Name.Length) + resultName;
            }

            // ReSharper disable once InvertIf
            if (currentClass.GenericTypeArguments.Any())
            {
                var nameBuilder = new StringBuilder(resultName.Substring(0, resultName.IndexOf('`')));
                currentClass.GenericTypeArguments.Select(t => t.Name).ToList().ForEach(n => nameBuilder.Append($"[{n}]"));
                resultName = nameBuilder.ToString();
            }

            return resultName;
        }
        private static void As<T>(this IServiceCollection services, T type) where T : Type
        {
            var previousRegistration = services.LastOrDefault();
            if (previousRegistration == null)
            {
                throw new InvalidOperationException("Previous registration was not found");
            }

            ServiceDescriptor serviceDescriptor;
            if (previousRegistration.ImplementationInstance != null)
            {
                serviceDescriptor = new ServiceDescriptor(type, previousRegistration.ImplementationInstance);
            }
            else if (previousRegistration.ImplementationFactory != null)
            {
                serviceDescriptor = new ServiceDescriptor(type, previousRegistration.ImplementationFactory, previousRegistration.Lifetime);
            }
            else if (previousRegistration.ImplementationType != null)
            {
                serviceDescriptor = new ServiceDescriptor(type, previousRegistration.ImplementationType, previousRegistration.Lifetime);
            }
            else
            {
                throw new NotImplementedException("Overloaded constructor was not found for previousRegistration");
            }
            services.Add(serviceDescriptor);
        }
    }
}
