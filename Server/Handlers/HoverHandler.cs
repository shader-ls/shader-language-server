using Microsoft.Extensions.Logging;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using ShaderlabVS.Data;
using ShaderLS.Management;

namespace ShaderLS.Handlers
{
    public class HoverHandler : HoverHandlerBase
    {
        private readonly ILogger<HoverHandler> _logger;
        private readonly ILanguageServerConfiguration _configuration;
        private readonly Workspace _workspace;
        private readonly DocumentSelector _documentSelector;

        private Dictionary<string, string> _quickInfos = new();

        public HoverHandler(
            ILogger<HoverHandler> logger,
            ILanguageServerConfiguration configuration,
            DocumentSelector documentSelector,
            Workspace workspace)
        {
            this._logger = logger;
            this._configuration = configuration;
            this._documentSelector = documentSelector;
            this._workspace = workspace;
            QuickInfoInit();
        }

        public override async Task<Hover?> Handle(HoverParams request, CancellationToken cancellationToken)
        {
            var uri = request.TextDocument.Uri;

            Position position = request.Position;

            string keyText = _workspace.BufferService.GetWordAtPosition(uri, position);

            string info;
            _quickInfos.TryGetValue(keyText, out info);

            if (info == null)
                info = string.Empty;

            _logger.LogWarning("keyText: " + keyText);
            _logger.LogWarning("info: " + info);

            return new Hover
            {
                Contents = new MarkedStringsOrMarkupContent(new MarkupContent() { Value = info, Kind = MarkupKind.Markdown })
            };
        }

        protected override HoverRegistrationOptions CreateRegistrationOptions(HoverCapability capability, ClientCapabilities clientCapabilities)
            => new HoverRegistrationOptions()
            {
                DocumentSelector = _documentSelector
            };

        private void QuickInfoInit()
        {
            if (_quickInfos.Keys.Count != 0)
                return;

            ShaderlabDataManager.Instance.HLSLCGFunctions.ForEach((f) =>
            {
                if (_quickInfos.ContainsKey(f.Name))
                {
                    string info = _quickInfos[f.Name];
                    info = info + string.Format("\nFunction: {0}", f.Description);
                    _quickInfos[f.Name] = info;
                }
                else
                {
                    _quickInfos.Add(f.Name, f.Description);
                }
            });

            ShaderlabDataManager.Instance.UnityBuiltinDatatypes.ForEach((d) =>
            {
                if (_quickInfos.ContainsKey(d.Name))
                {
                    _quickInfos[d.Name] = _quickInfos[d.Name] + string.Format("\nUnity3d built-in balues: {0}", d.Description);
                }
                else
                {
                    _quickInfos.Add(d.Name, d.Description);
                }
            });

            ShaderlabDataManager.Instance.UnityBuiltinFunctions.ForEach((f) =>
            {
                if (_quickInfos.ContainsKey(f.Name))
                {
                    _quickInfos[f.Name] = _quickInfos[f.Name] + string.Format("\nUnity3D built-in function: {0}", f.Description);
                }
                else
                {
                    _quickInfos.Add(f.Name, f.Description);
                }
            });

            ShaderlabDataManager.Instance.UnityBuiltinMacros.ForEach((f) =>
            {

                string description = string.Format("{0}\n{1}", string.Join(";\n", f.Synopsis), f.Description);
                if (_quickInfos.ContainsKey(f.Name))
                {
                    _quickInfos[f.Name] = _quickInfos[f.Name] + string.Format("\nUnity3D built-in macros: {0}", description);
                }
                else
                {
                    _quickInfos.Add(f.Name, description);
                }
            });

            ShaderlabDataManager.Instance.UnityKeywords.ForEach((k) =>
            {
                if (_quickInfos.ContainsKey(k.Name))
                {
                    _quickInfos[k.Name] = _quickInfos[k.Name] + string.Format("\nUnity3D keywords: {0}", k.Description);
                }
                else
                {
                    _quickInfos.Add(k.Name, k.Description);
                }
            });

            ShaderlabDataManager.Instance.UnityBuiltinValues.ForEach((v) =>
            {
                if (_quickInfos.ContainsKey(v.Name))
                {
                    _quickInfos[v.Name] = _quickInfos[v.Name] + string.Format("\nUnity3d built-in values: {0}", v.VauleDescription);
                }
                else
                {
                    _quickInfos.Add(v.Name, v.VauleDescription);
                }
            });
        }

    }
}
