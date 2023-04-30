using Newtonsoft.Json.Linq;
using OmniSharp.Extensions.JsonRpc;
using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using Range = OmniSharp.Extensions.LanguageServer.Protocol.Models.Range;

namespace ShaderLS.LanguageServerProtocol
{
    public static class Helpers
    {
        public static DocumentUri ToUri(string fileName) => DocumentUri.File(fileName);
        public static string FromUri(DocumentUri uri) => uri.GetFileSystemPath().Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);

        public static Range ToRange((int column, int line) location)
        {
            return new Range()
            {
                Start = ToPosition(location),
                End = ToPosition(location)
            };
        }

        public static Position ToPosition((int column, int line) location)
        {
            return new Position(location.line, location.column);
        }

        public static Range ToRange((int column, int line) start, (int column, int line) end)
        {
            return new Range()
            {
                Start = new Position(start.line, start.column),
                End = new Position(end.line, end.column)
            };
        }
    }

    public static class CommandExtensions
    {
        public static T ExtractArguments<T>(this IExecuteCommandParams @params, ISerializer serializer)
            where T : notnull =>
            ExtractArguments<T>(@params.Arguments, serializer);

        public static T ExtractArguments<T>(this Command @params, ISerializer serializer)
            where T : notnull =>
            ExtractArguments<T>(@params.Arguments, serializer);

        public static (T arg1, T2 arg2) ExtractArguments<T, T2>(this IExecuteCommandParams command, ISerializer serializer)
            where T : notnull
            where T2 : notnull =>
            ExtractArguments<T, T2>(command.Arguments, serializer);

        public static (T arg1, T2 arg2) ExtractArguments<T, T2>(this Command command, ISerializer serializer)
            where T : notnull
            where T2 : notnull =>
            ExtractArguments<T, T2>(command.Arguments, serializer);

        public static (T arg1, T2 arg2, T3 arg3) ExtractArguments<T, T2, T3>(this IExecuteCommandParams command, ISerializer serializer)
            where T : notnull
            where T2 : notnull
            where T3 : notnull =>
            ExtractArguments<T, T2, T3>(command.Arguments, serializer);

        public static (T arg1, T2 arg2, T3 arg3) ExtractArguments<T, T2, T3>(this Command command, ISerializer serializer)
            where T : notnull
            where T2 : notnull
            where T3 : notnull =>
            ExtractArguments<T, T2, T3>(command.Arguments, serializer);

        public static (T arg1, T2 arg2, T3 arg3, T4 arg4) ExtractArguments<T, T2, T3, T4>(this IExecuteCommandParams command, ISerializer serializer)
            where T : notnull
            where T2 : notnull
            where T3 : notnull
            where T4 : notnull =>
            ExtractArguments<T, T2, T3, T4>(command.Arguments, serializer);

        public static (T arg1, T2 arg2, T3 arg3, T4 arg4) ExtractArguments<T, T2, T3, T4>(this Command command, ISerializer serializer)
            where T : notnull
            where T2 : notnull
            where T3 : notnull
            where T4 : notnull =>
            ExtractArguments<T, T2, T3, T4>(command.Arguments, serializer);

        public static (T arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) ExtractArguments<T, T2, T3, T4, T5>(this IExecuteCommandParams command, ISerializer serializer)
            where T : notnull
            where T2 : notnull
            where T3 : notnull
            where T4 : notnull
            where T5 : notnull =>
            ExtractArguments<T, T2, T3, T4, T5>(command.Arguments, serializer);

        public static (T arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) ExtractArguments<T, T2, T3, T4, T5>(this Command command, ISerializer serializer)
            where T : notnull
            where T2 : notnull
            where T3 : notnull
            where T4 : notnull
            where T5 : notnull =>
            ExtractArguments<T, T2, T3, T4, T5>(command.Arguments, serializer);

        public static (T arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) ExtractArguments<T, T2, T3, T4, T5, T6>(this IExecuteCommandParams command, ISerializer serializer)
            where T : notnull
            where T2 : notnull
            where T3 : notnull
            where T4 : notnull
            where T5 : notnull
            where T6 : notnull =>
            ExtractArguments<T, T2, T3, T4, T5, T6>(command.Arguments, serializer);

        public static (T arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) ExtractArguments<T, T2, T3, T4, T5, T6>(this Command command, ISerializer serializer)
            where T : notnull
            where T2 : notnull
            where T3 : notnull
            where T4 : notnull
            where T5 : notnull
            where T6 : notnull =>
            ExtractArguments<T, T2, T3, T4, T5, T6>(command.Arguments, serializer);

        private static T ExtractArguments<T>(JArray args, ISerializer serializer)
            where T : notnull
        {
            args ??= new JArray();
            T arg1 = default;
            if (args.Count > 0) arg1 = args[0].ToObject<T>(serializer.JsonSerializer);
            return arg1!;
        }

        private static (T arg1, T2 arg2) ExtractArguments<T, T2>(JArray args, ISerializer serializer)
            where T : notnull
            where T2 : notnull
        {
            args ??= new JArray();
            T arg1 = default;
            if (args.Count > 0) arg1 = args[0].ToObject<T>(serializer.JsonSerializer);
            T2 arg2 = default;
            if (args.Count > 1) arg2 = args[1].ToObject<T2>(serializer.JsonSerializer);

            return (arg1!, arg2!);
        }

        private static (T arg1, T2 arg2, T3 arg3) ExtractArguments<T, T2, T3>(JArray args, ISerializer serializer)
            where T : notnull
            where T2 : notnull
            where T3 : notnull
        {
            args ??= new JArray();
            T arg1 = default;
            if (args.Count > 0) arg1 = args[0].ToObject<T>(serializer.JsonSerializer);
            T2 arg2 = default;
            if (args.Count > 1) arg2 = args[1].ToObject<T2>(serializer.JsonSerializer);
            T3 arg3 = default;
            if (args.Count > 2) arg3 = args[2].ToObject<T3>(serializer.JsonSerializer);

            return (arg1!, arg2!, arg3!);
        }

        private static (T arg1, T2 arg2, T3 arg3, T4 arg4) ExtractArguments<T, T2, T3, T4>(JArray args, ISerializer serializer)
            where T : notnull
            where T2 : notnull
            where T3 : notnull
            where T4 : notnull
        {
            args ??= new JArray();
            T arg1 = default;
            if (args.Count > 0) arg1 = args[0].ToObject<T>(serializer.JsonSerializer);
            T2 arg2 = default;
            if (args.Count > 1) arg2 = args[1].ToObject<T2>(serializer.JsonSerializer);
            T3 arg3 = default;
            if (args.Count > 2) arg3 = args[2].ToObject<T3>(serializer.JsonSerializer);
            T4 arg4 = default;
            if (args.Count > 3) arg4 = args[3].ToObject<T4>(serializer.JsonSerializer);

            return (arg1!, arg2!, arg3!, arg4!);
        }

        private static (T arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) ExtractArguments<T, T2, T3, T4, T5>(JArray args, ISerializer serializer)
            where T : notnull
            where T2 : notnull
            where T3 : notnull
            where T4 : notnull
            where T5 : notnull
        {
            args ??= new JArray();
            T arg1 = default;
            if (args.Count > 0) arg1 = args[0].ToObject<T>(serializer.JsonSerializer);
            T2 arg2 = default;
            if (args.Count > 1) arg2 = args[1].ToObject<T2>(serializer.JsonSerializer);
            T3 arg3 = default;
            if (args.Count > 2) arg3 = args[2].ToObject<T3>(serializer.JsonSerializer);
            T4 arg4 = default;
            if (args.Count > 3) arg4 = args[3].ToObject<T4>(serializer.JsonSerializer);
            T5 arg5 = default;
            if (args.Count > 4) arg5 = args[4].ToObject<T5>(serializer.JsonSerializer);

            return (arg1!, arg2!, arg3!, arg4!, arg5!);
        }

        private static (T arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) ExtractArguments<T, T2, T3, T4, T5, T6>(JArray args, ISerializer serializer)
            where T : notnull
            where T2 : notnull
            where T3 : notnull
            where T4 : notnull
            where T5 : notnull
            where T6 : notnull
        {
            args ??= new JArray();
            T arg1 = default;
            if (args.Count > 0) arg1 = args[0].ToObject<T>(serializer.JsonSerializer);
            T2 arg2 = default;
            if (args.Count > 1) arg2 = args[1].ToObject<T2>(serializer.JsonSerializer);
            T3 arg3 = default;
            if (args.Count > 2) arg3 = args[2].ToObject<T3>(serializer.JsonSerializer);
            T4 arg4 = default;
            if (args.Count > 3) arg4 = args[3].ToObject<T4>(serializer.JsonSerializer);
            T5 arg5 = default;
            if (args.Count > 4) arg5 = args[4].ToObject<T5>(serializer.JsonSerializer);
            T6 arg6 = default;
            if (args.Count > 5) arg6 = args[5].ToObject<T6>(serializer.JsonSerializer);

            return (arg1!, arg2!, arg3!, arg4!, arg5!, arg6!);
        }
    }
}
