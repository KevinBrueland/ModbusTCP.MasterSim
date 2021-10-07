using Microsoft.Extensions.Configuration;
using Modbus.Master.Simulator.Interfaces;
using System;
using System.ComponentModel;
using System.Linq;

namespace Modbus.Master.Simulator.Providers
{
    public class AppSettingsProvider : IAppSettingsProvider
    {
        private readonly IConfiguration _configuration;
        public AppSettingsProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        private string Get(string key)
        {
            try
            {
                return _configuration.GetValue<string>($"AppSettings:{key}");
            }
            catch (Exception exception)
            {
                throw new Exception($"Unable to fetch configuration value for key: {key}", exception);
            }

        }

        private T Get<T>(string key)
        {
            try
            {
                var appSettingsValue = _configuration.GetValue<string>($"AppSettings:{key}");

                var converter = TypeDescriptor.GetConverter(typeof(T));
                return (T)(converter.ConvertFromInvariantString(appSettingsValue));
            }
            catch (Exception exception)
            {
                throw new Exception($"Unable to fetch configuration value for key: {key}", exception);
            }

        }

        public byte[] GetIPAddress()
        {
            var ipAddressParts = Get<string>("SlaveIPAddress").Split('.');
            var ipAddress = ipAddressParts.Select(c => Convert.ToByte(c)).ToArray();

            return ipAddress;
        }

        public int GetPort()
        {
            var port = Get<int>("SlaveTcpPort");
            return port;
        }

        public byte GetSlaveId()
        {
            var port = Get<byte>("SlaveId");
            return port;
        }
    }
}
