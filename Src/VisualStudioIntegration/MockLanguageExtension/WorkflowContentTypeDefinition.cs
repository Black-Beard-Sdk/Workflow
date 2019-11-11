using Microsoft.VisualStudio.LanguageServer.Client;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;

namespace WorkflowLanguageExtension
{
    public class WorkflowContentDefinition
    {
        [Export]
        [Name("workflow")]
        [BaseDefinition(CodeRemoteContentDefinition.CodeRemoteContentTypeName)]
        internal static ContentTypeDefinition WorkflowContentTypeDefinition;


        [Export]
        [FileExtension(".workflow")]
        [ContentType("workflow")]
        internal static FileExtensionToContentTypeDefinition WorkflowFileExtensionDefinition;
    }
}
