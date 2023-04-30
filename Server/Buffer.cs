using System.Numerics;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using ShaderlabVS;
using static System.Net.Mime.MediaTypeNames;

namespace ShaderLS
{
    public class Buffer
    {
        private string _text = "";  // full buffer
        private HashSet<string> _tokens = new();


        public string GetText() { return _text; }
        public HashSet<string> Tokens() { return _tokens; }

        public Buffer(string text)
        {
            this._text = text;
            SetWordsInDocuments(text);
        }

        public string GetWordAtPosition(Position position)
        {
            if (position == null)
                return null;

            string[] lines = _text.Split("\n");

            int line = position.Line;
            int column = position.Character;

            string lineStr = lines[line];

            string[] words = GetWords(lineStr);

            int currentColumn = 0;

            string on = words[0];

            foreach (var word in words)
            {
                if (column <= currentColumn)
                    break;

                currentColumn = lineStr.IndexOf(word, currentColumn);
                currentColumn += word.Length;  // push to the end
                on = word;
            }

            return on;
        }

        private void SetWordsInDocuments(string text)
        {
            var reader = new StringReader(text);

            string line = reader.ReadLine();

            while (line != null)
            {
                if (Utilities.IsCommentLine(line))
                {
                    line = reader.ReadLine();
                    continue;
                }

                string[] words = GetWords(line);

                foreach (var word in words)
                {
                    _tokens.Add(word);
                }

                line = reader.ReadLine();
            }
        }

        private string[] GetWords(string text)
        {
            return text.Split(
                    new char[] { '{', '}', ' ', '\t', '(', ')', '[', ']', '+', '-', '*', '/', '%', '^', '>', '<', ':',
                                '.', ';', '\"', '\'', '?', '\\', '&', '|', '`', '$', '#', ','},
                    StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
