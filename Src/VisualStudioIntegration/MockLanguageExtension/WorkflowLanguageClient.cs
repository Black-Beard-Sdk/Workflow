﻿using Microsoft.VisualStudio;
using Microsoft.VisualStudio.LanguageServer.Client;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;
using Microsoft.VisualStudio.Utilities;
using StreamJsonRpc;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Reflection;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace WorkflowLanguageExtension
{
    [ContentType("Workflow")]
    [Export(typeof(ILanguageClient))]
    public class WorkflowLanguageClient : ILanguageClient, ILanguageClientCustomMessage
    {
        internal const string UiContextGuidString = "ADB4A81B-C197-4979-A677-0AE92F1F4A46";

        private Guid uiContextGuid = new Guid(UiContextGuidString);

        public event AsyncEventHandler<EventArgs> StartAsync;
        public event AsyncEventHandler<EventArgs> StopAsync;

        public WorkflowLanguageClient()
        {
            Instance = this;
        }

        internal static WorkflowLanguageClient Instance { get; set; }

        internal JsonRpc Rpc { get; set; }

        public string Name => "Workflow Language Extension";

        public IEnumerable<string> ConfigurationSections
        {
            get
            {
                yield return "Workflow";
            }
        }

        public object InitializationOptions => null;

        public IEnumerable<string> FilesToWatch => null;

        public object MiddleLayer => null;

        public object CustomMessageTarget => null;

        public async Task<Connection> ActivateAsync(CancellationToken token)
        {
            ProcessStartInfo info = new ProcessStartInfo();
            var programPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Server", @"LanguageServerWithUI.exe");
            info.FileName = programPath;
            info.WorkingDirectory = Path.GetDirectoryName(programPath);

            var stdInPipeName = @"output";
            var stdOutPipeName = @"input";


            var sid = new SecurityIdentifier(WellKnownSidType.AuthenticatedUserSid, null);
            var pipeAccessRule = new PipeAccessRule(sid, PipeAccessRights.ReadWrite, System.Security.AccessControl.AccessControlType.Allow);
            var pipeSecurity = new PipeSecurity();

            // "Some or all identity references could not be translated."
            pipeSecurity.AddAccessRule(pipeAccessRule);



            var bufferSize = 256;
            var readerPipe = new NamedPipeServerStream(stdInPipeName, PipeDirection.InOut, 4, PipeTransmissionMode.Message, PipeOptions.Asynchronous, bufferSize, bufferSize, pipeSecurity);
            var writerPipe = new NamedPipeServerStream(stdOutPipeName, PipeDirection.InOut, 4, PipeTransmissionMode.Message, PipeOptions.Asynchronous, bufferSize, bufferSize, pipeSecurity);

            Process process = new Process();
            process.StartInfo = info;

            if (process.Start())
            {
                await readerPipe.WaitForConnectionAsync(token);
                await writerPipe.WaitForConnectionAsync(token);

                return new Connection(readerPipe, writerPipe);
            }

            return null;
        }

        public async System.Threading.Tasks.Task AttachForCustomMessageAsync(JsonRpc rpc)
        {
            this.Rpc = rpc;

            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            // Sets the UI context so the custom command will be available.
            var monitorSelection = ServiceProvider.GlobalProvider.GetService(typeof(IVsMonitorSelection)) as IVsMonitorSelection;
            if (monitorSelection != null)
            {

                if (monitorSelection.GetCmdUIContextCookie(ref this.uiContextGuid, out uint cookie) == VSConstants.S_OK)
                    monitorSelection.SetCmdUIContext(cookie, 1);
            
            }
        }

        public async System.Threading.Tasks.Task OnLoadedAsync()
        {
            await StartAsync?.InvokeAsync(this, EventArgs.Empty);
        }

        public System.Threading.Tasks.Task OnServerInitializedAsync()
        {
            return System.Threading.Tasks.Task.CompletedTask;
        }

        public System.Threading.Tasks.Task OnServerInitializeFailedAsync(Exception e)
        {
            return System.Threading.Tasks.Task.CompletedTask;
        }
    }
}
