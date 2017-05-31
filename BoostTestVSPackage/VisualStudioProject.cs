using System;
using BoostTestVSPackage.Interfaces;
using BoostTestVSPackage.Utils;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.VCProjectEngine;

namespace BoostTestVSPackage
{
    public class VisualStudioProject : IVisualStudioProject
    {
        private readonly DTE2 _applicationObject;
        private readonly bool _dryRun;
        private readonly Project _project;

        public VisualStudioProject(DTE2 applicationObject, Project project, bool dryRun)
        {
            if (project == null)
                throw new ArgumentNullException("project");

            _applicationObject = applicationObject;
            _project = project;
            _dryRun = dryRun;
        }

        public void SetCurrentConfigurationCommandLineArgs(string arguments)
        {
            Logger.Write("SetCurrentConfigurationCommandLineArgs: " + arguments);

            if (_dryRun)
                return;

            var vcproj = _project.Object as VCProject;
            if (vcproj == null)
            {
                Logger.Write("Cannot cast _project.Object to VCProject.");
                return;
            }

            var vcconfig = vcproj.ActiveConfiguration;
            var vcdebug = vcconfig.DebugSettings as VCDebugSettings;

            if (vcdebug == null)
            {
                Logger.Write("Cannot retrieve VCDebugSettings.");
                return;
            }

            vcdebug.CommandArguments = arguments;
        }

        public void SetProjectAsStartup()
        {
            Logger.Write("SetProjectAsStartup: " + _project.Name);

            if (_dryRun)
                return;

            _applicationObject.DTE.Solution.SolutionBuild.StartupProjects = _project.UniqueName;
        }

        public void BuildProject()
        {
            Logger.Write("BuildProject: " + _project.Name);

            if (_dryRun)
                return;

            var config = _project.ConfigurationManager.ActiveConfiguration;
            _applicationObject.DTE.Solution.SolutionBuild.BuildProject(config.ConfigurationName, _project.UniqueName, WaitForBuildToFinish: true);
        }

        public void StartProject()
        {
            Logger.Write("StartProject: " + _project.Name);

            if (_dryRun)
                return;

            _applicationObject.DTE.ExecuteCommand("Debug.StartWithoutDebugging");
        }

        public void DebugProject()
        {
            Logger.Write("DebugProject: " + _project.Name);

            if (_dryRun)
                return;

            _applicationObject.DTE.ExecuteCommand("Debug.Start");
        }
    }
}