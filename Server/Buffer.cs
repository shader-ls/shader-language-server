using ShaderlabVS;

namespace ShaderLS
{
    public class Buffer
    {
        private string _text = "";
        private HashSet<string> _tokens = new();

        public string GetText() { return _text; }
        public HashSet<string> Tokens() { return _tokens; }

        public Buffer(string text)
        {
            this._text = text;
            SetWordsInDocuments(text);
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

                string[] words = line.Split(
                    new char[] { '{', '}', ' ', '\t', '(', ')', '[', ']', '+', '-', '*', '/', '%', '^', '>', '<', ':',
                                '.', ';', '\"', '\'', '?', '\\', '&', '|', '`', '$', '#', ','},
                    StringSplitOptions.RemoveEmptyEntries);

                foreach (var word in words)
                {
                    _tokens.Add(word);
                }

                line = reader.ReadLine();
            }
        }
    }
}
