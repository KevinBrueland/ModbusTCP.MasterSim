using System.Collections.Generic;

namespace Modbus.Master.Simulator.Parsers
{
    public class ParseResult<T>
    {
        public ParseResult()
        {
            Errors = new List<string>();
        }
        public bool IsValid => Errors.Count == 0;
        public T Value { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}
