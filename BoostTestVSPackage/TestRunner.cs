using System;
using System.Linq;
using BoostTestVSPackage.Boost;
using BoostTestVSPackage.Interfaces;
using BoostTestVSPackage.Model;
using BoostTestVSPackage.Utils;

namespace BoostTestVSPackage
{
    public class TestRunner : ITestRunner
    {
        private readonly ICommandLineBuilder _commandLineBuilder;
        private readonly ITestTreeBuilder _testTreeBuilder;
        private readonly IVisualStudio _visualStudio;

        public TestRunner(IVisualStudio visualStudio, ITestTreeBuilder testTreeBuilder, ICommandLineBuilder commandLineBuilder)
        {
            if (visualStudio == null)
                throw new ArgumentNullException("visualStudio");
            if (testTreeBuilder == null)
                throw new ArgumentNullException("testTreeBuilder");
            if (commandLineBuilder == null)
                throw new ArgumentNullException("commandLineBuilder");

            _visualStudio = visualStudio;
            _testTreeBuilder = testTreeBuilder;
            _commandLineBuilder = commandLineBuilder;
        }

        public void RunCurrentProject(TestRunnerOptions testRunnerOptions)
        {
            var commandLineArgs = _commandLineBuilder.GetCommandLineArguments(testItemPath: null, testRunnerOptions: testRunnerOptions);
            ConfigureAndStartProject(commandLineArgs, testRunnerOptions);
        }

        public void RunCurrentTestCase(TestRunnerOptions testRunnerOptions)
        {
            var testTree = ParseActiveDocument();
            Logger.Write(testTree.Root.PrettyPrint());

            var testCase = FindTestCaseFromCurrentLine(testTree);
            if (testCase == null)
            {
                Logger.Write("Failed to find a test case to run");
                return;
            }

            var itemPath = PathBuilder.GetPath(testCase);
            if (string.IsNullOrEmpty(itemPath))
            {
                Logger.Write("Failed to find a test case to run");
                return;
            }
            Logger.Write("Found test case to run: " + itemPath);

            var commandLineArgs = _commandLineBuilder.GetCommandLineArguments(itemPath, testRunnerOptions);
            ConfigureAndStartProject(commandLineArgs, testRunnerOptions);
        }

        public void RunCurrentTestSuite(TestRunnerOptions testRunnerOptions)
        {
            var testTree = ParseActiveDocument();
            Logger.Write(testTree.Root.PrettyPrint());

            var testSuite = FindTestSuiteFromCurrentLine(testTree);
            if (testSuite == null)
            {
                Logger.Write("Failed to find a test suite to run");
                return;
            }

            var itemPath = PathBuilder.GetPath(testSuite);
            if (string.IsNullOrEmpty(itemPath))
            {
                Logger.Write("Failed to find a test suite to run");
                return;
            }
            Logger.Write("Found test suite to run: " + itemPath);

            var commandLineArgs = _commandLineBuilder.GetCommandLineArguments(itemPath, testRunnerOptions);
            ConfigureAndStartProject(commandLineArgs, testRunnerOptions);
        }


        private void ConfigureAndStartProject(string commandLineArgs, TestRunnerOptions testRunnerOptions)
        {
            var project = _visualStudio.ActiveDocumentProject;

            project.SetProjectAsStartup();
            project.SetCurrentConfigurationCommandLineArgs(commandLineArgs);

            if (testRunnerOptions.WithDebugger)
                project.DebugProject();
            else
                project.StartProject();
        }
        
        private TestSuite FindTestSuiteFromCurrentLine(TestTree testTree)
        {
            var lineNumber = _visualStudio.CursorLineNumber;
            return testTree.Root.GetAllTestSuites().OrderBy(x => x.LineNumber).LastOrDefault(x => x.LineNumber <= lineNumber);
        }

        private TestCase FindTestCaseFromCurrentLine(TestTree testTree)
        {
            var lineNumber = _visualStudio.CursorLineNumber;
            return testTree.Root.GetAllTestCases().OrderBy(x => x.LineNumber).LastOrDefault(x => x.LineNumber <= lineNumber);
        }

        private TestTree ParseActiveDocument()
        {
            Logger.Write("ParseActiveDocument: " + _visualStudio.ActiveDocumentFullName);
            return _testTreeBuilder.ParseText(_visualStudio.ActiveDocumentContent);
        }
    }
}
