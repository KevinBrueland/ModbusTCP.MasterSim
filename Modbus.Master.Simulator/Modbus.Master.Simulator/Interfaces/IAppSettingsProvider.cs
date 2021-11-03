
namespace Modbus.Master.Simulator.Interfaces
{
    public interface IAppSettingsProvider
    {
        byte[] GetIPAddress();
        int GetPort();
        byte GetSlaveId();

        int GetMaxRetryCount();
        int GetRetryInterval();
    }
}
