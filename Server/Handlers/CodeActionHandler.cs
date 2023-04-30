using MediatR;
using Microsoft.Extensions.Logging;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using OmniSharp.Extensions.LanguageServer.Protocol.Server.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Workspace;

namespace ShaderLS.Handlers
{
    public class CodeActionHandler : CodeActionHandlerBase, IExecuteCommandHandler
    {
        private readonly ILogger<CodeActionHandler> _logger;
        private readonly ILanguageServerConfiguration _configuration;

        private readonly DocumentSelector _documentSelector = new DocumentSelector(
            new DocumentFilter { Pattern = "**/*.shader" },
            new DocumentFilter { Pattern = "**/*.cginc" },
            new DocumentFilter { Pattern = "**/*.glslinc" },
            new DocumentFilter { Pattern = "**/*.compute" },
            new DocumentFilter { Pattern = "**/*.cg" },
            new DocumentFilter { Pattern = "**/*.hlsl" }
        );

        public CodeActionHandler(
            ILogger<CodeActionHandler> logger,
            ILanguageServerConfiguration configuration)
        {
            this._logger = logger;
            this._configuration = configuration;
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
