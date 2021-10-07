using Modbus.Master.Simulator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Modbus.Master.Simulator.Parsers
{
    public class InputArgumentParser : IInputArgumentParser
    {
        public ParseResult<T> Parse<T>(string inputCommand, string argumentName, string argumentIndicator = "--")
        {
            var parseResult = new ParseResult<T>();

            //Get all the arguments in input
            var arguments = Regex.Matches(inputCommand, $"\\{argumentIndicator}(\\w+)").Cast<Match>().Select(match => match.Value).ToList();

            //Check if the argument we want to parse exists
            var argumentToParse = arguments.Where(x => x.Contains(argumentName)).SingleOrDefault();

            if (string.IsNullOrWhiteSpace(argumentToParse))
                parseResult.Errors.Add($"Missing argument {argumentIndicator + argumentName}");

            //Try to get the argument value from the catch group following the argument
            var argValueGroup = Regex.Match(inputCommand, $"{argumentToParse}\\s?(\\S+)").Groups;
            if (argValueGroup.Count < 1)
                parseResult.Errors.Add($"Missing value for argument {argumentIndicator + argumentToParse}");

            //Try to convert the value to the required type
            var argValue = argValueGroup[1].Value;
            try
            {
                parseResult.Value = (T)Convert.ChangeType(argValue, typeof(T));
            }
            catch (Exception)
            {
                if (!string.IsNullOrWhiteSpace(argumentToParse))
                    parseResult.Errors.Add($"Unable to parse value {argValue} to type {typeof(T)} for argument {argumentToParse}");
            }

            return parseResult;
        }

        public IEnumerable<ParseResult<T>> ParseMultiple<T>(string inputCommand, string argumentName, string argumentIndicator = "--", char multipleValueSeparater = ',')
        {
            var errors = new List<string>();

            //Get all the arguments in input
            var arguments = Regex.Matches(inputCommand, $"\\{argumentIndicator}(\\w+)").Cast<Match>().Select(match => match.Value).ToList();

            //Check if the argument we want to parse exists
            var argumentToParse = arguments.Where(x => x.Contains(argumentName)).SingleOrDefault();

            if (string.IsNullOrWhiteSpace(argumentToParse))
                errors.Add($"Missing argument {argumentIndicator + argumentName}");

            //Try to get the argument value from the catch group following the argument
            var argValueGroup = Regex.Match(inputCommand, $"{argumentToParse}\\s?(\\S+)").Groups;
            if (argValueGroup.Count < 1)
                errors.Add($"Missing value for argument {argumentIndicator + argumentToParse}");

            //Try to convert the value to the required type
            var argValueString = argValueGroup[1].Value;

            var argValues = argValueString.Split(multipleValueSeparater);

            foreach (var argValue in argValues)
            {
                var parseResult = new ParseResult<T>();
                parseResult.Errors.AddRange(errors);
                try
                {
                    parseResult.Value = (T)Convert.ChangeType(argValue, typeof(T));
                }
                catch (Exception)
                {
                    if (!string.IsNullOrWhiteSpace(argumentToParse))
                        parseResult.Errors.Add($"Unable to parse value {argValue} to type {typeof(T)} for argument {argumentToParse}");
                }

                yield return parseResult;
            }
        }
    }
}
