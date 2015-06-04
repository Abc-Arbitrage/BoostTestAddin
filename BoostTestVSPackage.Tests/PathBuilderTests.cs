using BoostTestVSPackage.Boost;
using BoostTestVSPackage.Model;
using NUnit.Framework;

namespace BoostTestVSPackage.Tests
{
    [TestFixture]
    public class PathBuilderTests
    {
        [Test]
        public void Should_build_inner_test_suite_path()
        {
            // Arrange
            var tree = new TestTree();
            tree.Root.Suites.Add(new TestSuite("TestSuite", 1, tree.Root));
            tree.Root.Suites[0].Suites.Add(new TestSuite("InnerTestSuite", 1, tree.Root.Suites[0]));

            // Act
            var path = PathBuilder.GetPath(tree.Root.Suites[0].Suites[0]);

            // Assert
            Assert.AreEqual("TestSuite/InnerTestSuite", path);
        }

        [Test]
        public void Should_build_test_case_path_when_in_root_suite()
        {
            // Arrange
            var tree = new TestTree();
            tree.Root.TestCases.Add(new TestCase("UnitTest", 1, tree.Root, isTemplate: false));

            // Act
            var path = PathBuilder.GetPath(tree.Root.TestCases[0]);

            // Assert
            Assert.AreEqual("UnitTest", path);
        }

        [Test]
        public void Should_build_template_test_case_path_when_in_root_suite()
        {
            // Arrange
            var tree = new TestTree();
            tree.Root.TestCases.Add(new TestCase("UnitTest", 1, tree.Root, isTemplate: true));

            // Act
            var path = PathBuilder.GetPath(tree.Root.TestCases[0]);

            // Assert
            Assert.AreEqual("UnitTest*", path);
        }
        
        [Test]
        public void Should_build_test_case_path_when_in_suite()
        {
            // Arrange
            var tree = new TestTree();
            tree.Root.Suites.Add(new TestSuite("TestSuite", 1, tree.Root));
            tree.Root.Suites[0].TestCases.Add(new TestCase("UnitTest", 1, tree.Root.Suites[0], isTemplate: false));

            // Act
            var path = PathBuilder.GetPath(tree.Root.Suites[0].TestCases[0]);

            // Assert
            Assert.AreEqual("TestSuite/UnitTest", path);
        }


        [Test]
        public void Should_build_template_test_case_path_when_in_suite()
        {
            // Arrange
            var tree = new TestTree();
            tree.Root.Suites.Add(new TestSuite("TestSuite", 1, tree.Root));
            tree.Root.Suites[0].TestCases.Add(new TestCase("UnitTest", 1, tree.Root.Suites[0], isTemplate: true));

            // Act
            var path = PathBuilder.GetPath(tree.Root.Suites[0].TestCases[0]);

            // Assert
            Assert.AreEqual("TestSuite/UnitTest*", path);
        }
        
        [Test]
        public void Should_build_test_suite_path()
        {
            // Arrange
            var tree = new TestTree();
            tree.Root.Suites.Add(new TestSuite("TestSuite", 1, tree.Root));
            tree.Root.Suites[0].TestCases.Add(new TestCase("UnitTest", 1, tree.Root.Suites[0], isTemplate: false));

            // Act
            var path = PathBuilder.GetPath(tree.Root.Suites[0]);

            // Assert
            Assert.AreEqual("TestSuite", path);
        }
    }
}