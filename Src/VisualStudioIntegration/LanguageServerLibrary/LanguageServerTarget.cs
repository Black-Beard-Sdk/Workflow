using Microsoft.VisualStudio.LanguageServer.Protocol;
using Newtonsoft.Json.Linq;
using StreamJsonRpc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LanguageServer
{
    public class LanguageServerTarget : IDisposable
    {

        public LanguageServerTarget(LanguageServer server)
        {

            //if (!System.Diagnostics.Debugger.IsAttached)
            //    System.Diagnostics.Debugger.Launch();

            //System.Diagnostics.Debugger.Break();

            this.server = server;
        }

        public event EventHandler Initialized;

        [JsonRpcMethod(Methods.InitializeName)]
        public object Initialize(JToken arg)
        {

            var capabilities = new ServerCapabilities
            {
                TextDocumentSync = new TextDocumentSyncOptions(),
                CompletionProvider = new CompletionOptions
                {
                    ResolveProvider = false,
                    TriggerCharacters = new string[] { ",", "." }
                },
            };
            capabilities.TextDocumentSync.OpenClose = true;
            capabilities.TextDocumentSync.Change = TextDocumentSyncKind.Full;

            var result = new InitializeResult
            {
                Capabilities = capabilities
            };

            Initialized?.Invoke(this, new EventArgs());

            return result;
        }

        [JsonRpcMethod(Methods.TextDocumentDidOpenName)]
        public void OnTextDocumentOpened(JToken arg)
        {
            var parameter = arg.ToObject<DidOpenTextDocumentParams>();
            server.OnTextDocumentOpened(parameter);
        }

        [JsonRpcMethod(Methods.TextDocumentDidChangeName)]
        public void OnTextDocumentChanged(JToken arg)
        {

            //if (!System.Diagnostics.Debugger.IsAttached)
            //    System.Diagnostics.Debugger.Launch();

            //System.Diagnostics.Debugger.Break();

            var parameter = arg.ToObject<DidChangeTextDocumentParams>();
            server.OnTextDocumentChanged(parameter);
        }

        [JsonRpcMethod(Methods.TextDocumentCompletionName)]
        public CompletionItem[] OnTextDocumentCompletion(JToken arg)
        {

            //if (!System.Diagnostics.Debugger.IsAttached)
            //    System.Diagnostics.Debugger.Launch();

            //System.Diagnostics.Debugger.Break();

            List<CompletionItem> items = new List<CompletionItem>();

            for (int i = 0; i < 10; i++)
                items.Add(new CompletionItem
                {
                    Label = "Item " + i,
                    InsertText = "Item" + i,
                    Kind = (CompletionItemKind)(i % (Enum.GetNames(typeof(CompletionItemKind)).Length) + 1)
                });

            return items.ToArray();

        }

        [JsonRpcMethod(Methods.WorkspaceDidChangeConfigurationName)]
        public void OnDidChangeConfiguration(JToken arg)
        {

            if (!System.Diagnostics.Debugger.IsAttached)
                System.Diagnostics.Debugger.Launch();

            System.Diagnostics.Debugger.Break();

            var parameter = arg.ToObject<DidChangeConfigurationParams>();
            this.server.SendSettings(parameter);
        }

        [JsonRpcMethod(Methods.ShutdownName)]
        public object Shutdown()
        {
            return null;
        }

        [JsonRpcMethod(Methods.ExitName)]
        public void Exit()
        {
            server.Exit();
        }

        public void Dispose()
        {
        }

        //public string GetText()
        //{

        //    if (!System.Diagnostics.Debugger.IsAttached)
        //        System.Diagnostics.Debugger.Launch();

        //    System.Diagnostics.Debugger.Break();

        //    return string.IsNullOrWhiteSpace(this.server.CustomText)
        //        ? "custom text from language server target"
        //        : this.server.CustomText;
        //}

        private readonly LanguageServer server;

    }
}
