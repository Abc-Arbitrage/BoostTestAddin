using BoostTestVSPackage.Interfaces;

namespace BoostTestVSPackage.Boost
{
    public class BoostCommandLineBuilder : ICommandLineBuilder
    {
        public string GetCommandLineArguments(string testItemPath, TestRunnerOptions testRunnerOptions)
        {
            var commandLineArgs = string.Empty;
                
            if (testItemPath != null)
                commandLineArgs += "--run_test=" + testItemPath;

            if (!testRunnerOptions.DetectMemoryLeak)
                commandLineArgs += " --detect_memory_leaks=0";

            if (testRunnerOptions.ShowProgress)
                commandLineArgs += " --show_progress=yes";

            return commandLineArgs;
        }
    }
}