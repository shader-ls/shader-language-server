using Microsoft.Extensions.Logging;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using ShaderlabVS.Data;
using ShaderLS.Management;

namespace ShaderLS.Handlers
{
    public class SignatureHelpHandler : SignatureHelpHandlerBase
    {
        private readonly ILogger<SignatureHelpHandler> _logger;
        private readonly ILanguageServerConfiguration _configuration;
        private readonly Workspace _workspace;
        private readonly DocumentSelector _documentSelector;

        public SignatureHelpHandler(
            ILogger<SignatureHelpHandler> logger,
            ILanguageServerConfiguration configuration,
            DocumentSelector documentSelector,
            Workspace workspace)
        {
            this._logger = logger;
            this._configuration = configuration;
            this._documentSelector = documentSelector;
            this._workspace = workspace;
        }

        public override async Task<SignatureHelp?> Handle(SignatureHelpParams request, CancellationToken cancellationToken)
        {
            var uri = request.TextDocument.Uri;
            Position position = request.Position;

            string current = _workspace.BufferService.GetWordAtPosition(uri, position);

            int newChar = Math.Max(position.Character - current.Length, 0);
            Position newPos = new Position(position.Line, newChar);

            // Attempt to get function name.
            string word = _workspace.BufferService.GetWordAtPosition(uri, newPos);

            _logger.LogWarning("word: " + word);

            var signatures = new Container<SignatureInformation>();

            ShaderlabDataManager.Instance.HLSLCGFunctions.ForEach(f =>
            {
                foreach (var item in f.Synopsis)
                {
                    if (f.Name.Equals(word))
                    {
                        var sign = CreateSignature(item, f.Description);
                        signatures.Append(sign);
                    }
                }
            });

            ShaderlabDataManager.Instance.UnityBuiltinFunctions.ForEach(f =>
            {
                foreach (var item in f.Synopsis)
                {
                    if (f.Name.Equals(word))
                    {
                        var sign = CreateSignature(item, f.Description);
                        signatures.Append(sign);
                    }
                }
            });

            return new SignatureHelp
            {
                Signatures = signatures
            };
        }

        protected override SignatureHelpRegistrationOptions CreateRegistrationOptions(SignatureHelpCapability capability, ClientCapabilities clientCapabilities)
        {
            return new SignatureHelpRegistrationOptions()
            {
                DocumentSelector = _documentSelector,
                TriggerCharacters = new[] { "(", ",", "<", "{", "[", "," }
            };
        }

        private SignatureInformation CreateSignature(string sign, string documentation)
        {
            var signature = new SignatureInformation
            {
                Documentation = documentation,
                Label = sign,
                Parameters = new()
            };

            //find the parameters in the method signature (expect methodname(one, two)
            string[] pars = sign.Split(new char[] { '(', ',', ')', ';' }, StringSplitOptions.RemoveEmptyEntries);

            int locusSearchStart = 0;
            for (int i = 1; i < pars.Length; ++i)
            {
                string param = pars[i].Trim();

                if (string.IsNullOrEmpty(param))
                    continue;

                int locusStart = sign.IndexOf(param, locusSearchStart);

                if (locusStart >= 0)
                {
                    _logger.LogWarning("param: " + param);

                    var newPararm = new ParameterInformation()
                    {
                        Documentation = "some doc",
                        Label = param
                    };

                    signature.Parameters.Append(newPararm);
                }
            }

            return signature;
        }
    }
}
