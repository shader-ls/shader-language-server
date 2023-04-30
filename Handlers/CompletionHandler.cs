using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using ShaderlabVS.Data;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

#nullable enable

namespace ShaderLS.Handlers
{
    public class CompletionHandler : CompletionHandlerBase
    {
        private readonly ILogger<CompletionHandler> _logger;
        private readonly ILanguageServerConfiguration _configuration;
        private readonly DocumentSelector _documentSelector;

        public CompletionHandler(
            ILogger<CompletionHandler> logger,
            ILanguageServerConfiguration configuration,
            DocumentSelector documentSelector)
        {
            this._logger = logger;
            this._configuration = configuration;
            this._documentSelector = documentSelector;
        }

        public override Task<CompletionItem> Handle(CompletionItem request, CancellationToken cancellationToken)
        {
            return Task.FromResult(request);
        }

        public override async Task<CompletionList> Handle(CompletionParams request, CancellationToken cancellationToken)
        {
            return GenerateCompletions(request, cancellationToken);
        }

        protected override CompletionRegistrationOptions CreateRegistrationOptions(CompletionCapability capability, ClientCapabilities clientCapabilities)
            => new CompletionRegistrationOptions()
            {
                DocumentSelector = _documentSelector,
                ResolveProvider = true,
                TriggerCharacters = new[] { ":", "." },
                AllCommitCharacters = new Container<string>(new[] { "\n" })
            };

        public CompletionList GenerateCompletions(CompletionParams request, CancellationToken cancellationToken)
        {
            var completions = new List<CompletionItem>();
            var keywords = new HashSet<string>();

            var dm = ShaderlabDataManager.Instance;

            dm.HLSLCGFunctions.ForEach(f =>
            {
                completions.Add(new CompletionItem
                {
                    Kind = CompletionItemKind.Function,
                    Label = f.Name,
                    InsertText = f.Name,
                    Documentation = new MarkupContent { Kind = MarkupKind.Markdown, Value = f.Description },
                });
                keywords.Add(f.Name);
            });

            dm.HLSLCGDatatypes.ForEach(d =>
            {
                completions.Add(new CompletionItem
                {
                    Kind = CompletionItemKind.Keyword,
                    Label = d,
                    InsertText = d,
                    Documentation = new MarkupContent { Kind = MarkupKind.Markdown, Value = "" },
                });
                keywords.Add(d);
            });

            //completions.Add(new CompletionItem
            //{
            //    Kind = CompletionItemKind.Text,
            //    Label = "float4",
            //    InsertText = "float4",
            //    Documentation = new MarkupContent { Kind = MarkupKind.Markdown, Value = "This is documentation" },
            //});
            //completions.Add(new CompletionItem
            //{
            //    Kind = CompletionItemKind.Text,
            //    Label = "some",
            //    InsertText = "some",
            //    Documentation = new MarkupContent { Kind = MarkupKind.Markdown, Value = "This is documentation" },
            //});

            return new CompletionList(completions, completions.Count > 1);
        }
    }
}
