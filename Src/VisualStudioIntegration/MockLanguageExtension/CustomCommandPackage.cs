using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Win32;

namespace WorkflowLanguageExtension
{
    /// <summary>
    /// This package only loads when the WorkflowLanguageClient.UiContextGuidString UI context is set.  This ensures that this extension is only loaded when the language server is activated.
    /// </summary>
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideAutoLoad(WorkflowLanguageClient.UiContextGuidString)]
    [Guid(CustomCommandPackage.PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    public sealed class CustomCommandPackage : Package
    {
        /// <summary>
        /// CustomCommandPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "3365682D-8CFB-45A0-9800-1683B3B72A5D";

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomCommand"/> class.
        /// </summary>
        public CustomCommandPackage()
        {

            //var e = (EnvDTE.DTE)Package.GetGlobalService(typeof(EnvDTE.DTE));


        }

        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            CustomCommand.Initialize(this);
            base.Initialize();
        }

        #endregion
    
    }
}
