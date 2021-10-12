using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modbus.Master.Simulator.Clients;
using Modbus.Master.Simulator.Interfaces;
using Modbus.Master.Simulator.Parsers;
using Modbus.Master.Simulator.Providers;
using StructureMap;
using System.IO;
using System.Threading.Tasks;

namespace Modbus.Master.Simulator
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var services = ConfigureServices();

            var serviceProvider = services.BuildServiceProvider();

            // calls the Run method in App, which is replacing Main
            await serviceProvider.GetService<ModbusMasterSim>().Run();
        }

        private static IServiceCollection ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();

            var config = LoadConfiguration();
            services.AddSingleton(config);

            services.AddScoped<IModbusMasterClient, ModbusMasterClient>();
            services.AddScoped<IInputArgumentParser, InputArgumentParser>();
            services.AddSingleton<IAppSettingsProvider, AppSettingsProvider>();
            services.AddTransient<ModbusMasterSim>();

            return services;
        }

        public static IConfiguration LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false);

            return builder.Build();
        }
    }
}
