using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using System.Collections.Concurrent;
using Range = OmniSharp.Extensions.LanguageServer.Protocol.Models.Range;

namespace ShaderLS
{
    public class BufferService
    {
        private readonly ConcurrentDictionary<DocumentUri, Buffer> _buffers = new();

        public void Add(DocumentUri key, string text)
        {
            _buffers.TryAdd(key, new Buffer(text));
        }

        public void Remove(DocumentUri key)
        {
            _buffers.TryRemove(key, out _);
        }

        public void ApplyFullChange(DocumentUri key, string text)
        {
            var buffer = _buffers[key];
            _buffers.TryUpdate(key, new Buffer(text), buffer);
        }

        public void ApplyIncrementalChange(DocumentUri key, Range range, string text)
        {
            var buffer = _buffers[key];
            var newText = Splice(buffer.GetText(), range, text);
            _buffers.TryUpdate(key, new Buffer(newText), buffer);
        }

        public string GetText(DocumentUri key)
        {
            return _buffers[key].GetText();
        }

        public string? GetLine(DocumentUri key, Position position)
        {
            return _buffers[key].GetLine(position);
        }

        public List<string>? GetLineSplit(DocumentUri key, Position position)
        {
            return _buffers[key].GetLineSplit(position);
        }

        public string GetWordAtPosition(DocumentUri key, Position position)
        {
            return _buffers[key].GetWordAtPosition(position);
        }

        public HashSet<string> Tokens(DocumentUri key)
        {
            return _buffers[key].Tokens();
        }

        private static int GetIndex(string buffer, Position position)
        {
            var index = 0;
            for (var i = 0; i < position.Line; ++i)
            {
                index = buffer.IndexOf('\n', index) + 1;
            }
            return index + position.Character;
        }

        private static string Splice(string buffer, Range range, string text)
        {
            var start = GetIndex(buffer, range.Start);
            var end = GetIndex(buffer, range.End);
            return buffer[..start] + text + buffer[end..];
        }
    }
}