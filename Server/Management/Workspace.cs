using OmniSharp.Extensions.LanguageServer.Protocol;

namespace ShaderLS.Management
{
    public class Document
    {
        private string _path = "";
        private string _text = "";

        public string Path { get { return this._path; } set { this._path = value; } }
        public string Text { get { return this._text; } set { this._text = value; } }

        public Document(string path, string text)
        {
            this._path = path;
            this._text = text;
        }
    }

    public class Workspace
    {
        private BufferService _buffersServ = new BufferService();

        private ProjectInfo? _info;

        public ProjectInfo? Info { get { return this._info; } }
        public BufferService BufferService { get { return this._buffersServ; } }

        public void Init(DocumentUri uri)
        {
            this._info = ProjectInfo.Find(uri);
        }
    }
}
