using System;
using BoostTestVSPackage.Interfaces;
using BoostTestVSPackage.Model;
using Moq;
using NUnit.Framework;

namespace BoostTestVSPackage.Tests
{
    [TestFixture]
    public class TestRunnerTests
    {
        [SetUp]
        public void Setup()
        {
            _visualStudioProject = new Mock<IVisualStudioProject>();
            _visualStudio = new Mock<IVisualStudio>();
            _commandLineBuiler = new Mock<ICommandLineBuilder>();
            _testTreeBuilder = new Mock<ITestTreeBuilder>();
        }

        private Mock<IVisualStudioProject> _visualStudioProject;
        private Mock<IVisualStudio> _visualStudio;
        private Mock<ICommandLineBuilder> _commandLineBuiler;
        private Mock<ITestTreeBuilder> _testTreeBuilder;
        private const string _documentContent = @"anything since it won't be actually parsed";

        [Test]
        public void Should_configure_project_build__stuffs_before_run()
        {
            // Arrange
            _visualStudio.SetupGet(x => x.ActiveDocumentContent).Returns(_documentContent);
            _visualStudio.SetupGet(x => x.CursorLineNumber).Returns(2);
            _visualStudio.SetupGet(x => x.ActiveDocumentProject).Returns(() => _visualStudioProject.Object);

            var testRunner = new TestRunner(_visualStudio.Object, _testTreeBuilder.Object, _commandLineBuiler.Object);

            // Act
            testRunner.RunCurrentProject(new TestRunnerOptions());

            // Assert
            _visualStudioProject.Verify(x => x.SetCurrentConfigurationCommandLineArgs(It.IsAny<string>()), Times.Once());
            _visualStudioProject.Verify(x => x.SetProjectAsStartup(), Times.Once());
            _visualStudioProject.Verify(x => x.BuildProject(), Times.Never());
        }
        
        
        [Test]
        public void Should_start_debugger_when_enabled()
        {
            // Arrange
            _visualStudio.SetupGet(x => x.ActiveDocumentContent).Returns(_documentContent);
            _visualStudio.SetupGet(x => x.CursorLineNumber).Returns(2);
            _visualStudio.SetupGet(x => x.ActiveDocumentProject).Returns(() => _visualStudioProject.Object);

            var testRunner = new TestRunner(_visualStudio.Object, _testTreeBuilder.Object, _commandLineBuiler.Object);

            var testRunnerOptions = new TestRunnerOptions
            {
                WithDebugger = true,
            };

            // Act
            testRunner.RunCurrentProject(testRunnerOptions);

            // Assert
            _visualStudioProject.Verify(x => x.DebugProject(), Times.Once());
            _visualStudioProject.Verify(x => x.StartProject(), Times.Never());
        }
        
        [Test]
        public void Should_not_start_debugger_when_disabled()
        {
            // Arrange
            _visualStudio.SetupGet(x => x.ActiveDocumentContent).Returns(_documentContent);
            _visualStudio.SetupGet(x => x.CursorLineNumber).Returns(2);
            _visualStudio.SetupGet(x => x.ActiveDocumentProject).Returns(() => _visualStudioProject.Object);

            var testRunner = new TestRunner(_visualStudio.Object, _testTreeBuilder.Object, _commandLineBuiler.Object);

            var testRunnerOptions = new TestRunnerOptions
            {
                WithDebugger = false,
            };

            // Act
            testRunner.RunCurrentProject(testRunnerOptions);

            // Assert
            _visualStudioProject.Verify(x => x.DebugProject(), Times.Never());
            _visualStudioProject.Verify(x => x.StartProject(), Times.Once());
        }

        [Test]
        public void Should_run_current_test_suite()
        {
            // Arrange
            _visualStudio.SetupGet(x => x.ActiveDocumentContent).Returns(_documentContent);
            _visualStudio.SetupGet(x => x.CursorLineNumber).Returns(2);
            _visualStudio.SetupGet(x => x.ActiveDocumentProject).Returns(() => _visualStudioProject.Object);

            _testTreeBuilder.Setup(x => x.ParseText(_documentContent)).Returns(() =>
            {
                var testTree = new TestTree();
                var testSuite = new TestSuite("TheTestSuiteToRun", 1, testTree.Root);
                testSuite.TestCases.Add(new TestCase("TestCase", 2, testSuite, isTemplate: false));
                testTree.Root.Suites.Add(testSuite);
                return testTree;
            });

            string basedCommandLineArg = null;
            _commandLineBuiler.Setup(x => x.GetCommandLineArguments(It.IsAny<string>(), It.IsAny<TestRunnerOptions>()))
                .Returns("arg")
                .Callback((Action<string, TestRunnerOptions>)((s, r) => basedCommandLineArg = s));

            var testRunner = new TestRunner(_visualStudio.Object, _testTreeBuilder.Object, _commandLineBuiler.Object);

            var testRunnerOptions = new TestRunnerOptions
            {
                WithDebugger = false,
            };

            // Act
            testRunner.RunCurrentTestSuite(testRunnerOptions);

            // Assert
            _testTreeBuilder.Verify(x => x.ParseText(_documentContent), Times.Once());
            _commandLineBuiler.Verify(x => x.GetCommandLineArguments(It.IsAny<string>(), It.IsAny<TestRunnerOptions>()), Times.Once());

            Assert.IsNotNull(basedCommandLineArg);
            Assert.IsTrue(basedCommandLineArg.Contains("TheTestSuiteToRun"));
            Assert.IsFalse(basedCommandLineArg.Contains("TestCase"));
        }

        [Test]
        public void Should_run_current_unit_test()
        {
            // Arrange
            _visualStudio.SetupGet(x => x.ActiveDocumentContent).Returns(_documentContent);
            _visualStudio.SetupGet(x => x.CursorLineNumber).Returns(2);
            _visualStudio.SetupGet(x => x.ActiveDocumentProject).Returns(() => _visualStudioProject.Object);

            _testTreeBuilder.Setup(x => x.ParseText(_documentContent)).Returns(() =>
            {
                var testTree = new TestTree();
                var testSuite = new TestSuite("TestSuite", 1, testTree.Root);
                testSuite.TestCases.Add(new TestCase("TestCaseToRun", 2, testSuite, isTemplate: false));
                testTree.Root.Suites.Add(testSuite);
                return testTree;
            });

            string basedCommandLineArg = null;
            _commandLineBuiler.Setup(x => x.GetCommandLineArguments(It.IsAny<string>(), It.IsAny<TestRunnerOptions>()))
                .Returns("arg")
                .Callback((Action<string, TestRunnerOptions>)((s, r) => basedCommandLineArg = s));
            
            var testRunner = new TestRunner(_visualStudio.Object, _testTreeBuilder.Object, _commandLineBuiler.Object);

            var testRunnerOptions = new TestRunnerOptions
            {
                WithDebugger = false,
            };

            // Act
            testRunner.RunCurrentTestCase(testRunnerOptions);

            // Assert
            _testTreeBuilder.Verify(x => x.ParseText(_documentContent), Times.Once());
            _commandLineBuiler.Verify(x => x.GetCommandLineArguments(It.IsAny<string>(), It.IsAny<TestRunnerOptions>()), Times.Once());
            
            Assert.IsNotNull(basedCommandLineArg);
            Assert.IsTrue(basedCommandLineArg.Contains("TestSuite"));
            Assert.IsTrue(basedCommandLineArg.Contains("TestCaseToRun"));
        }
    }
}