using System;
using System.Collections.Generic;
using System.Text;

namespace Modbus.Master.Simulator.Types
{
    public class ModbusMasterOptions
    {
        public int MaxRetryCount { get; set; }
        public int RetryInterval { get; set; }
        public int SendTimeout { get; set; }
        public int ReceiveTimeout { get; set; }
    }
}
