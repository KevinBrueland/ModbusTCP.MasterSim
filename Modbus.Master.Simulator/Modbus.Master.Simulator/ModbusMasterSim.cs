
using Modbus.Master.Simulator.Common;
using Modbus.Master.Simulator.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Modbus.Master.Simulator
{
    class ModbusMasterSim
    {
        private readonly IModbusMasterClient _modbusMasterClient;
        private readonly IAppSettingsProvider _appSettingsProvider;
        private readonly IInputArgumentParser _inputParser;

        public ModbusMasterSim(IModbusMasterClient modbusMasterClient, IAppSettingsProvider appSettingsProvider, IInputArgumentParser inputParser)
        {
            _modbusMasterClient = modbusMasterClient;
            _appSettingsProvider = appSettingsProvider;
            _inputParser = inputParser;
        }

        public async Task Run()
        {
            Console.WriteLine();

            IPAddress ipAddress = new IPAddress(_appSettingsProvider.GetIPAddress());
            var tcpPort = _appSettingsProvider.GetPort();
            byte slaveId = _appSettingsProvider.GetSlaveId();
            var retryCount =_modbusMasterClient.MaxRetryCount = _appSettingsProvider.GetMaxRetryCount();
            var retryInterval = _modbusMasterClient.RetryInterval = _appSettingsProvider.GetRetryInterval();
            
            ConsoleHelper.Info("Found default connection values:");
            ConsoleHelper.Info($"IpAddress: {ipAddress}");
            ConsoleHelper.Info($"TcpPort: {tcpPort}");
            ConsoleHelper.Info($"SlaveId: {slaveId}");
            ConsoleHelper.Info($"RetryCount: {retryCount}");
            ConsoleHelper.Info($"RetryInterval: {retryInterval}");
            
            if (!_modbusMasterClient.IsConnected)
                await _modbusMasterClient.AttemptToConnect(ipAddress, tcpPort, slaveId);

            int exitCode = 0;
            while (exitCode != -1)
            {
                
                Console.WriteLine();

                Console.WriteLine("Please enter a command. For help, type 'help'");
                try
                {
                    ConsoleHelper.Question("Command: ", false);
                    var command = Console.ReadLine()?.ToUpper() ?? "";
                    var executiveCommand = command.Split(new string[] { "--" }, StringSplitOptions.None)[0].TrimEnd();


                    //READ COILS
                    if (executiveCommand.Equals("READ COILS"))
                    {
                        var registerStartAddress = _inputParser.Parse<ushort>(command, "STARTADDRESS");
                        var numberToRead = _inputParser.Parse<ushort>(command, "NUMBERTOREAD");

                        if (registerStartAddress.IsValid && numberToRead.IsValid)
                        {
                            await _modbusMasterClient.ReadCoils(registerStartAddress.Value, numberToRead.Value);
                        }
                        else
                        {
                            var errors = registerStartAddress.Errors.Concat(numberToRead.Errors);
                            ConsoleHelper.Error(string.Join(Environment.NewLine, errors));
                        }
                    }

                    //WRITE TO SINGLE COIL
                    else if (executiveCommand.Equals("WRITE COIL"))
                    {
                        var registerAddress = _inputParser.Parse<ushort>(command, "ADDRESS");
                        var inputValue = _inputParser.Parse<bool>(command, "VALUE");


                        if (registerAddress.IsValid && inputValue.IsValid)
                        {
                            await _modbusMasterClient.WriteToSingleCoil(registerAddress.Value, inputValue.Value);
                        }
                        else
                        {
                            var errors = registerAddress.Errors.Concat(inputValue.Errors);
                            ConsoleHelper.Error(string.Join(Environment.NewLine, errors));
                        }

                    }

                    //WRITE MULTIPLE COILS
                    else if (executiveCommand.Equals("WRITE COILS"))
                    {
                        var registerAddress = _inputParser.Parse<ushort>(command, "STARTADDRESS");
                        var inputValues = _inputParser.ParseMultiple<bool>(command, "VALUES");

                        if (registerAddress.IsValid && inputValues.All(x => x.IsValid))
                        {
                            var coilsToWrite = inputValues.Select(v => v.Value).ToArray();
                            await _modbusMasterClient.WriteToCoils(registerAddress.Value, coilsToWrite);
                        }
                        else
                        {
                            var inputErrors = inputValues.SelectMany(e => e.Errors).ToList();
                            var errors = registerAddress.Errors.Concat(inputErrors);
                            ConsoleHelper.Error(string.Join(Environment.NewLine, errors));
                        }

                    }

                    //READ DISCRETE INPUTS
                    else if (executiveCommand.Equals("READ DISCRETES"))
                    {
                        var registerStartAddress = _inputParser.Parse<ushort>(command, "STARTADDRESS");
                        var numberToRead = _inputParser.Parse<ushort>(command, "NUMBERTOREAD");

                        if (registerStartAddress.IsValid && numberToRead.IsValid)
                        {
                            await _modbusMasterClient.ReadDiscreteInputs(registerStartAddress.Value, numberToRead.Value);
                        }
                        else
                        {
                            var errors = registerStartAddress.Errors.Concat(numberToRead.Errors);
                            ConsoleHelper.Error(string.Join(Environment.NewLine, errors));
                        }
                    }

                    //READ INPUT REGISTERS
                    else if (executiveCommand.Equals("READ INPUTREGS"))
                    {
                        var registryStartAddress = _inputParser.Parse<ushort>(command, "STARTADDRESS");
                        var numberToRead = _inputParser.Parse<ushort>(command, "NUMBERTOREAD");

                        if (registryStartAddress.IsValid && numberToRead.IsValid)
                        {
                            await _modbusMasterClient.ReadInputRegisters(registryStartAddress.Value, numberToRead.Value);
                        }
                        else
                        {
                            var errors = registryStartAddress.Errors.Concat(numberToRead.Errors);
                            ConsoleHelper.Error(string.Join(Environment.NewLine, errors));
                        }

                    }

                    //READ INPUT REGISTERS AS BITS
                    else if (executiveCommand.Equals("READ INPUTREGSB"))
                    {
                        var registryStartAddress = _inputParser.Parse<ushort>(command, "STARTADDRESS");
                        var numberToRead = _inputParser.Parse<ushort>(command, "NUMBERTOREAD");

                        if (registryStartAddress.IsValid && numberToRead.IsValid)
                        {
                            await _modbusMasterClient.ReadInputRegistersAsBits(registryStartAddress.Value, numberToRead.Value);
                        }
                        else
                        {
                            var errors = registryStartAddress.Errors.Concat(numberToRead.Errors);
                            ConsoleHelper.Error(string.Join(Environment.NewLine, errors));
                        }

                    }

                    //READ INPUT REGISTERS AS FLOATS
                    else if (executiveCommand.Equals("READ INPUTREGSF"))
                    {
                        var registryStartAddress = _inputParser.Parse<ushort>(command, "STARTADDRESS");
                        var numberToRead = _inputParser.Parse<ushort>(command, "NUMBERTOREAD");

                        if (registryStartAddress.IsValid && numberToRead.IsValid)
                        {
                            await _modbusMasterClient.ReadInputRegistersAsFloat(registryStartAddress.Value, numberToRead.Value);
                        }
                        else
                        {
                            var errors = registryStartAddress.Errors.Concat(numberToRead.Errors);
                            ConsoleHelper.Error(string.Join(Environment.NewLine, errors));
                        }

                    }

                    //READ HOLDING REGISTERS
                    else if (executiveCommand.Equals("READ HOLDREGS"))
                    {
                        var registryStartAddress = _inputParser.Parse<ushort>(command, "STARTADDRESS");
                        var numberToRead = _inputParser.Parse<ushort>(command, "NUMBERTOREAD");

                        if (registryStartAddress.IsValid && numberToRead.IsValid)
                        {
                            await _modbusMasterClient.ReadHoldingRegisters(registryStartAddress.Value, numberToRead.Value);
                        }
                        else
                        {
                            var errors = registryStartAddress.Errors.Concat(numberToRead.Errors);
                            ConsoleHelper.Error(string.Join(Environment.NewLine, errors));
                        }
                    }

                    //READ HOLDING REGISTERS AS BITS
                    else if (executiveCommand.Equals("READ HOLDREGSB"))
                    {
                        var registryStartAddress = _inputParser.Parse<ushort>(command, "STARTADDRESS");
                        var numberToRead = _inputParser.Parse<ushort>(command, "NUMBERTOREAD");

                        if (registryStartAddress.IsValid && numberToRead.IsValid)
                        {
                            await _modbusMasterClient.ReadHoldingRegistersAsBits(registryStartAddress.Value, numberToRead.Value);
                        }
                        else
                        {
                            var errors = registryStartAddress.Errors.Concat(numberToRead.Errors);
                            ConsoleHelper.Error(string.Join(Environment.NewLine, errors));
                        }

                    }

                    //READ HOLDING REGISTERS AS FLOATS
                    else if (executiveCommand.Equals("READ HOLDREGSF"))
                    {
                        var registryStartAddress = _inputParser.Parse<ushort>(command, "STARTADDRESS");
                        var numberToRead = _inputParser.Parse<ushort>(command, "NUMBERTOREAD");

                        if (registryStartAddress.IsValid && numberToRead.IsValid)
                        {
                            await _modbusMasterClient.ReadHoldingRegistersAsFloat(registryStartAddress.Value, numberToRead.Value);
                        }
                        else
                        {
                            var errors = registryStartAddress.Errors.Concat(numberToRead.Errors);
                            ConsoleHelper.Error(string.Join(Environment.NewLine, errors));
                        }

                    }

                    // WRITE TO SINGLE HOLDING REGISTER
                    else if (executiveCommand.Equals("WRITE HOLDREG"))
                    {
                        var registerAddress = _inputParser.Parse<ushort>(command, "ADDRESS");
                        var inputValue = _inputParser.Parse<ushort>(command, "VALUE");

                        if (registerAddress.IsValid && inputValue.IsValid)
                        {
                            await _modbusMasterClient.WriteToSingleHoldingRegister(registerAddress.Value, inputValue.Value);
                        }
                        else
                        {
                            var errors = registerAddress.Errors.Concat(inputValue.Errors);
                            ConsoleHelper.Error(string.Join(Environment.NewLine, errors));
                        }

                    }

                    // WRITE TO MULTIPLE HOLDING REGISTERS
                    else if (executiveCommand.Equals("WRITE HOLDREGS"))
                    {
                        var registerAddress = _inputParser.Parse<ushort>(command, "STARTADDRESS");
                        var inputValues = _inputParser.ParseMultiple<string>(command, "VALUES").ToArray();

                        //if ushorts
                        if (registerAddress.IsValid && inputValues.All(v => ushort.TryParse(v.Value, out _)))
                        {
                            var holdingRegistersToWrite = inputValues.Select(v => ushort.Parse(v.Value)).ToArray();
                            await _modbusMasterClient.WriteToHoldingRegisters(registerAddress.Value, holdingRegistersToWrite);
                        }
                        //if binary strings
                        else if (registerAddress.IsValid && inputValues.All(v => IsInputBinary(v.Value, 16)))
                        {
                            var holdingRegistersToWrite = inputValues.Select(v => v.Value).ToArray();
                            await _modbusMasterClient.WriteBitsToHoldingRegisters(registerAddress.Value, holdingRegistersToWrite);
                        }
                        //if floats
                        else if (registerAddress.IsValid && inputValues.All(v => float.TryParse(v.Value, out _)))
                        {
                            var holdingRegistersToWrite = inputValues.Select(v => float.Parse(v.Value)).ToArray();
                            await _modbusMasterClient.WriteFloatsToHoldingRegisters(registerAddress.Value, holdingRegistersToWrite);
                        }
                        else
                        {
                            var inputErrors = inputValues.SelectMany(e => e.Errors).ToList();
                            var errors = registerAddress.Errors.Concat(inputErrors);
                            ConsoleHelper.Error(string.Join(Environment.NewLine, errors));
                        }

                    }

                    //CONNECT TO SLAVE
                    else if (executiveCommand.Equals("CONNECT"))
                    {
                        var ipAddressInput = _inputParser.Parse<string>(command, "IPADDRESS");
                        var tcpPortInput = _inputParser.Parse<int>(command, "PORT");
                        var slaveIdInput = _inputParser.Parse<byte>(command, "SLAVEID");

                        if (tcpPortInput.IsValid && slaveIdInput.IsValid)
                        {
                            if (IPAddress.TryParse(ipAddressInput.Value, out IPAddress ip))
                                await _modbusMasterClient.AttemptToConnect(ip, tcpPortInput.Value, slaveIdInput.Value);
                            else
                                ConsoleHelper.Error($"Unable to parse {ipAddressInput.Value} to type {typeof(IPAddress)}");
                        }
                        else
                        {
                            var errors = tcpPortInput.Errors.Concat(slaveIdInput.Errors);
                            ConsoleHelper.Error(string.Join(Environment.NewLine, errors));
                        }
                            
                    }

                    //SET SLAVE ID
                    else if (executiveCommand.Equals("SET SLAVEID"))
                    {
                        var inputValue = _inputParser.Parse<byte>(command, "SLAVEID");
                        if (inputValue.IsValid)
                        {
                            ConsoleHelper.Info($"Setting new slave id. Previous value: {_modbusMasterClient.SlaveId} => New value {inputValue.Value}");
                            _modbusMasterClient.SlaveId = inputValue.Value;
                        }
                        else
                            ConsoleHelper.Error(string.Join(Environment.NewLine, inputValue.Errors));

                    }

                    //SET SLAVE IP ADDRESS
                    else if (executiveCommand.Equals("SET IPADDRESS"))
                    {
                        var ip = _inputParser.Parse<string>(command, "IPADDRESS");
                        if (IPAddress.TryParse(ip.Value, out IPAddress address))
                        {
                            ConsoleHelper.Info($"Setting slave IP address. Previous value: {_modbusMasterClient.IPAddress} => New value {address}");
                            _modbusMasterClient.Disconnect();
                            await _modbusMasterClient.AttemptToConnect(address, _modbusMasterClient.TcpPort, _modbusMasterClient.SlaveId);

                        }
                        else
                            ConsoleHelper.Error($"Unable to parse {ip.Value} to {typeof(IPAddress)}");

                    }

                    //SET SLAVE TCP PORT
                    else if (executiveCommand.Equals("SET PORT"))
                    {
                        //Set slave id
                        var port = _inputParser.Parse<int>(command, "PORT");
                        if (port.IsValid)
                        {
                            ConsoleHelper.Info($"Setting slave IP address. Previous value: {_modbusMasterClient.TcpPort} => New value {port.Value}");
                            _modbusMasterClient.Disconnect();
                            await _modbusMasterClient.AttemptToConnect(_modbusMasterClient.IPAddress, port.Value, _modbusMasterClient.SlaveId);

                        }
                        else
                            ConsoleHelper.Error(string.Join(Environment.NewLine, port.Errors));

                    }
                    else if (executiveCommand == "HELP")
                    {
                        Console.WriteLine();
                        Console.WriteLine("Modbus Commands:");
                        ConsoleHelper.Info("READ COILS         --startAddress  --numberToRead             || Read range of coils.");
                        ConsoleHelper.Info("WRITE COIL         --address       --value                    || Write to a single coil. {true/false}");
                        ConsoleHelper.Info("WRITE COILS        --startAddress  --values                   || Write to multiple coils. {comma-separated true/false}");
                        ConsoleHelper.Info("READ DISCRETES     --startAddress  --numberToRead             || Read range of discrete inputs.");
                        ConsoleHelper.Info("READ INPUTREGS     --startAddress  --numberToRead             || Read range of input registers.");
                        ConsoleHelper.Info("READ INPUTREGSB    --startAddress  --numberToRead             || Read range of input registers as bits.");
                        ConsoleHelper.Info("READ INPUTREGSF    --startAddress  --numberToRead             || Read range of input registers as 32-bit floats.");
                        ConsoleHelper.Info("READ HOLDREGS      --startAddress  --numberToRead             || Read range of holding registers.");
                        ConsoleHelper.Info("READ HOLDREGSB     --startAddress  --numberToRead             || Read range of holding registers as bits.");
                        ConsoleHelper.Info("READ HOLDREGSF     --startAddress  --numberToRead             || Read range of holding registers as 32-bit floats.");
                        ConsoleHelper.Info("WRITE HOLDREG      --address       --value                    || Write to a single holding register. {uint16 OR 32-bit float OR 16 bit binary string (LSB->MSB)}");
                        ConsoleHelper.Info("WRITE HOLDREGS     --startAddress  --values                   || Write to multiple holding registers. {comma-separated uint16s OR 32-bit floats OR 16 bit binary strings (LSB->MSB)}");
                        Console.WriteLine();
                        Console.WriteLine("Client Commands:");
                        ConsoleHelper.Info("CONNECT            --ipAddress     --port         --slaveid   || Connect to slave with ip, port and slaveid");
                        ConsoleHelper.Info("SET SLAVEID        --slaveId                                  || Set target slave ID. {1-255}");
                        ConsoleHelper.Info("SET IPADDRESS      --ipAddress                                || Set target slave IP address. Will force a reconnect.");
                        ConsoleHelper.Info("SET PORT           --port                                     || Set target slave TCP port. Will force a reconnect."); ;

                    }
                    else if (command == "EXIT")
                    {
                        exitCode = -1;
                        _modbusMasterClient.Disconnect();
                    }
                    else
                    {
                        ConsoleHelper.Error("Unknown command. Type 'HELP' for a list of valid commands.");
                    }
                }
                catch (IOException ioEx)
                {
                    if (!_modbusMasterClient.IsConnected)
                    {
                        ConsoleHelper.Error("Not connected to slave. Attempting to reconnect...");
                        await _modbusMasterClient.AttemptToConnect(_modbusMasterClient.IPAddress, _modbusMasterClient.TcpPort, _modbusMasterClient.SlaveId);
                    }
                    else
                    {
                        ConsoleHelper.Error($"Unable to read/write to slave. Make sure target slaveID exists. Current slaveID: {_modbusMasterClient.SlaveId}");
                    }
                       
                }
                catch (Exception ex)
                {
                    ConsoleHelper.Error("Unable to parse command parameters. Try again." + ex.Message);
                }

                Console.WriteLine();
            }


        }
        private bool IsInputBinary(string input, int bitLength)
        {
            if (input.Length != bitLength)
                return false;

            foreach (var c in input)
                if (c != '0' && c != '1')
                    return false;

            return true;
        }

    }
}

