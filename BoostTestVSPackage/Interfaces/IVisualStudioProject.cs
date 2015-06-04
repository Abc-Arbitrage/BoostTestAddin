namespace BoostTestVSPackage.Interfaces
{
    public interface IVisualStudioProject
    {
        void SetProjectAsStartup();
        void BuildProject();

        void DebugProject();
        void StartProject();
        
        void SetCurrentConfigurationCommandLineArgs(string arguments);
    }
}