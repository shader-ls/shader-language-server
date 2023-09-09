using System.Text.RegularExpressions;
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

        public override async Task<SignatureHelp?> Handle(SignatureHelpParams request, CancellationToken token)
        {
            var uri = request.TextDocument.Uri;
            Position position = request.Position;

            // Attempt to get function name.
            List<string>? line = _workspace.BufferService.GetLineSplit(uri, position);

            if (line == null)
                return null;

            string[] func = line[0].Split("(", StringSplitOptions.RemoveEmptyEntries);

            if (func.Length == 0)
                return null;

            int max = Math.Max(func.Length - 2, 0);
            string[] funcNames = Helpers.GetWords(func[max]);

            if (funcNames.Length == 0)
                return null;

            string word = funcNames[funcNames.Length - 1];

            var signatures = new List<SignatureInformation>();

            ShaderlabDataManager.Instance.HLSLCGFunctions.ForEach(f =>
            {
                foreach (var item in f.Synopsis)
                {
                    if (f.Name.Equals(word))
                    {
                        var sign = CreateSignature(item, f.Description);

                        signatures.Add(sign);
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

                        signatures.Add(sign);
                    }
                }
            });

            string paramStr = func[func.Length - 1];
            int act = Regex.Matches(paramStr, ",").Count;

            return new SignatureHelp
            {
                ActiveParameter = act,
                Signatures = signatures
            };
        }

        protected override SignatureHelpRegistrationOptions CreateRegistrationOptions(SignatureHelpCapability capability, ClientCapabilities clientCapabilities)
        {
            return new SignatureHelpRegistrationOptions()
            {
                DocumentSelector = _documentSelector,
                TriggerCharacters = new[] { "(", ",", "<", "{", "[", }
            };
        }

        private SignatureInformation CreateSignature(string sign, string documentation)
        {
            var paramLst = new Container<ParameterInformation>();

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
                    var newPararm = new ParameterInformation()
                    {
                        Documentation = "",
                        Label = param
                    };

                    paramLst.Append(newPararm);
                }
            }

            return new SignatureInformation
            {
                Documentation = documentation,
                Label = sign,
                Parameters = paramLst
            };
        }
    }
}
