using System;
using System.Text.RegularExpressions;
using BoostTestVSPackage.Interfaces;

namespace BoostTestVSPackage.Boost
{
    public class BoostLineParser : ILineParser
    {
        public Tuple<TokenType, string> ParseLine(string line)
        {
            var name = MatchTestCase(line);
            if (name != null)
                return new Tuple<TokenType, string>(TokenType.TestCase, name);

            name = MatchTestCaseTemplate(line);
            if (name != null)
                return new Tuple<TokenType, string>(TokenType.TestCaseTemplate, name); 
            
            name = MatchTestSuiteBegin(line);
            if (name != null)
                return new Tuple<TokenType, string>(TokenType.TestSuiteBegin, name);

            if (MatchTestSuiteEnd(line))
                return new Tuple<TokenType, string>(TokenType.TestSuiteEnd, null);

            return new Tuple<TokenType, string>(TokenType.None, null);
        }

        private static string MatchTestCase(string line)
        {
            var match = Regex.Match(line, @"^\s*BOOST_(AUTO|FIXTURE)_TEST_CASE[\s]*\((.*)\)");
            if (match.Success)
            {
                return match.Groups[2].Value.Trim().Split(',')[0].Trim();
            }

            return null;
        }

        private static string MatchTestCaseTemplate(string line)
        {
            var match = Regex.Match(line, @"^\s*BOOST_(AUTO|FIXTURE)_TEST_CASE_TEMPLATE[\s]*\((.*)\)");
            if (match.Success)
            {
                return match.Groups[2].Value.Trim().Split(',')[0].Trim();
            }

            return null;
        }

        private static string MatchTestSuiteBegin(string line)
        {
            var match = Regex.Match(line, @"^\s*BOOST_(AUTO|FIXTURE)_TEST_SUITE[\s]*\((.*)\)");
            if (match.Success)
            {
                return match.Groups[2].Value.Trim().Split(',')[0].Trim();
            }

            return null;
        }

        private static bool MatchTestSuiteEnd(string line)
        {
            var match = Regex.Match(line, @"^\s*BOOST_AUTO_TEST_SUITE_END.*");
            return match.Success;
        }
    }
}