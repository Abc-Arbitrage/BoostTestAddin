namespace BoostTestVSPackage
{
    public class TestRunnerOptions
    {
        public bool DryRun;
        public bool WithDebugger;
        public bool DetectMemoryLeak;

        public override string ToString()
        {
            return string.Format("DryRun: {0}, WithDebugger: {1}, DetectMemoryLeak: {2}", DryRun, WithDebugger, DetectMemoryLeak);
        }
    }
}