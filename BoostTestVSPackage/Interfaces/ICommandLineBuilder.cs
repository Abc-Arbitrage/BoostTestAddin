namespace BoostTestVSPackage.Interfaces
{
    public interface ICommandLineBuilder
    {
        string GetCommandLineArguments(string testItemPath, TestRunnerOptions testRunnerOptions);
    }
}