namespace BoostTestVSPackage
{
    public class TestRunnerOptions
    {
        public bool DryRun;
        public bool WithDebugger;

        public bool DetectMemoryLeak;
        public bool ShowProgress;
        public bool RunInRandomOrder;

        public override string ToString()
        {
            return $"DryRun: {DryRun}, WithDebugger: {WithDebugger}, DetectMemoryLeak: {DetectMemoryLeak}, ShowProgress: {ShowProgress}, RunInRandomOrder: {RunInRandomOrder}";
        }
    }
}