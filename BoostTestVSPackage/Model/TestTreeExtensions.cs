using System.Collections.Generic;
using System.Text;

namespace BoostTestVSPackage.Model
{
    public static class TestTreeExtensions
    {
        public static string PrettyPrint(this TestTree @this)
        {
            return @this.Root.PrettyPrint();
        }

        public static string PrettyPrint(this TestSuite @this, int indentLevel = 0)
        {
            var sb = new StringBuilder();

            if (!@this.IsRoot)
            {
                sb.Append(' ', indentLevel * 4);
                sb.Append("Suite ");
                sb.Append(@this.Name);
                sb.AppendFormat(" (line: {0})", @this.LineNumber);
                sb.AppendLine();

                indentLevel++;
            }

            foreach (var childSuite in @this.Suites)
            {
                sb.Append(PrettyPrint(childSuite, indentLevel));
            }

            foreach (var testCase in @this.TestCases)
            {
                sb.Append(' ', indentLevel*4);
                sb.Append("Case ");
                sb.Append(testCase.Name);
                sb.AppendFormat(" (line: {0})", testCase.LineNumber);
                sb.AppendLine();
            }

            return sb.ToString();
        }

        /// <summary>
        /// Gets all test items of a test suite (test cases, child test suites and their own test cases) in a single enumeration of <see cref="TestItem"/> instances.
        /// </summary>
        /// <param name="this">The test suite.</param>
        /// <returns></returns>
        public static IEnumerable<TestCase> GetAllTestCases(this TestSuite @this)
        {
            foreach (var testCase in @this.TestCases)
            {
                yield return testCase;
            }

            foreach (var suite in @this.Suites)
            {
                foreach (var testCase in GetAllTestCases(suite))
                {
                    yield return testCase;
                }
            }
        }

        /// <summary>
        /// Gets all child test suites of a test suite in a single enumeration of <see cref="TestItem"/> instances.
        /// </summary>
        /// <param name="this">The test suite.</param>
        /// <returns></returns>
        public static IEnumerable<TestSuite> GetAllTestSuites(this TestSuite @this)
        {
            yield return @this;

            foreach (var suite in @this.Suites)
            {
                yield return suite;
            }
        }
    }
}