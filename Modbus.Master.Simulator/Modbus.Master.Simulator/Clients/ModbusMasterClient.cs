using Modbus.Master.Simulator.Common;
using Modbus.Master.Simulator.Interfaces;
using NModbus;
using System;
using System.Collections;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Modbus.Master.Simulator.Clients
{
    public class ModbusMasterClient : IModbusMasterClient
    {
        public IPAddress IPAddress { get; set; }
        public int TcpPort { get; set; }
        public byte SlaveId { get; set; }
        public bool IsConnected => _tcpClient.Connected;

        private static IModbusMaster _master;
        private static TcpClient _tcpClient;
        private static ModbusFactory _factory;

        public ModbusMasterClient()
        {
            _factory = new ModbusFactory();
            _tcpClient = new TcpClient();
            _master = _factory.CreateMaster(_tcpClient);
        }
        public async Task AttemptToConnect(IPAddress ipAddress, int tcpPort, byte slaveId)
        {
            var retryCount = 1;

            if (_tcpClient == null)
            {
                _tcpClient = new TcpClient();
                _master = _factory.CreateMaster(_tcpClient);
            }

            while (!_tcpClient.Connected)
            {
                try
                {
                    ConsoleHelper.Info($"Trying to connect to slaveId {slaveId} on IP address: {ipAddress}:{tcpPort} - Connection attempt {retryCount}");
                    await _tcpClient.ConnectAsync(ipAddress, tcpPort);
                    ConsoleHelper.Success($"Master connected to slaveId {slaveId} on IP address: {ipAddress}:{tcpPort}");

                    IPAddress = ipAddress;
                    TcpPort = tcpPort;
                    SlaveId = slaveId;
                    break;

                }
                catch (Exception)
                {
                    ConsoleHelper.Error($"Unable to establish connection. Trying again in 5 seconds");
                    await Task.Delay(5000);
                    retryCount++;
                }
            }
        }

        public void Disconnect()
        {
            if (_tcpClient.Connected)
            {
                ConsoleHelper.Info($"Disconnecting from slaveId {SlaveId} on Ip address: {IPAddress}:{TcpPort}");
                _tcpClient.Close();
                _tcpClient = null;
            }
        }

        public async Task ReadCoils(ushort registryStartAddress, ushort numberToRead)
        {
            ConsoleHelper.Info($"Reading coil addresses {registryStartAddress} - {registryStartAddress + numberToRead - 1}:");
            var coilValues = await _master.ReadCoilsAsync(SlaveId, registryStartAddress, numberToRead);

            foreach (var coilValue in coilValues)
            {
                ConsoleHelper.Success($"Coil address: {registryStartAddress++} | Value: {coilValue}");
            }
        }

        public async Task WriteToSingleCoil(ushort registryAddress, bool value)
        {
            ConsoleHelper.Info($"Writing value: {value} to coil address: {registryAddress}");
            await _master.WriteSingleCoilAsync(SlaveId, registryAddress, value);
        }

        public async Task WriteToCoils(ushort registryStartAddress, bool[] values)
        {
            var addressCounter = registryStartAddress;
            ConsoleHelper.Info($"Writing values to coils:");
            for (int i = 0; i < values.Length; i++)
            {
                ConsoleHelper.Info($"Coil address: {addressCounter++} | Value: {values[i]}");
            }
            await _master.WriteMultipleCoilsAsync(SlaveId, registryStartAddress, values);
        }

        public async Task ReadDiscreteInputs(ushort registryStartAddress, ushort numberToRead)
        {
            ConsoleHelper.Info($"Reading discrete input addresses {registryStartAddress} - {registryStartAddress + numberToRead - 1}:");
            var discreteInputValues = await _master.ReadInputsAsync(SlaveId, registryStartAddress, numberToRead);

            foreach (var discreteValues in discreteInputValues)
            {
                ConsoleHelper.Success($"Discrete input address: {registryStartAddress++} | Value: {discreteValues}");
            }
        }

        public async Task ReadInputRegisters(ushort registryStartAddress, ushort numberToRead)
        {
            ConsoleHelper.Info($"Reading input register addresses {registryStartAddress} - {registryStartAddress + numberToRead - 1}:");
            var inputRegisterValues = await _master.ReadInputRegistersAsync(SlaveId, registryStartAddress, numberToRead);
            foreach (var registerValue in inputRegisterValues)
            {
                ConsoleHelper.Success($"Input register address: {registryStartAddress++} | Value: {registerValue}");
            }
        }

        public async Task ReadInputRegistersAsBits(ushort registryStartAddress, ushort numberToRead)
        {
            ConsoleHelper.Info($"Reading bit values from input register addresses {registryStartAddress} - {registryStartAddress + numberToRead - 1}:");
            var inputRegisterValues = await _master.ReadInputRegistersAsync(SlaveId, registryStartAddress, numberToRead);
            foreach (var registerValue in inputRegisterValues)
            {
                var signals = ConvertUshortValueToBinaryString(registerValue);
                ConsoleHelper.Info($"Register address: {registryStartAddress++} | Value: {registerValue}");
                ConsoleHelper.Info("LSB");
                for (int i = 0; i < signals.Length; i++)
                {
                    ConsoleHelper.Success($"Bit{i}: {Convert.ToInt32(signals[i])}");
                }
                ConsoleHelper.Info("MSB");
            }
        }

        public async Task ReadInputRegistersAsFloat(ushort registryStartAddress, ushort numberToRead)
        {
            ConsoleHelper.Info($"Reading float values from input registers addresses {registryStartAddress} - {registryStartAddress + numberToRead - 1}:");
            var inputRegisterValues = await _master.ReadInputRegistersAsync(SlaveId, registryStartAddress, numberToRead);

            for (int i = 0; i < inputRegisterValues.Length; i += 2)
            {
                var floatValue = ConvertUshortsToFloat(inputRegisterValues[i], inputRegisterValues[i + 1]);
                ConsoleHelper.Success($"Input register addresses {registryStartAddress++} - {registryStartAddress++} | Value: {floatValue}");
            }
        }

        public async Task ReadHoldingRegisters(ushort registryStartAddress, ushort numberOfRegistersToRead)
        {
            ConsoleHelper.Info($"Reading holding register addresses {registryStartAddress} - {registryStartAddress + numberOfRegistersToRead - 1}:");
            var holdingRegisterValues = await _master.ReadHoldingRegistersAsync(SlaveId, registryStartAddress, numberOfRegistersToRead);
            foreach (var registerValue in holdingRegisterValues)
            {
                ConsoleHelper.Success($"Input register address: {registryStartAddress++} | Value: {registerValue}");
            }

        }

        public async Task ReadHoldingRegistersAsBits(ushort registryStartAddress, ushort numberToRead)
        {
            ConsoleHelper.Info($"Reading bit values from holding register addresses {registryStartAddress} - {registryStartAddress + numberToRead - 1}:");
            var holdingRegisterValues = await _master.ReadHoldingRegistersAsync(SlaveId, registryStartAddress, numberToRead);
            foreach (var registerValue in holdingRegisterValues)
            {
                var signals = ConvertUshortValueToBinaryString(registerValue);
                ConsoleHelper.Info($"Holding register address: {registryStartAddress++} | Value: {registerValue}");
                ConsoleHelper.Info("LSB");
                for (int i = 0; i < signals.Length; i++)
                {
                    ConsoleHelper.Success($"Bit{i}: {Convert.ToInt32(signals[i])}");
                }
                ConsoleHelper.Info("MSB");
            }
        }

        public async Task ReadHoldingRegistersAsFloat(ushort registryStartAddress, ushort numberToRead)
        {
            numberToRead *= 2;
            ConsoleHelper.Info($"Reading float values from holding registers addresses {registryStartAddress} - {registryStartAddress + numberToRead - 1}:");
            var holdingRegisterValues = await _master.ReadHoldingRegistersAsync(SlaveId, registryStartAddress, numberToRead);

            for (int i = 0; i < holdingRegisterValues.Length; i += 2)
            {
                var floatValue = ConvertUshortsToFloat(holdingRegisterValues[i], holdingRegisterValues[i + 1]);
                ConsoleHelper.Success($"Holding register addresses {registryStartAddress++} - {registryStartAddress++} | Value: {floatValue}");
            }
        }

        public async Task WriteToSingleHoldingRegister(ushort registryAddress, ushort value)
        {
            ConsoleHelper.Info($"Writing value: {value} to holding register address: {registryAddress}");
            await _master.WriteSingleRegisterAsync(SlaveId, registryAddress, value);
        }

        public async Task WriteToHoldingRegisters(ushort registryStartAddress, ushort[] values)
        {
            var addressCounter = registryStartAddress;
            ConsoleHelper.Info($"Writing values to holding registers:");
            for (int i = 0; i < values.Length; i++)
            {
                ConsoleHelper.Info($"Register address: {addressCounter++} | Value: {values[i]}");
            }

            await _master.WriteMultipleRegistersAsync(SlaveId, registryStartAddress, values);
        }

        public async Task WriteBitsToHoldingRegisters(ushort registryStartAddress, string[] values)
        {
            var addressCounter = registryStartAddress;
            ConsoleHelper.Info($"Writing 16 bit binary values to holding registers:");
            for (int i = 0; i < values.Length; i++)
            {
                ConsoleHelper.Info($"Register address: {addressCounter++} | Value: {values[i]} ({Convert.ToInt32(values[i], 2)})");
            }

            var ushortVals = new ushort[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                ushortVals[i] = ConvertBinaryStringToUshort(values[i]);
            }

            await _master.WriteMultipleRegistersAsync(SlaveId, registryStartAddress, ushortVals);
        }

        public async Task WriteFloatsToHoldingRegisters(ushort registryStartAddress, float[] values)
        {
            var addressCounter = registryStartAddress;
            ConsoleHelper.Info($"Writing 32-bit float values to holding registers:");
            for (int i = 0; i < values.Length; i++)
            {
                ConsoleHelper.Info($"Register address: {addressCounter++}-{addressCounter++} | Value: {values[i]}");
            }
            var registerValues = ConvertFloatsToUshorts(values);
            await _master.WriteMultipleRegistersAsync(SlaveId, registryStartAddress, registerValues);
        }

        private float ConvertUshortsToFloat(ushort first, ushort second)
        {

            var firstUshortBytes = BitConverter.GetBytes(first);
            var secondUshortBytes = BitConverter.GetBytes(second);

            var ushortBytes = new byte[firstUshortBytes.Length + secondUshortBytes.Length];

            if (BitConverter.IsLittleEndian)
                ushortBytes = firstUshortBytes.Concat(secondUshortBytes).ToArray();
            else
                ushortBytes = secondUshortBytes.Concat(firstUshortBytes).ToArray();

            var floatVal = BitConverter.ToSingle(ushortBytes, 0);

            return floatVal;
        }

        private ushort[] ConvertFloatsToUshorts(float[] floatValues)
        {
            //two ushorts per float
            var ushorts = new ushort[floatValues.Length * 2];
            var ushortCounter = 0;
            foreach (var floatValue in floatValues)
            {
                var floatBytes = BitConverter.GetBytes(floatValue);


                for (int i = 0; i < floatBytes.Length; i += 2)
                {
                    //get the first two bytes that make up a ushort
                    byte[] ushortBytes = new byte[2];
                    Buffer.BlockCopy(floatBytes, i, ushortBytes, 0, 2);

                    var ushortValue = BitConverter.ToUInt16(ushortBytes, 0);
                    ushorts[ushortCounter++] = ushortValue;
                }


            }

            return ushorts;

        }

        private bool[] ConvertUshortValueToBinaryString(ushort registryValue)
        {
            var bitArray = new BitArray(new int[] { registryValue }).Cast<bool>().ToArray();
            var bit = new bool[16];
            Array.Copy(bitArray, 0, bit, 0, 16);

            return bit;
        }

        private static ushort ConvertBinaryStringToUshort(string binaryString)
        {
            var bits = binaryString.Select(c => c == '1').ToArray();
            BitArray registryBits = new BitArray(bits);
            int[] registryInt = new int[1];

            registryBits.CopyTo(registryInt, 0);
            return (ushort)registryInt[0];
        }
    }
}
