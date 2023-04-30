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
        private static Dictionary<string, Workspace> _workspaces = new Dictionary<string, Workspace>();

        private BufferService _buffersServ = new BufferService();

        private ProjectInfo? _info;

        public ProjectInfo? Info { get { return this._info; } }
        public BufferService BufferService { get { return this._buffersServ; } }

        public Workspace(ProjectInfo? info)
        {
            this._info = info;
        }

        public static Workspace Get(DocumentUri uri)
        {
            var info = ProjectInfo.Find(uri);

            Workspace workspace;

            // When project root found!
            if (info != null)
            {
                if (_workspaces.ContainsKey(info.path))
                    workspace = _workspaces[info.path];
                else
                {
                    workspace = new Workspace(info);
                    _workspaces.Add(info.path, workspace);
                }
            }
            else
            {
                // Single file, no project root!
                workspace = new Workspace(info);
                _workspaces.Add(uri.Path, workspace);
            }

            return workspace;
        }
    }
}
