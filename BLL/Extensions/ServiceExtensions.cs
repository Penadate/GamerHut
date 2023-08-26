using BLL.Mappings;
using BLL.Services;
using BLL.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Extensions
{
    public static class DependencyRegistrar
    {
        public static IServiceCollection ConfigureBusinessLayerServices(
            this IServiceCollection services)
        {
            services.AddScoped<IGameService, GameService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();

            services.ConfigureAutomapper();
            return services;
        }

        private static IServiceCollection ConfigureAutomapper(
            this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetAssembly(typeof(MappingProfile)));
            return services;
        }
    }
}
