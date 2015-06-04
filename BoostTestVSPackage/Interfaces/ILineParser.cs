using System;

namespace BoostTestVSPackage.Interfaces
{
    
    public enum TokenType
    {
        None,
        TestCase,
        TestCaseTemplate,
        TestSuiteBegin,
        TestSuiteEnd,
    }

    public interface ILineParser
    {
        Tuple<TokenType, string> ParseLine(string line);
    }
}