using Microsoft.Extensions.Logging;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using ShaderlabVS;
using ShaderlabVS.Data;
using ShaderLS.Management;

#nullable enable

namespace ShaderLS.Handlers
{
    public class CompletionHandler : CompletionHandlerBase
    {
        private readonly ILogger<CompletionHandler> _logger;
        private readonly ILanguageServerConfiguration _configuration;
        private readonly DocumentSelector _documentSelector;
        private readonly Workspace _workspace;

        public CompletionHandler(
            ILogger<CompletionHandler> logger,
            ILanguageServerConfiguration configuration,
            DocumentSelector documentSelector,
            Workspace workspace)
        {
            this._logger = logger;
            this._configuration = configuration;
            this._documentSelector = documentSelector;
            this._workspace = workspace;
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
            var uri = request.TextDocument.Uri;

            _logger.LogWarning("!!! {" + request.Position.Character + "}");

            var completions = new List<CompletionItem>();
            var keywords = new HashSet<string>();
            
            var dm = ShaderlabDataManager.Instance;

            // Add functions into auto completion list
            //
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

            // Datatypes
            //
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

            // Keywords
            //
            ShaderlabDataManager.Instance.HLSLCGBlockKeywords.ForEach(k =>
            {
                completions.Add(new CompletionItem
                {
                    Kind = CompletionItemKind.Keyword,
                    Label = k,
                    InsertText = k,
                    Documentation = new MarkupContent { Kind = MarkupKind.Markdown, Value = "" },
                });
                keywords.Add(k);
            });

            ShaderlabDataManager.Instance.HLSLCGNonblockKeywords.ForEach(k =>
            {
                completions.Add(new CompletionItem
                {
                    Kind = CompletionItemKind.Keyword,
                    Label = k,
                    InsertText = k,
                    Documentation = new MarkupContent { Kind = MarkupKind.Markdown, Value = "" },
                });
                keywords.Add(k);
            });

            ShaderlabDataManager.Instance.HLSLCGSpecialKeywords.ForEach(k =>
            {
                completions.Add(new CompletionItem
                {
                    Kind = CompletionItemKind.Keyword,
                    Label = k,
                    InsertText = k,
                    Documentation = new MarkupContent { Kind = MarkupKind.Markdown, Value = "" },
                });
                keywords.Add(k);
            });

            if (!Utilities.IsInCGOrHLSLFile(request.TextDocument.Uri.Path))
            {
                // Unity data types
                //
                ShaderlabDataManager.Instance.UnityBuiltinDatatypes.ForEach(d =>
                {
                    completions.Add(new CompletionItem
                    {
                        Kind = CompletionItemKind.Keyword,
                        Label = d.Name,
                        InsertText = d.Name,
                        Documentation = new MarkupContent { Kind = MarkupKind.Markdown, Value = d.Description },
                    });
                    keywords.Add(d.Name);
                });

                // Unity Functions
                //
                ShaderlabDataManager.Instance.UnityBuiltinFunctions.ForEach(f =>
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

                ShaderlabDataManager.Instance.UnityKeywords.ForEach(k =>
                {
                    completions.Add(new CompletionItem
                    {
                        Kind = CompletionItemKind.Function,
                        Label = k.Name,
                        InsertText = k.Name,
                        Documentation = new MarkupContent { Kind = MarkupKind.Markdown, Value = k.Description },
                    });
                    keywords.Add(k.Name);
                });

                // Unity values/enums
                ShaderlabDataManager.Instance.UnityBuiltinValues.ForEach(v =>
                {
                    completions.Add(new CompletionItem
                    {
                        Kind = CompletionItemKind.Value,
                        Label = v.Name,
                        InsertText = v.Name,
                        Documentation = new MarkupContent { Kind = MarkupKind.Markdown, Value = v.VauleDescription },
                    });
                    keywords.Add(v.Name);
                });

                // Unity Macros
                // 
                ShaderlabDataManager.Instance.UnityBuiltinMacros.ForEach(m =>
                {
                    string description = string.Format("{0}\n{1}", string.Join(";\n", m.Synopsis), m.Description);
                    completions.Add(new CompletionItem
                    {
                        Kind = CompletionItemKind.Method,
                        Label = m.Name,
                        InsertText = m.Name,
                        Documentation = new MarkupContent { Kind = MarkupKind.Markdown, Value = description },
                    });
                    keywords.Add(m.Name);
                });
            }

            // Add words in current file
            //
            foreach (var word in _workspace.BufferService.Tokens(uri))
            {
                if (!keywords.Contains(word))
                {
                    completions.Add(new CompletionItem
                    {
                        Kind = CompletionItemKind.Text,
                        Label = word,
                        InsertText = word,
                        Documentation = new MarkupContent { Kind = MarkupKind.Markdown, Value = string.Empty },
                    });
                }
            }

            return new CompletionList(completions, completions.Count > 1);
        }
    }
}
