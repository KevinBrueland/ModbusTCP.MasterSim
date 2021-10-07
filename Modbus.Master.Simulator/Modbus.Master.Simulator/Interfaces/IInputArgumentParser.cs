using Modbus.Master.Simulator.Parsers;
using System.Collections.Generic;

namespace Modbus.Master.Simulator.Interfaces
{
    public interface IInputArgumentParser
    {
        ParseResult<T> Parse<T>(string inputCommand, string argumentName, string argumentIndicator = "--");
        IEnumerable<ParseResult<T>> ParseMultiple<T>(string inputCommand, string argumentName, string argumentIndicator = "--", char multipleValueSeparater = ',');
    }
}
