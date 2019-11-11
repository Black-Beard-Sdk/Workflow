using Microsoft.VisualStudio.LanguageServer.Protocol;
using Newtonsoft.Json.Linq;
using StreamJsonRpc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;
using Bb.VisualStudio.Parser.Workflows.Grammar;
using System.Text;

namespace LanguageServer
{

    public class LanguageServer : INotifyPropertyChanged, IDisposable
    {

        public LanguageServer(Stream sender, Stream reader)
        {

            this.WorkflowConfigParsers = new WorkflowConfigParsers();
            this.target = new LanguageServerTarget(this);
            this.rpc = JsonRpc.Attach(sender, reader, this.target);
            this.rpc.Disconnected += OnRpcDisconnected;

            this.target.Initialized += OnInitialized;
        }

        public string CustomText { get; set; }

        public string CurrentSettings { get; private set; }

        public event EventHandler Disconnected;
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnInitialized(object sender, EventArgs e)
        {
            Timer timer = new Timer(LogMessage, null, 0, 5 * 1000);
        }

        public void OnTextDocumentOpened(DidOpenTextDocumentParams messageParams)
        {
            var sb = new System.Text.StringBuilder(messageParams.TextDocument.Text);
            ParseDocument(sb, messageParams.TextDocument.Uri.ToString());
        }

        public void OnTextDocumentChanged(DidChangeTextDocumentParams parameter)
        {
            var sb = new System.Text.StringBuilder(parameter.ContentChanges[0].Text);
            ParseDocument(sb, parameter.TextDocument.Uri.ToString());
        }

        private void ParseDocument(StringBuilder content, string path)
        {

            var result = this.WorkflowConfigParsers.ParseString(content, path);
            var errors = this.WorkflowConfigParsers.Errors;

            List<Diagnostic> diagnostics = new List<Diagnostic>();
            foreach (var item in errors)
                diagnostics.Add(new Diagnostic()
                {
                    Message = item.Message,
                    Source = path,
                    Severity = DiagnosticSeverity.Error,
                    Range = new Range()
                    {
                        Start = new Position(item.Line - 1, item.Column),
                        End = new Position(item.Line - 1, item.Column + item.Text.Length),
                    },
                    Code = item.Code
                });

            SendDiagnostics(diagnostics, path);

        }

        public void SendDiagnostics(List<Diagnostic> diagnostics, string path)
        {

            PublishDiagnosticParams parameter = new PublishDiagnosticParams
            {
                Uri = new Uri(path),
                Diagnostics = this.maxProblems > -1
                    ? diagnostics.Take(this.maxProblems).ToArray()
                    : diagnostics.ToArray()
            };

            this.rpc.NotifyWithParameterObjectAsync(Methods.TextDocumentPublishDiagnosticsName, parameter);

        }

        public void LogMessage(object arg)
        {
            this.LogMessage(arg, MessageType.Info);
        }

        public void LogMessage(object arg, MessageType messageType)
        {
            this.LogMessage(arg, "testing " + counter++, messageType);
        }

        public void LogMessage(object arg, string message, MessageType messageType)
        {
            LogMessageParams parameter = new LogMessageParams
            {
                Message = message,
                MessageType = messageType
            };
            this.rpc.NotifyWithParameterObjectAsync(Methods.WindowLogMessageName, parameter);
        }

        public void ShowMessage(string message, MessageType messageType)
        {
            ShowMessageParams parameter = new ShowMessageParams
            {
                Message = message,
                MessageType = messageType
            };
            this.rpc.NotifyWithParameterObjectAsync(Methods.WindowShowMessageName, parameter);
        }

        public async Task<MessageActionItem> ShowMessageRequestAsync(string message, MessageType messageType, string[] actionItems)
        {
            ShowMessageRequestParams parameter = new ShowMessageRequestParams
            {
                Message = message,
                MessageType = messageType,
                Actions = actionItems.Select(a => new MessageActionItem { Title = a }).ToArray()
            };

            var response = await this.rpc.InvokeWithParameterObjectAsync<JToken>(Methods.WindowShowMessageRequestName, parameter);
            return response.ToObject<MessageActionItem>();
        }

        public void SendSettings(DidChangeConfigurationParams parameter)
        {
            this.CurrentSettings = parameter.Settings.ToString();
            this.NotifyPropertyChanged(nameof(CurrentSettings));

            JToken parsedSettings = JToken.Parse(this.CurrentSettings);
            int newMaxProblems = parsedSettings.Children().First().Values<int>("maxNumberOfProblems").First();
            if (this.maxProblems != newMaxProblems)
                this.maxProblems = newMaxProblems;
        }

        public void WaitForExit()
        {
            this.disconnectEvent.WaitOne();
        }

        public void Exit()
        {
            this.disconnectEvent.Set();
            Disconnected?.Invoke(this, new EventArgs());
        }

        private void OnRpcDisconnected(object sender, JsonRpcDisconnectedEventArgs e)
        {
            Exit();
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Dispose()
        {

            this.rpc.Disconnected -= OnRpcDisconnected;
            this.rpc.Dispose();
            
            this.target.Initialized -= OnInitialized;
            this.target.Dispose();
        
        }

        private int maxProblems = -1;
        private readonly JsonRpc rpc;

        public WorkflowConfigParsers WorkflowConfigParsers { get; }

        private readonly LanguageServerTarget target;
        private readonly ManualResetEvent disconnectEvent = new ManualResetEvent(false);
        //private TextDocumentItem textDocument = null;
        private int counter = 100;

    }

}
