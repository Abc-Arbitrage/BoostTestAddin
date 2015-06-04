namespace BoostTestVSPackage
{
    public interface ITestRunner
    {
        void RunCurrentProject(TestRunnerOptions testRunnerOptions);
        void RunCurrentTestCase(TestRunnerOptions testRunnerOptions);
        void RunCurrentTestSuite(TestRunnerOptions testRunnerOptions);
    }
}