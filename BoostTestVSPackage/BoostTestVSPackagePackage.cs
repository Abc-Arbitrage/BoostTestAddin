using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using BoostTestVSPackage.Boost;
using BoostTestVSPackage.Model;
using BoostTestVSPackage.Utils;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;

namespace BoostTestVSPackage
{
    /// <summary>
    ///     This is the class that implements the package exposed by this assembly.
    ///     The minimum requirement for a class to be considered a valid package for Visual Studio
    ///     is to implement the IVsPackage interface and register itself with the shell.
    ///     This package uses the helper classes defined inside the Managed Package Framework (MPF)
    ///     to do it: it derives from the Package class that provides the implementation of the
    ///     IVsPackage interface and uses the registration attributes defined in the framework to
    ///     register itself and its components with the shell.
    /// </summary>
    // This attribute tells the PkgDef creation utility (CreatePkgDef.exe) that this class is
    // a package.
    [PackageRegistration(UseManagedResourcesOnly = true)]
    // This attribute is used to register the information needed to show this package
    // in the Help/About dialog of Visual Studio.
    [InstalledProductRegistration("#110", "#112", "1.2", IconResourceID = 400)]
    // This attribute is needed to let the shell know that this package exposes some menus.
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(GuidList.guidBoostTestVSPackagePkgString)]
    public sealed class BoostTestVSPackagePackage : Package
    {
        private DTE2 _applicationObject;
        private bool _detectMemoryLeak;

        /// <summary>
        ///     Default constructor of the package.
        ///     Inside this method you can place any initialization code that does not require
        ///     any Visual Studio service because at this point the package object is created but
        ///     not sited yet inside Visual Studio environment. The place to do all the other
        ///     initialization is the Initialize method.
        /// </summary>
        public BoostTestVSPackagePackage()
        {
            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", this.ToString()));
        }

        /////////////////////////////////////////////////////////////////////////////
        // Overridden Package Implementation

        #region Package Members

        /// <summary>
        ///     Initialization of the package; this method is called right after the package is sited, so this is the place
        ///     where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", this.ToString()));
            base.Initialize();

            // Add our command handlers for menu (commands must exist in the .vsct file)
            var mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (null == mcs)
                return;

            _applicationObject = (DTE2)GetService(typeof(DTE));

            // Create the command for the menu item.

            {
                var menuCommandId = new CommandID(GuidList.guidBoostTestVSPackageCmdSet, (int)PkgCmdIdList.cmdidDebugCurrentTest);
                var menuCommand = new MenuCommand((sender, args) => RunTestCase(true), menuCommandId);
                mcs.AddCommand(menuCommand);
            }

            {
                var menuCommandId = new CommandID(GuidList.guidBoostTestVSPackageCmdSet, (int)PkgCmdIdList.cmdidRunCurrentTest);
                var menuCommand = new MenuCommand((sender, args) => RunTestCase(false), menuCommandId);
                mcs.AddCommand(menuCommand);
            }

            {
                var menuCommandId = new CommandID(GuidList.guidBoostTestVSPackageCmdSet, (int)PkgCmdIdList.cmdidRunCurrentSuite);
                var menuCommand = new MenuCommand((sender, args) => RunTestSuite(false), menuCommandId);
                mcs.AddCommand(menuCommand);
            }

            {
                var menuCommandId = new CommandID(GuidList.guidBoostTestVSPackageCmdSet, (int)PkgCmdIdList.cmdidRunCurrentProject);
                var menuCommand = new MenuCommand((sender, args) => RunProject(false), menuCommandId);
                mcs.AddCommand(menuCommand);
            }

            {
                var menuCommandId = new CommandID(GuidList.guidBoostTestVSPackageCmdSet, (int)PkgCmdIdList.cmdidDetectMemoryLeak);
                var menuCommand = new MenuCommand((sender, args) =>
                {
                    var menu = sender as MenuCommand;
                    if (menu == null)
                        return;

                    menu.Checked = !menu.Checked;
                    _detectMemoryLeak = !_detectMemoryLeak;
                }, menuCommandId);
                mcs.AddCommand(menuCommand);
            }
        }

        #endregion

        private TestRunnerOptions MakeTestRunnerOptions(bool withDebugger)
        {
            var testRunnerOptions = new TestRunnerOptions
            {
                DetectMemoryLeak = _detectMemoryLeak,
                DryRun = true,
                WithDebugger = withDebugger,
            };

            return testRunnerOptions;
        }

        private void RunTestCase(bool withDebugger)
        {
            Logger.Write("===================================================");

            var runner = new TestRunner(new VisualStudio(_applicationObject), new TestTreeBuilder(new BoostLineParser()), new BoostCommandLineBuilder());
            runner.RunCurrentTestCase(MakeTestRunnerOptions(withDebugger));
        }

        private void RunTestSuite(bool withDebugger)
        {
            Logger.Write("===================================================");

            var runner = new TestRunner(new VisualStudio(_applicationObject), new TestTreeBuilder(new BoostLineParser()), new BoostCommandLineBuilder());
            runner.RunCurrentTestSuite(MakeTestRunnerOptions(withDebugger));
        }

        private void RunProject(bool withDebugger)
        {
            Logger.Write("===================================================");

            var runner = new TestRunner(new VisualStudio(_applicationObject), new TestTreeBuilder(new BoostLineParser()), new BoostCommandLineBuilder());
            runner.RunCurrentProject(MakeTestRunnerOptions(withDebugger));
        }
    }
}
