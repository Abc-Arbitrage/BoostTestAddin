using System.IO;
using BoostTestVSPackage.Interfaces;

namespace BoostTestVSPackage.Model
{
    public class TestTreeBuilder : ITestTreeBuilder
    {
        private readonly ILineParser _parser;
        private int _currentLineNumber;
        private TestSuite _currentSuite;
        private TestTree _tree;

        public TestTreeBuilder(ILineParser lineParser)
        {
            _parser = lineParser;
        }

        public TestTree ParseText(string text)
        {
            Reset();

            var reader = new StringReader(text);

            var line = reader.ReadLine();
            while (line != null)
            {
                ParseLine(line);
                line = reader.ReadLine();
            }

            return _tree;
        }

        private void ParseLine(string line)
        {
            _currentLineNumber++;

            var result = _parser.ParseLine(line);

            if (result.Item1 == TokenType.TestSuiteBegin)
            {
                var suite = new TestSuite(result.Item2, _currentLineNumber, _currentSuite);
                _currentSuite.Suites.Add(suite);
                _currentSuite = suite;
            }
            else if (result.Item1 == TokenType.TestSuiteEnd)
            {
                _currentSuite = _currentSuite.ParentSuite;
            }
            else if (result.Item1 == TokenType.TestCase || result.Item1 == TokenType.TestCaseTemplate)
            {
                _currentSuite.TestCases.Add(new TestCase(result.Item2, _currentLineNumber, _currentSuite, result.Item1 == TokenType.TestCaseTemplate));
            }
        }

        private void Reset()
        {
            _currentLineNumber = 0;
            _tree = new TestTree();
            _currentSuite = _tree.Root;
        }
    }
}