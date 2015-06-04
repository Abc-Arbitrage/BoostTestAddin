using BoostTestVSPackage.Boost;
using BoostTestVSPackage.Interfaces;
using NUnit.Framework;

namespace BoostTestVSPackage.Tests
{
    [TestFixture]
    public class BoostLineParserTests
    {
        [TestCase("", TokenType.None, null)]

        [TestCase("BOOST_AUTO_TEST_CASE(UnitTest)", TokenType.TestCase, "UnitTest")]
        [TestCase("BOOST_AUTO_TEST_CASE_TEMPLATE(UnitTest, T, Types)", TokenType.TestCase, "UnitTest")]
        [TestCase("BOOST_FIXTURE_TEST_CASE(UnitTest, Fixture)", TokenType.TestCase, "UnitTest")]
        [TestCase("  BOOST_AUTO_TEST_CASE(UnitTest)  ", TokenType.TestCase, "UnitTest")]
        [TestCase("//BOOST_AUTO_TEST_CASE(UnitTest)", TokenType.None, null)]

        [TestCase("BOOST_AUTO_TEST_SUITE(TestSuite)", TokenType.TestSuiteBegin, "TestSuite")]
        [TestCase("BOOST_FIXTURE_TEST_SUITE(TestSuite)", TokenType.TestSuiteBegin, "TestSuite")]
        [TestCase("  BOOST_AUTO_TEST_SUITE(TestSuite)  ", TokenType.TestSuiteBegin, "TestSuite")]
        [TestCase("//BOOST_AUTO_TEST_SUITE(TestSuite)", TokenType.None, null)]

        [TestCase("BOOST_AUTO_TEST_SUITE_END", TokenType.TestSuiteEnd, null)]
        [TestCase("  BOOST_AUTO_TEST_SUITE_END  ", TokenType.TestSuiteEnd, null)]
        [TestCase("//BOOST_AUTO_TEST_SUITE_END()", TokenType.None, null)]

        public void TestCases(string text, TokenType expectedToken, string expectedName)
        {
            var parser = new BoostLineParser();
            var result = parser.ParseLine(text);

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedToken, result.Item1);
            Assert.AreEqual(expectedName, result.Item2);
        }
    }
}