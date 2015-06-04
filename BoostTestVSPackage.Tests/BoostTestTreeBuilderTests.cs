using BoostTestVSPackage.Boost;
using BoostTestVSPackage.Model;
using NUnit.Framework;

namespace BoostTestVSPackage.Tests
{
    [TestFixture]
    public class BoostTestTreeBuilderTests
    {
        [Test]
        public void Should_handle_fixture_test_suite()
        {
            // Arrange
            var fileContent = @"BOOST_FIXTURE_TEST_SUITE(TestSuite, ScopedLogLevel< LogLevels::Off >);
                                BOOST_AUTO_TEST_CASE(TestCase)
                                {
                                }
                                BOOST_AUTO_TEST_SUITE_END()";

            // Act
            var testTree = new TestTreeBuilder(new BoostLineParser()).ParseText(fileContent);

            // Assert
            Assert.AreEqual(1, testTree.Root.Suites.Count);

            var testSuite = testTree.Root.Suites[0];
            Assert.AreEqual("TestSuite", testSuite.Name);
            Assert.AreEqual(0, testSuite.Suites.Count);
            Assert.AreEqual(1, testSuite.TestCases.Count);

            var testCase = testSuite.TestCases[0];
            Assert.AreEqual("TestCase", testCase.Name);
            Assert.AreEqual(2, testCase.LineNumber);
        }

        [Test]
        public void Should_handle_inner_test_suite()
        {
            // Arrange
            var fileContent = @"BOOST_AUTO_TEST_SUITE(OuterTestSuite)
                                    BOOST_AUTO_TEST_SUITE(InnerTestSuite)
                                        BOOST_AUTO_TEST_CASE(InnerTestSuite_TestCase)
                                        {
                                        }
                                    BOOST_AUTO_TEST_SUITE_END()

                                BOOST_AUTO_TEST_CASE(OuterTestSuite_TestCase)
                                {
                                }

                                BOOST_AUTO_TEST_SUITE_END()";

            // Act
            var testTree = new TestTreeBuilder(new BoostLineParser()).ParseText(fileContent);

            // Assert
            Assert.AreEqual(1, testTree.Root.Suites.Count);

            var outerTestSuite = testTree.Root.Suites[0];
            Assert.AreEqual("OuterTestSuite", outerTestSuite.Name);
            Assert.AreEqual(1, outerTestSuite.Suites.Count);
            Assert.AreEqual(1, outerTestSuite.TestCases.Count);

            var outerTestCase = outerTestSuite.TestCases[0];
            Assert.AreEqual("OuterTestSuite_TestCase", outerTestCase.Name);
            Assert.AreEqual(8, outerTestCase.LineNumber);

            var innerTestSuite = outerTestSuite.Suites[0];
            Assert.AreEqual("InnerTestSuite", innerTestSuite.Name);
            Assert.AreEqual(0, innerTestSuite.Suites.Count);
            Assert.AreEqual(1, innerTestSuite.TestCases.Count);

            var innerTestCase = innerTestSuite.TestCases[0];
            Assert.AreEqual("InnerTestSuite_TestCase", innerTestCase.Name);
            Assert.AreEqual(3, innerTestCase.LineNumber);
        }

        [Test]
        public void Should_handle_single_test_suite()
        {
            // Arrange
            var fileContent = @"BOOST_AUTO_TEST_SUITE(TestSuite)
                                BOOST_AUTO_TEST_CASE(TestCase1)
                                {
                                }
                                BOOST_AUTO_TEST_CASE(TestCase2)
                                {
                                }
                                BOOST_AUTO_TEST_CASE_TEMPLATE(TestCase3)
                                {
                                }
                                BOOST_AUTO_TEST_SUITE_END()";
            // Act
            var testTree = new TestTreeBuilder(new BoostLineParser()).ParseText(fileContent);

            // Assert
            Assert.AreEqual(1, testTree.Root.Suites.Count);

            var testSuite = testTree.Root.Suites[0];
            Assert.AreEqual("TestSuite", testSuite.Name);
            Assert.AreEqual(3, testSuite.TestCases.Count);
            Assert.AreEqual(0, testSuite.Suites.Count);

            Assert.AreEqual("TestCase1", testSuite.TestCases[0].Name);
            Assert.False(testSuite.TestCases[0].IsTemplate);
            Assert.AreEqual(2, testSuite.TestCases[0].LineNumber);
            
            Assert.AreEqual("TestCase2", testSuite.TestCases[1].Name);
            Assert.False(testSuite.TestCases[1].IsTemplate);
            Assert.AreEqual(5, testSuite.TestCases[1].LineNumber);

            Assert.AreEqual("TestCase3", testSuite.TestCases[2].Name);
            Assert.True(testSuite.TestCases[2].IsTemplate);
            Assert.AreEqual(8, testSuite.TestCases[2].LineNumber);

            Assert.AreSame(testSuite.TestCases[0].ParentSuite, testSuite.TestCases[1].ParentSuite);
            Assert.AreSame(testSuite.TestCases[0].ParentSuite, testSuite.TestCases[2].ParentSuite);
        }
        
        [Test]
        public void Should_ignore_comments()
        {
            // Arrange
            var fileContent = @"BOOST_AUTO_TEST_SUITE(TestSuite)
                                //BOOST_AUTO_TEST_SUITE(CommentedTestSuite)
                                //BOOST_AUTO_TEST_CASE(CommentedTestCase)
                                BOOST_AUTO_TEST_CASE(UnitTest)
                                {
                                }
                                BOOST_AUTO_TEST_SUITE_END()";
            // Act
            var testTree = new TestTreeBuilder(new BoostLineParser()).ParseText(fileContent);

            // Assert
            Assert.AreEqual(1, testTree.Root.Suites.Count);

            var testSuite = testTree.Root.Suites[0];
            Assert.AreEqual("TestSuite", testSuite.Name);
            Assert.AreEqual(1, testSuite.TestCases.Count);
            Assert.AreEqual(0, testSuite.Suites.Count);

            Assert.AreEqual("UnitTest", testSuite.TestCases[0].Name);
            Assert.AreEqual(4, testSuite.TestCases[0].LineNumber);
        }
    }
}