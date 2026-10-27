using Autofac;
using eShopLegacyWebForms.Models;
using eShopLegacyWebForms.Models.Infrastructure;
using eShopLegacyWebForms.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace eShopLegacyWebForms.Modules
{
    public class ApplicationModule : Module
    {
        private bool useMockData;
        private readonly IConfiguration _configuration;

        public ApplicationModule(bool useMockData)
        {
            this.useMockData = useMockData;
        }

        public ApplicationModule(bool useMockData, IConfiguration configuration)
        {
            this.useMockData = useMockData;
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            if (this.useMockData)
            {
                builder.RegisterType<CatalogServiceMock>()
                    .As<ICatalogService>()
                    .SingleInstance();
            }
            else
            {
                builder.RegisterType<CatalogService>()
                    .As<ICatalogService>()
                    .InstancePerLifetimeScope();

                builder.Register(c =>
                {
                    var optionsBuilder = new DbContextOptionsBuilder<CatalogDBContext>();
                    var connectionString = _configuration.GetConnectionString("CatalogDBContext");
                    optionsBuilder.UseSqlServer(connectionString);
                    return new CatalogDBContext(optionsBuilder.Options);
                })
                .InstancePerLifetimeScope();

                builder.Register(c => new CatalogDBInitializer(
                    c.Resolve<CatalogItemHiLoGenerator>(),
                    c.Resolve<IConfiguration>()))
                    .InstancePerLifetimeScope();
            }

            builder.RegisterType<CatalogItemHiLoGenerator>()
                .SingleInstance();
        }
    }
}
