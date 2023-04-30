using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OmniSharp.Extensions.LanguageServer.Protocol;
using ShaderLS.LanguageServerProtocol;

namespace ShaderLS.Management
{
    public class ProjectInfo
    {
        private string _path = "";

        public string path { get { return _path; } }

        private ProjectInfo(string path)
        {
            this._path = path;
        }

        /// <summary>
        /// Return the project root if possible.
        /// </summary>
        public static ProjectInfo? Find(DocumentUri uri)
        {
            string path = Helpers.FromUri(uri.Path);

            if (!File.Exists(path))
                return null;

            string root = Directory.GetDirectoryRoot(path);
            string current = Path.GetDirectoryName(path);

            FileInfo[] files = new FileInfo[0];

            while (root != current && files.Length == 0)
            {
                var di = new DirectoryInfo(current);
                files = di.GetFiles("*.csproj", SearchOption.TopDirectoryOnly);
                if (files.Length != 0)
                    break;
                current = Path.GetDirectoryName(current);
            }

            if (files.Length == 0)
                return null;

            var info = new ProjectInfo(current);

            return info;
        }
    }
}
