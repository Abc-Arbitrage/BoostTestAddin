using System.IO;
using BoostTestVSPackage.Interfaces;
using EnvDTE;
using EnvDTE80;

namespace BoostTestVSPackage
{
    public class VisualStudio : IVisualStudio
    {
        private readonly DTE2 _applicationObject;
        private readonly bool _dryRun;

        public VisualStudio(DTE2 applicationObject, bool dryRun = false)
        {
            _applicationObject = applicationObject;
            _dryRun = dryRun;
        }


        public string ActiveDocumentContent
        {
            get
            {
                return File.ReadAllText(ActiveDocumentFullName);
            }
        }

        public string ActiveDocumentFullName
        {
            get { return _applicationObject.DTE.ActiveDocument.FullName; }
        }



        public int CursorLineNumber
        {
            get
            {
                var selection = (TextSelection)_applicationObject.DTE.ActiveDocument.Selection;
                return selection.TopPoint.Line;
            }
        }


        public IVisualStudioProject ActiveDocumentProject
        {
            get
            {
                return new VisualStudioProject(_applicationObject, _applicationObject.DTE.ActiveDocument.ProjectItem.ContainingProject, _dryRun);
            }
        }
    }
}