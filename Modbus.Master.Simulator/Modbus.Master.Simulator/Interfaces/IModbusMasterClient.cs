using Modbus.Master.Simulator.Clients;
using Modbus.Master.Simulator.Types;
using System.Net;
using System.Threading.Tasks;

namespace Modbus.Master.Simulator.Interfaces
{
    public interface IModbusMasterClient
    {
        IPAddress IPAddress { get; }
        int TcpPort { get; }
        byte SlaveId { get; set; }
        int MaxRetryCount { get; }
        int RetryInterval { get; }
        bool IsConnected { get; }
        Task AttemptToConnect(IPAddress ipAddress, int tcpPort, byte slaveId, ModbusMasterOptions options);
        Task AttemptToConnect(IPAddress ipAddress, int tcpPort, byte slaveId);
        void Disconnect();
        Task ReadCoils(ushort registryStartAddress, ushort numberOfCoilsToRead);
        Task WriteToSingleCoil(ushort registryAddress, bool value);
        Task WriteToCoils(ushort registryStartAddress, bool[] values);

        Task ReadDiscreteInputs(ushort registryStartAddress, ushort numberToRead);
        Task ReadInputRegisters(ushort registryStartAddress, ushort numberOfRegistersToRead);
        Task ReadInputRegistersAsBits(ushort registryStartAddress, ushort numberOfRegistersToRead);
        Task ReadInputRegistersAsFloat(ushort registryStartAddress, ushort numberOfRegistersToRead);
        Task WriteToSingleHoldingRegister(ushort registryAddress, ushort value);
        Task WriteToHoldingRegisters(ushort registryAddress, ushort[] values);
        Task WriteBitsToHoldingRegisters(ushort registryStartAddress, string[] values);
        Task WriteFloatsToHoldingRegisters(ushort registryStartAddress, float[] valuesToWrite);
        Task ReadHoldingRegisters(ushort registryStartAddress, ushort numberOfRegistersToRead);
        Task ReadHoldingRegistersAsBits(ushort registryStartAddress, ushort numberOfRegistersToRead);
        Task ReadHoldingRegistersAsFloat(ushort registryStartAddress, ushort numberOfRegistersToRead);
    }
}
