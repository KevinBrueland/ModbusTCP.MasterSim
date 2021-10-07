using System.Net;
using System.Threading.Tasks;

namespace Modbus.Master.Simulator.Interfaces
{
    public interface IModbusMasterClient
    {
        IPAddress IPAddress { get; set; }
        int TcpPort { get; set; }
        byte SlaveId { get; set; }
        bool IsConnected { get; }
        Task AttemptToConnect(IPAddress ipAddress, int tcpPort, byte slaveId);
        void Disconnect();
        Task ReadCoils(ushort registryStartAddress, ushort numberOfCoilsToRead);
        Task WriteToSingleCoil(ushort registryAddress, bool value);
        Task WriteToCoils(ushort registryStartAddress, bool[] values);
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
