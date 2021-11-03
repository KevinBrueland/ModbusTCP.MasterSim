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

        private T GetOrDefault<T>(string key, T defaultValue)
        {
            try
            {
                var appSettingsValue = _configuration.GetValue<string>($"AppSettings:{key}");

                var converter = TypeDescriptor.GetConverter(typeof(T));
                return (T)(converter.ConvertFromInvariantString(appSettingsValue));
            }
            catch (Exception exception)
            {
                return defaultValue;
            }

        }

        public byte[] GetIPAddress()
        {
            var ipAddressParts = GetOrDefault<string>("SlaveIPAddress", "127.0.0.1").Split('.');
            var ipAddress = ipAddressParts.Select(c => Convert.ToByte(c)).ToArray();

            return ipAddress;
        }

        public int GetPort()
        {
            var port = GetOrDefault<int>("SlaveTcpPort", 502);
            return port;
        }

        public byte GetSlaveId()
        {
            var port = GetOrDefault<byte>("SlaveId", 1);
            return port;
        }

        public int GetMaxRetryCount()
        {
            var retryCount = GetOrDefault<int>("MaxRetryCount", 10);
            return retryCount;
        }

        public int GetRetryInterval()
        {
            var retryInterval = GetOrDefault<int>("RetryInterval", 5000);
            return retryInterval;
        }
    }
}
