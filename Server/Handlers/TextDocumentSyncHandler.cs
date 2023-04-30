using MediatR;
using Microsoft.Extensions.Logging;
using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using OmniSharp.Extensions.LanguageServer.Protocol.Server.Capabilities;
using Workspace = ShaderLS.Management.Workspace;

#pragma warning disable CS0618

namespace ShaderLS.Handlers
{
    public class TextDocumentSyncHandler : TextDocumentSyncHandlerBase
    {
        private readonly ILogger<TextDocumentSyncHandler> _logger;
        private readonly ILanguageServerConfiguration _configuration;

        private readonly TextDocumentSyncKind _documentSyncKind;
        private readonly Workspace _workspace;

        private readonly DocumentSelector _documentSelector;

        public TextDocumentSyncHandler(
            ILogger<TextDocumentSyncHandler> logger, 
            ILanguageServerConfiguration configuration,
            DocumentSelector documentSelector,
            Workspace workspace)
        {
            this._logger = logger;
            this._configuration = configuration;
            this._documentSelector = documentSelector;
            this._workspace = workspace;
        }

        public TextDocumentSyncKind Change { get; } = TextDocumentSyncKind.Full;

        public override async Task<Unit> Handle(DidChangeTextDocumentParams notification, CancellationToken token)
        {
            var uri = notification.TextDocument.Uri;
            var config = await _configuration.GetScopedConfiguration(uri, token);

            foreach (var change in notification.ContentChanges)
            {
                if (change.Range != null)
                {
                    _workspace.BufferService.ApplyIncrementalChange(uri, change.Range, change.Text);
                    //_logger.LogWarning(_workspace.BufferService.GetText(uri));
                }
                else
                {
                    _workspace.BufferService.ApplyFullChange(uri, change.Text);
                }
            }

            return Unit.Value;
        }

        public override async Task<Unit> Handle(DidOpenTextDocumentParams notification, CancellationToken token)
        {
            DocumentUri uri = notification.TextDocument.Uri;
            string text = notification.TextDocument.Text;
            _workspace.Init(uri);
            _logger.LogWarning("didOpen: " + uri.Path);
            _workspace.BufferService.Add(uri, text);
            _logger.LogWarning(_workspace.BufferService.Tokens(uri).ToString());
            return Unit.Value;
        }

        public override async Task<Unit> Handle(DidCloseTextDocumentParams notification, CancellationToken token)
        {
            if (_configuration.TryGetScopedConfiguration(notification.TextDocument.Uri, out var disposable))
                disposable.Dispose();
            DocumentUri uri = notification.TextDocument.Uri;
            _logger.LogWarning("didClose: " + uri.Path);
            _workspace.BufferService.Remove(uri);
            return Unit.Value;
        }

        public override Task<Unit> Handle(DidSaveTextDocumentParams notification, CancellationToken token)
        {
            if (Capability?.DidSave == true)
            {

            }
            return Unit.Task;
        }

        protected override TextDocumentSyncRegistrationOptions CreateRegistrationOptions(SynchronizationCapability capability, ClientCapabilities clientCapabilities) => new TextDocumentSyncRegistrationOptions()
        {
            DocumentSelector = _documentSelector,
            Change = TextDocumentSyncKind.Incremental,
            Save = new SaveOptions { IncludeText = false }  // we don't need it for anything
        };

        public override TextDocumentAttributes GetTextDocumentAttributes(DocumentUri uri)
        {
            var langaugeId = "shaderlab";
            return new TextDocumentAttributes(uri, uri.Scheme, langaugeId);
        }
    }
}
