using MediatR;
using Microsoft.Extensions.Logging;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using OmniSharp.Extensions.LanguageServer.Protocol.Workspace;
using ShaderLS.Management;

namespace ShaderLS.Handlers
{
    public class CodeActionHandler : CodeActionHandlerBase, IExecuteCommandHandler
    {
        private readonly ILogger<CodeActionHandler> _logger;
        private readonly ILanguageServerConfiguration _configuration;
        private readonly Workspace _workspace;
        private readonly DocumentSelector _documentSelector;

        public CodeActionHandler(
            ILogger<CodeActionHandler> logger,
            ILanguageServerConfiguration configuration,
            DocumentSelector documentSelector,
            Workspace workspace)
        {
            this._logger = logger;
            this._configuration = configuration;
            this._documentSelector = documentSelector;
            this._workspace = workspace;
        }

        public ExecuteCommandRegistrationOptions GetRegistrationOptions(ExecuteCommandCapability capability, ClientCapabilities clientCapabilities)
            => new ExecuteCommandRegistrationOptions
            {
                Commands = null
            };

        public Task<Unit> Handle(ExecuteCommandParams request, CancellationToken cancellationToken)
        {
            return Unit.Task;
        }

        public override Task<CodeAction> Handle(CodeAction request, CancellationToken cancellationToken)
        {
            return Task.FromResult(request);
        }

        public override Task<CommandOrCodeActionContainer> Handle(CodeActionParams request, CancellationToken cancellationToken)
        {
            return null;
        }

        protected override CodeActionRegistrationOptions CreateRegistrationOptions(CodeActionCapability capability, ClientCapabilities clientCapabilities)
        {
            return null;
        }
    }
}
