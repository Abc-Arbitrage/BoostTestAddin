namespace BoostTestVSPackage.Interfaces
{
    public interface IVisualStudio
    {
        string ActiveDocumentContent { get; }
        string ActiveDocumentFullName { get; }
        int CursorLineNumber { get; }
        IVisualStudioProject ActiveDocumentProject { get; }
    }
}