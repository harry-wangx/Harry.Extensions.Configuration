/*
 * ²Î¿¼£º
 * https://github.com/MartinJohns/ConfigurationContrib/blob/master/src/Microsoft.Extensions.Configuration.Yaml/YamlConfigurationFileParser.cs
 */
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YamlDotNet.Core;
using YamlDotNet.RepresentationModel;

namespace Harry.Extensions.Configuration.Yaml
{
    internal class YamlConfigurationFileParser
    {
        private YamlConfigurationFileParser() { }

        private readonly IDictionary<string, string> _data = new SortedDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private readonly Stack<string> _context = new Stack<string>();
        private string _currentPath;

        public static IDictionary<string, string> Parse(Stream input)
        {
            try
            {
                return new YamlConfigurationFileParser().ParseStream(input);
            }
            catch (YamlException ex)
            {
                string errorLine = string.Empty;
                if (input.CanSeek)
                {
                    input.Seek(0, SeekOrigin.Begin);

                    using (var streamReader = new StreamReader(input))
                    {
                        var fileContent = ReadLines(streamReader);
                        errorLine = RetrieveErrorContext(ex, fileContent);
                    }
                }

                throw new FormatException(
                    "Could not parse the YAML file. " +
                    $"Error on line number '{ex.Start.Line}': '{errorLine}'.", ex);
            }

        }

        private IDictionary<string, string> ParseStream(Stream input)
        {
            _data.Clear();

            var yaml = new YamlStream();
            yaml.Load(new StreamReader(input));

            if (!yaml.Documents.Any())
            {
                return _data;
            }

            if (yaml.Documents[0].RootNode is YamlMappingNode mapping)
            {
                foreach (var entry in mapping.Children)
                {
                    var context = ((YamlScalarNode)entry.Key).Value;
                    VisitYamlNode(context, entry.Value);
                }
            }

            return _data;
        }

        private void VisitYamlNode(string context, YamlNode node)
        {
            switch (node)
            {
                case YamlScalarNode scalarNode:
                    VisitYamlScalarNode(context, scalarNode);
                    break;
                case YamlMappingNode mappingNode:
                    VisitYamlMappingNode(context, mappingNode);
                    break;
                case YamlSequenceNode sequenceNode:
                    VisitYamlSequenceNode(context, sequenceNode);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(node),
                        $"Unsupported YAML node type '{node.GetType().Name} was found. " +
                        $"Path '{_currentPath}', line {node.Start.Line} position {node.Start.Column}.");
            }
        }

        private void VisitYamlScalarNode(string context, YamlScalarNode scalarNode)
        {
            EnterContext(context);
            var currentKey = _currentPath;

            if (_data.ContainsKey(currentKey))
            {
                throw new FormatException($"A duplicate key '{currentKey}' was found.");
            }

            _data[currentKey] = scalarNode.Value;
            ExitContext();
        }

        private void VisitYamlMappingNode(string context, YamlMappingNode mappingNode)
        {
            EnterContext(context);

            foreach (var nodePair in mappingNode.Children)
            {
                var innerContext = ((YamlScalarNode)nodePair.Key).Value;
                VisitYamlNode(innerContext, nodePair.Value);
            }

            ExitContext();
        }

        private void VisitYamlSequenceNode(string context, YamlSequenceNode sequenceNode)
        {
            EnterContext(context);

            for (var i = 0; i < sequenceNode.Children.Count; ++i)
            {
                VisitYamlNode(i.ToString(), sequenceNode.Children[i]);
            }

            ExitContext();
        }

        private void EnterContext(string context)
        {
            _context.Push(context);
            _currentPath = ConfigurationPath.Combine(_context.Reverse());
        }

        private void ExitContext()
        {
            _context.Pop();
            _currentPath = ConfigurationPath.Combine(_context.Reverse());
        }

        #region ´íÎó´¦Àí
        private static string RetrieveErrorContext(YamlException ex, IEnumerable<string> fileContent)
        {
            var possibleLineContent = fileContent.Skip(ex.Start.Line - 1).FirstOrDefault();
            return possibleLineContent ?? string.Empty;
        }

        private static IEnumerable<string> ReadLines(StreamReader streamReader)
        {
            string line;
            do
            {
                line = streamReader.ReadLine();
                yield return line;
            } while (line != null);
        }
        #endregion
    }
}
